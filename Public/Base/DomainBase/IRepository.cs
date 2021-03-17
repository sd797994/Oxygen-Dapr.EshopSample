using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DomainBase
{
    public interface IRepository<T> where T : Entity
    {
        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        void Add(T t);
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        void Update(T t);
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        void Delete(T t);
        /// <summary>
        /// 根据条件删除对象
        /// </summary>
        /// <param name="t"></param>
        void Delete(Expression<Func<T, bool>> condition);
        /// <summary>
        /// 根据主键获取对象
        /// </summary>
        /// <returns></returns>
        Task<T> GetAsync(object key = null);
        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(object key = null);
        /// <summary>
        /// 根据条件判断对象是否存在
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> condition);
        /// <summary>
        /// 根据主键获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        IAsyncEnumerable<T> GetManyAsync(Guid[] key);
        /// <summary>
        /// 根据条件获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IAsyncEnumerable<T> GetManyAsync(Expression<Func<T, bool>> condition);
    }
}
