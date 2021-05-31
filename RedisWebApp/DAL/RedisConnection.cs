using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace RedisWebApp.DAL
{
    public class RedisConnection
    {
        //exemplo de implementacao manual ConnectionMultiplexer, nao utilizada nesse projeto
        private ConnectionMultiplexer _conexao;

        public RedisConnection(IConfiguration configuration)
        {
            _conexao = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisServer"));
        }

        public string GetValueFromKey(string key)
        {
            var dbRedis = _conexao.GetDatabase();
            return dbRedis.StringGet(key);
        }
    }
}
