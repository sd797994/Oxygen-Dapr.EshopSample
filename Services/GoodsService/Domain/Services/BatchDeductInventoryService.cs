using Domain.Dtos;
using Domain.Entities;
using Domain.Repository;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class BatchDeductInventoryService
    {
        private List<Goods> goodsList;
        public BatchDeductInventoryService(List<Goods> goodsList)
        {
            this.goodsList = goodsList;
        }
        public List<Goods> BatchDeductInventory(DeductionStockDto dto)
        {
            if (goodsList.Count < dto.Items.Count)
                throw new DomainException("库存扣除失败，部分商品未找到");
            goodsList.ForEach(x =>
            {
                var dtoItem = dto.Items.FirstOrDefault(y => y.GoodsId == x.Id);
                x.DeductionStock(dtoItem.Count);
            });
            return goodsList;
        }
    }
    public class BatchRollbackDeductInventoryService
    {
        private List<Goods> goodsList;
        public BatchRollbackDeductInventoryService(List<Goods> goodsList)
        {
            this.goodsList = goodsList;
        }
        public List<Goods> BatchRollbackDeductInventory(DeductionStockDto dto)
        {
            if (goodsList.Count < dto.Items.Count)
                throw new DomainException("库存回滚失败，部分商品未找到");
            goodsList.ForEach(x =>
            {
                var dtoItem = dto.Items.FirstOrDefault(y => y.GoodsId == x.Id);
                x.RollbackDeductionStock(dtoItem.Count);
            });
            return goodsList;
        }
    }
}
