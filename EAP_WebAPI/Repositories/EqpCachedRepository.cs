using EAP_Library.DTO;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAP_WebAPI.Repositories
{
    public class EqpCachedRepository
    {
        private IRedisCacheClient _RedisClient;
        private string _HashKey = "EqpInfo";
        public EqpCachedRepository(IRedisCacheClient redisClient) {
            this._RedisClient = redisClient;
        }
        public Task Add(EqpInfoDTO eqpInfo) {
            return this._RedisClient.Db0.HashSetAsync<EqpInfoDTO>(this._HashKey,eqpInfo.Id, eqpInfo);
        }
        public async Task<EqpInfoDTO> GetAsync(string eqpId)
        {
            return await _RedisClient.Db0.HashGetAsync<EqpInfoDTO>(this._HashKey, eqpId);
        }
        public async Task<Dictionary< string,EqpInfoDTO>> GetAllAsync()
        {
            return await _RedisClient.Db0.HashGetAllAsync< EqpInfoDTO>(this._HashKey);
        }
        public async Task<bool> DeleteAsync(string eqpId)
        {
            return await _RedisClient.Db0.HashDeleteAsync(this._HashKey, eqpId);
        }
    }
}
