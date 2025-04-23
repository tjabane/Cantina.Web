using NRedisStack.RedisStackCommands;
using NRedisStack;
using StackExchange.Redis;
using Cantina.SyncService.Data;
using System.Text.Json;
using NRedisStack.Search.Literals.Enums;
using NRedisStack.Search;
using System.Linq;

namespace Cantina.SyncService
{
    public class Worker : BackgroundService
    {
        private readonly JsonCommands _jsonCmd;
        private readonly SearchCommands _searchCmd;
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            var host = _configuration.GetConnectionString("Redis");
            var redis = ConnectionMultiplexer.Connect(host);
            var db = redis.GetDatabase();
            _jsonCmd = db.JSON();
            _searchCmd = db.FT();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try {
                    await ProcessMenu();
                    await ProcessReviews();
                }
                catch(Exception ex)
                {
                    _logger.LogWarning(ex, "Error fetching menu items: {message}", ex.Message);
                }
                await Task.Delay(10000, stoppingToken);
            }
        }


        private async Task ProcessMenu()
        {
            var items = await DB.GetMenuItems(_connectionString);
            _logger.LogInformation("From DB Menu ItemsCount: {time}", items.Count);
            await AddMenuToRedis(items);
            var redisItems = await _searchCmd.SearchAsync("menu-index", new Query("*"));
            _logger.LogInformation("From Redis Menu ItemsCount: {time}", redisItems.TotalResults);
        }

        private async Task ProcessReviews()
        {
            var items = await DB.GetReviews(_connectionString);
            _logger.LogInformation("From DB Reviews ItemsCount: {time}", items.Count);
            await AddReviewsToRedis(items);
            var redisItems = await _searchCmd.SearchAsync("review-index", new Query("*"));
            _logger.LogInformation("From Redis Reviews ItemsCount: {time}", redisItems.TotalResults);
        }

        private async Task AddReviewsToRedis(List<Review> items)
        {
            await CreateReviewsIndex();
            var insertRequests = items.Select(item =>
            {
                var jsonData = JsonSerializer.Serialize(item);
                return _jsonCmd.SetAsync($"review:{item.Id}", "$", item);
            }).ToArray();
            Task.WaitAll(insertRequests);
        }

        private async Task CreateReviewsIndex()
        {
            if (IndexExists("review-index")) return;
            try { _searchCmd.DropIndex("review-index"); } catch { }
            await _searchCmd.CreateAsync("reviews-index", new FTCreateParams().On(IndexDataType.JSON)
                                                    .Prefix("review:"),
                                            new Schema().AddNumericField(new FieldName("$.Id", "id"))
                                                        .AddTextField(new FieldName("$.Comment", "comment"))
                                                        .AddNumericField(new FieldName("$.Rating", "rating"), sortable: true)
                                                        .AddTextField(new FieldName("$.UserId", "userId"))
                                                        .AddTextField(new FieldName("$.MenuItemId", "menuItemId"))
                                                        );
        }

        private async Task AddMenuToRedis(List<MenuItem> menuItems)
        {
            await CreateMenuIndex();
            var insertRequests = menuItems.Select(item =>
            {
                var jsonData = JsonSerializer.Serialize(item);
                return _jsonCmd.SetAsync($"menu:{item.Id}", "$", item);
            }).ToArray();

            Task.WaitAll(insertRequests);
        }

        private async Task CreateMenuIndex()
        {
            if (IndexExists("menu-index"))
                return;
            try { _searchCmd.DropIndex("menu-index"); } catch { }
            await _searchCmd.CreateAsync("menu-index", new FTCreateParams().On(IndexDataType.JSON)
                                                    .Prefix("menu:"),
                                            new Schema().AddNumericField(new FieldName("$.Id", "id"))
                                                        .AddTextField(new FieldName("$.Name", "name"))
                                                        .AddTextField(new FieldName("$.Description", "description"))
                                                        .AddNumericField(new FieldName("$.Price", "price"), sortable: true)
                                                        .AddNumericField(new FieldName("$.Rating", "rating"), sortable: true)
                                                        );
        }

        private bool IndexExists(string indexName)
        {
            try
            {
                var existingIndexes = _searchCmd._List();
                if(existingIndexes is not null)
                    return existingIndexes.Any(index => index.ToString() == indexName);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error checking if index exists: {message}", ex.Message);
                return false;
            }
        }
    }
}
