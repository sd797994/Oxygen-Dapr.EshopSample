using Autofac;
using InfrastructureBase.Http;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.Data.Nest
{
    internal class NestClientProvider
    {
        private static Lazy<ElasticClient> esClient = new Lazy<ElasticClient>(() =>
        {
            var conStr = HttpContextExt.Current.RequestService.Resolve<IConfiguration>().GetSection("ElasticsearchConnectionString").Value;
            var client = new ElasticClient(new ConnectionSettings(new Uri(conStr)));
            return client;
        });
        public static ElasticClient GetClient()
        {
            return esClient.Value;
        }
    }
}
