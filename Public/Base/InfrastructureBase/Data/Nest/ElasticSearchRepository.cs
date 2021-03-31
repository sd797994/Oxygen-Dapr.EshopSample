using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.Data.Nest
{
    public class ElasticSearchRepository<T> : IElasticSearchRepository<T> where T : class
    {
        public ElasticSearchRepository()
        {
            NestClientProvider.GetClient();
        }
        internal Func<SearchDescriptor<T>, ISearchRequest> search { get; set; }
        internal SortDescriptor<T> sortQueries { get; set; }
        internal List<Func<QueryContainerDescriptor<T>, QueryContainer>> mustQueries { get; set; }
        internal List<Func<QueryContainerDescriptor<T>, QueryContainer>> mustnotQueries { get; set; }
        internal List<Func<QueryContainerDescriptor<T>, QueryContainer>> rangeQueries { get; set; }
        string IndexName { get; set; }
        bool Init { get; set; }
        internal bool InitPage { get; set; }
        internal SearchDescriptor<T> searchParams { get; set; }
        public IElasticSearchRepository<T> GetRepo(string indexName)
        {
            searchParams = new SearchDescriptor<T>();
            sortQueries = new SortDescriptor<T>();
            mustQueries = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
            mustnotQueries = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
            rangeQueries = new List<Func<QueryContainerDescriptor<T>, QueryContainer>>();
            IndexName = indexName;
            Init = true;
            return this;
        }

        public async Task<List<T>> SearchData()
        {
            if (Init)
            {
                searchParams.Index(IndexName).Sort(s => sortQueries).Query(q => q.Bool(b => b.Must(mustQueries).MustNot(mustnotQueries).Filter(rangeQueries)));
                search = (s) => searchParams;
                var response = await NestClientProvider.GetClient().SearchAsync(search);
                if (response.ApiCall.Success)
                    return response.Documents.ToList();
                throw new InfrastructureException("数据访问失败!");
            }
            else
                throw new Exception("没有初始化ElasticSearchRepository!");
        }

        public async Task SaveData(params T[] data)
        {
            if (Init)
            {
                foreach (var item in data)
                    await NestClientProvider.GetClient().IndexAsync(item, idx => idx.Index(IndexName));
            }
            else
                throw new Exception("没有初始化ElasticSearchRepository!");
        }
        public async Task RemoveData(params T[] data)
        {
            if (Init)
            {
                foreach (var item in data)
                    await NestClientProvider.GetClient().DeleteAsync<T>(item, i => i.Index(IndexName));
            }
            else
                throw new Exception("没有初始化ElasticSearchRepository!");
        }
    }
}
