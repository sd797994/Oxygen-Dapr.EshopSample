using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Elasticsearch
{
    public interface IEsGoodsRepository
    {
        Task WriteToElasticsearch(Goods goods);
        Task RemoveToElasticsearch(Goods goods);
    }
}
