using DomainBase;

namespace Domain.Entities
{
    /// <summary>
    /// 领域实体
    /// </summary>
    public class GoodsCategory : Entity, IAggregateRoot
    {
        public string CategoryName { get; set; }
        public int Sort { get; set; }
        public void CreateOrUpdate(string categoryName, int sort)
        {
            if (!string.IsNullOrEmpty(categoryName))
                CategoryName = categoryName;
            if (sort < 0)
                throw new DomainException("分类排序必须大于0");
            Sort = sort;
        }
    }
}
