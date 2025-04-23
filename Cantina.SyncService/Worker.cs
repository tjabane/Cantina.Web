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
        private readonly string reviewsIndexName = "review-index";

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
            await AddMenuToRedis(items);
            var redisItems = await _searchCmd.SearchAsync("menu-index", new Query("*"));
            _logger.LogInformation("From Redis Menu ItemsCount: {count}", redisItems.TotalResults);
        }

        private async Task ProcessReviews()
        {
            var reviews = await DB.GetReviews(_connectionString);
            _logger.LogInformation("From DB Reviews ItemsCount: {count}", reviews.Count);
            await AddReviewsToRedis(reviews);
            var redisItems = await _searchCmd.SearchAsync(reviewsIndexName, new Query("*"));
            _logger.LogInformation("From Redis Reviews ItemsCount: {count}", redisItems.TotalResults);
        }

        private async Task AddReviewsToRedis(List<Review> items)
        {
            await CreateReviewsIndex();
            var insertRequests = items.Select(item => _jsonCmd.SetAsync($"review:{item.Id}", "$", item)).ToArray();
            Task.WaitAll(insertRequests);
        }

        private async Task CreateReviewsIndex()
        {
            if (IndexExists(reviewsIndexName))
                return;
            try { _searchCmd.DropIndex(reviewsIndexName ); } catch { }
            await _searchCmd.CreateAsync(reviewsIndexName, new FTCreateParams().On(IndexDataType.JSON)
                                                    .Prefix("review:"),
                                            new Schema().AddNumericField(new FieldName("$.Id", "id"))
                                                        .AddTextField(new FieldName("$.Comment", "comment"))
                                                        .AddNumericField(new FieldName("$.Rating", "rating"), sortable: true)
                                                        .AddTextField(new FieldName("$.UserId", "userId"))
                                                        .AddNumericField(new FieldName("$.MenuItemId", "menuItemId"))
                                                        .AddTextField(new FieldName("$.TimeStamp", "timeStamp"))
                                                        );
        }

        private async Task AddMenuToRedis(List<MenuItem> menuItems)
        {
            await CreateMenuIndex();
            var insertRequests = menuItems.Select(item =>
            {
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
                return existingIndexes.Any(index => index.ToString() == indexName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error checking if index exists: {message}", ex.Message);
                return false;
            }
        }
    }
}
