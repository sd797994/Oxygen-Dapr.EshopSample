using Domain.Dtos;
using Domain.Repository;
using Domain.Services;
using IApplicationService.GoodsService;
using IApplicationService.GoodsService.Dtos.Input;
using IApplicationService.Sagas.CreateOrder.Dtos;
using IApplicationService.Sagas.CreateOrder.Handles;
using Infrastructure.EfDataAccess;
using InfrastructureBase;
using InfrastructureBase.Object;
using Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeductionStockDto = IApplicationService.Sagas.CreateOrder.Dtos.DeductionStockDto;

namespace ApplicationService
{
    public class GoodsSagaHandler : IGoodsHandler
    {
        private readonly IGoodsRepository repository;
        private readonly IUnitofWork unitofWork;
        public GoodsSagaHandler(IGoodsRepository repository, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.unitofWork = unitofWork;
        }
        public async Task<DeductionStockDto> DeductInventory(DeductionStockDto dto)
        {
            using var tran = await unitofWork.BeginTransactionAsync();
            try
            {
                var goods = new BatchDeductInventoryService(await repository.GetManyToListAsync(dto.Items.Select(x => x.GoodsId).ToArray()))
                        .BatchDeductInventory(dto.CopyTo<DeductionStockDto, Domain.Dtos.DeductionStockDto>());
                goods.ForEach(x => repository.Update(x));
                await unitofWork.CommitAsync(tran);
                return dto;
            }
            catch (Exception e)
            {
                throw new SagaException<DeductionStockDto>(dto, e.Message);
            }
        }

        public async Task InventoryRollback(DeductionStockDto dto)
        {
            using var tran = await unitofWork.BeginTransactionAsync();
            try
            {
                var goods = new BatchRollbackDeductInventoryService(await repository.GetManyToListAsync(dto.Items.Select(x => x.GoodsId).ToArray()))
                        .BatchRollbackDeductInventory(dto.CopyTo<DeductionStockDto, Domain.Dtos.DeductionStockDto>());
                goods.ForEach(x => repository.Update(x));
                await unitofWork.CommitAsync(tran);
            }
            catch (Exception e)
            {
                throw new SagaException<DeductionStockDto>(dto, e.Message);
            }
        }
    }
}
