using DomainBase;
using System;

namespace Domain.Entities
{
    /// <summary>
    /// 领域实体
    /// </summary>
    public class Goods : Entity, IAggregateRoot
    {
        public Guid CategoryId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsImage { get; set; }
        public int Stock { get; set; }
        public bool ShelfState { get; set; }
        public decimal Price { get; set; }

        public void CreateOrUpdateGoods(string goodsName, string goodsImage, decimal price, Guid categoryId)
        {
            if (!string.IsNullOrEmpty(goodsName))
                GoodsName = goodsName;
            if (!string.IsNullOrEmpty(goodsImage))
                GoodsImage = goodsImage;
            CategoryId = categoryId;
            if (price <= 0)
                throw new DomainException("商品价格不能为0");
            Price = price;

        }
        public void UpOrDownShelf(bool shelfState)
        {
            ShelfState = shelfState;
        }
        public void ChangeStock(int stock)
        {
            if (stock < 0 || stock > 100000)
                throw new DomainException("库存不能小于0或大于100000");
            Stock = stock;
        }
        public void DeductionStock(int stock)
        {
            if (stock < 0)
                throw new DomainException("库存不能为0!");
            if (Stock < stock)
                throw new DomainException("库存不足!");
            else
                Stock -= stock;
        }
        public void RollbackDeductionStock(int stock)
        {
            Stock += stock;
        }
    }
}
