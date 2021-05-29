using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Data.Common;

namespace RedisWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IDistributedCache distributedCache;       

        public RedisController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        [HttpPost("inserir")]
        public IActionResult CreateKeys(string chave, string valor)
        {
            distributedCache.SetString(chave, valor );

            return StatusCode(200);
        }

        [HttpGet("buscar")]
        public IActionResult GetKey(string chave)
        {        
            if( PesquisaChave(chave) )
                return StatusCode(200);

                return StatusCode(404);
        }

        [HttpPost("remover")]
        public IActionResult DeleteKey(string chave)
        {
            if (PesquisaChave(chave))
            {
                try
                {
                    distributedCache.RemoveAsync(chave);
                    return StatusCode(200);

                }
                catch (DbException e)
                {
                    throw new Exception(e.Message);
                }
            }

            return StatusCode(400);
        }

        private bool PesquisaChave(string chave)
        {
            //RedisValue verifica = distributedCache.GetString(chave);
            var verifica = distributedCache.GetAsync(chave);
           

            if (verifica != null)            
                return true;

            return false;
        }
    }
}
