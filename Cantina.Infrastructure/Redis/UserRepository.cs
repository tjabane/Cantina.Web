using Cantina.Core.Dto;
using Cantina.Core.Interface;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Infrastructure.Redis
{
    public class UserRepository(IConnectionMultiplexer redis) : IUserRepository
    {
        private readonly JsonCommands _jsonCMD = redis.GetDatabase().JSON();

        public async Task<User> GetById(int id)
        {
            return await _jsonCMD.GetAsync<User>($"user:{id}");
        }
    }
}
