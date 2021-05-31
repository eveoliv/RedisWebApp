using System;
using System.Data.Common;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : Controller
    {
        private readonly IDistributedCache distributedCache;
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public RedisController(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            this.distributedCache = distributedCache;
            this.connectionMultiplexer = connectionMultiplexer;
        }

        [HttpPost("inserir")]
        public IActionResult CreateKeys(string key, string valor)
        {
            if (!PesquisaChave(key))
                distributedCache.SetString(key, valor);
           
            return StatusCode(200, $"Chave [{key}]-[{valor}] inseridos com sucesso.");
        }

        [HttpGet("buscar")]
        public IActionResult GetKey(string key)
        {
            if (PesquisaChave(key))
                return StatusCode(200, $"Chave [{key}] encontrada.");

            return StatusCode(404, $"Chave [{key}] não encontrada.");
        }

        [HttpPost("remover")]
        public IActionResult DeleteKey(string key)
        {
            if (PesquisaChave(key))
            {
                try
                {                    
                    distributedCache.RemoveAsync(key);
                    return StatusCode(200, $"Chave [{key}] deletada com sucesso.");
                }
                catch (DbException e)
                {
                    throw new Exception(e.Message);
                }
            }
            return StatusCode(404, $"Chave [{key}] não encontrada.");
        }

        private bool PesquisaChave(string key)
        {
            try
            {
                if (connectionMultiplexer.GetDatabase().KeyExists(key))
                    return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return false;
        }
    }
}
