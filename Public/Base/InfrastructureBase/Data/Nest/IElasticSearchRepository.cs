using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.Data.Nest
{
    public interface IElasticSearchRepository<T> where T : class
    {
        IElasticSearchRepository<T> GetRepo(string Index);
        Task<List<T>> SearchData();
        Task SaveData(params T[] data);
        Task RemoveData(params T[] item);
    }
}
