using Autofac;
using Domain.Entities;
using Domain.Repository;
using IApplicationService.AppEvent;
using IApplicationService.GoodsService.Dtos.Event;
using IApplicationService.TradeService;
using IApplicationService.TradeService.Dtos.Input;
using IApplicationService.TradeService.Dtos.Output;
using Infrastructure.EfDataAccess;
using Infrastructure.Elasticsearch;
using InfrastructureBase;
using InfrastructureBase.Data.Nest;
using InfrastructureBase.Http;
using InfrastructureBase.Object;
using Microsoft.EntityFrameworkCore;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService
{
    public class EventHandler : IEventHandler
    {
        private readonly IGoodsRepository repository;
        private readonly IGoodsCategoryRepository categoryRepository;
        private readonly ILimitedTimeActivitieRepository limitrepository;
        private readonly IUnitofWork unitofWork;
        private readonly IEventBus eventBus;
        private readonly IStateManager stateManager;
        private readonly IEsGoodsRepository esGoodsRepository;
        private readonly EfDbContext efDbContext;
        private readonly IElasticSearchRepository<EsGoodsDto> searchRepository;
        private readonly IOrderQueryService orderQueryService;
        private readonly ILocalEventBus localEventBus;
        public EventHandler(IGoodsRepository repository, IGoodsCategoryRepository categoryRepository, IEsGoodsRepository esGoodsRepository, 
                ILimitedTimeActivitieRepository limitrepository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork,
                EfDbContext efDbContext, IElasticSearchRepository<EsGoodsDto> searchRepository, IOrderQueryService orderQueryService,
                ILocalEventBus localEventBus
                )
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.limitrepository = limitrepository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
            this.esGoodsRepository = esGoodsRepository;
            this.efDbContext = efDbContext;
            this.searchRepository = searchRepository;
            this.orderQueryService = orderQueryService;
            this.localEventBus = localEventBus;
        }

        [EventHandlerFunc(EventTopicDictionary.Account.InitTestUserSuccess)]
        public async Task<DefaultEventHandlerResponse> EventHandleSetDefMallSetting(EventHandleRequest<string> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(EventHandleSetDefMallSetting), input.GetDataJson(), async () =>
            {
                var categorys = new List<GoodsCategory>()
                {
                    new GoodsCategory() { CategoryName = "精选热菜", Sort = 0 },
                    new GoodsCategory() { CategoryName = "爽口凉菜", Sort = 1 },
                    new GoodsCategory() { CategoryName = "小吃主食", Sort = 2 },
                    new GoodsCategory() { CategoryName = "特色粥品", Sort = 3 }
                };
                categoryRepository.Add(categorys[0]);
                categoryRepository.Add(categorys[1]);
                categoryRepository.Add(categorys[2]);
                categoryRepository.Add(categorys[3]);
                var willSend = new List<Goods>();
                Goods createGoods(string name, string url, decimal price, Guid categoryid)
                {
                    var goods = new Goods();
                    goods.CreateOrUpdateGoods(name, url, price, categoryid);
                    goods.UpOrDownShelf(true);
                    goods.ChangeStock(100);
                    repository.Add(goods);
                    willSend.Add(goods);
                    return goods;
                }
                void createLimit(Goods goods)
                {
                    var entity = new LimitedTimeActivitie();
                    entity.CreateOrUpdate(goods.GoodsName, goods.Id, goods.Price - new Random(Guid.NewGuid().GetHashCode()).Next(1, 4), DateTime.Now, DateTime.Now.AddDays(Random.Shared.Next(1, 20)));
                    entity.UpOrDownShelf(true);
                    limitrepository.Add(entity);
                }
                createGoods("娃娃菜炖豆腐", "http://fuss10.elemecdn.com/d/2d/b1eb45b305635d9dd04ddf157165fjpeg.jpeg?imageView2/1/w/114/h/114", 17, categorys[0].Id);
                createGoods("手撕包菜", "http://fuss10.elemecdn.com/9/c6/f3bc84468820121112e79583c24efjpeg.jpeg?imageView2/1/w/114/h/114", 16, categorys[0].Id);
                var goods1 = createGoods("香酥黄金鱼/3条", "http://fuss10.elemecdn.com/4/e7/8277a6a2ea0a2e97710290499fc41jpeg.jpeg?imageView2/1/w/114/h/114", 11, categorys[0].Id);
                createGoods("八宝酱菜", "http://fuss10.elemecdn.com/9/b5/469d8854f9a3a03797933fd01398bjpeg.jpeg?imageView2/1/w/114/h/114", 4, categorys[1].Id);
                createGoods("拍黄瓜", "http://fuss10.elemecdn.com/6/54/f654985b4e185f06eb07f8fa2b2e8jpeg.jpeg?imageView2/1/w/114/h/114", 9, categorys[1].Id);
                createGoods("扁豆焖面", "http://fuss10.elemecdn.com/c/6b/29e3d29b0db63d36f7c500bca31d8jpeg.jpeg?imageView2/1/w/114/h/114", 14, categorys[2].Id);
                createGoods("葱花饼", "http://fuss10.elemecdn.com/f/28/a51e7b18751bcdf871648a23fd3b4jpeg.jpeg?imageView2/1/w/114/h/114", 10, categorys[2].Id);
                var goods2 = createGoods("牛肉馅饼", "http://fuss10.elemecdn.com/d/b9/bcab0e8ad97758e65ae5a62b2664ejpeg.jpeg?imageView2/1/w/114/h/114", 14, categorys[2].Id);
                createGoods("招牌猪肉白菜锅贴/10个", "http://fuss10.elemecdn.com/7/72/9a580c1462ca1e4d3c07e112bc035jpeg.jpeg?imageView2/1/w/114/h/114", 7, categorys[2].Id);
                createGoods("皮蛋瘦肉粥", "http://fuss10.elemecdn.com/c/cd/c12745ed8a5171e13b427dbc39401jpeg.jpeg?imageView2/1/w/114/h/114", 10, categorys[3].Id);
                var goods3 = createGoods("红豆薏米美肤粥", "http://fuss10.elemecdn.com/d/22/260bd78ee6ac6051136c5447fe307jpeg.jpeg?imageView2/1/w/114/h/114", 12, categorys[3].Id);
                createGoods("红枣山药糙米粥", "http://fuss10.elemecdn.com/9/b5/469d8854f9a3a03797933fd01398bjpeg.jpeg?imageView2/1/w/114/h/114", 10, categorys[3].Id);
                var goods4 = createGoods("鲜蔬菌菇粥", "http://fuss10.elemecdn.com/e/a3/5317c68dd618929b6ac05804e429ajpeg.jpeg?imageView2/1/w/114/h/114", 11, categorys[3].Id);
                createLimit(goods1);
                createLimit(goods2);
                createLimit(goods3);
                createLimit(goods4);
                if (await unitofWork.CommitAsync())
                    await localEventBus.SendEvent(EventTopicDictionary.Goods.Loc_WriteToElasticsearch, willSend);
            });
        }
        [EventHandlerFunc(EventTopicDictionary.Goods.UpdateGoodsToEs)]
        public async Task<DefaultEventHandlerResponse> EventHandleEventUpdateGoodsToEs(EventHandleRequest<UpdateGoodsToEsDto> input)
        {
            return await new DefaultEventHandlerResponse().RunAsync(nameof(EventHandleEventUpdateGoodsToEs), input.GetDataJson(), async () =>
            {
                var lastUpdateTime = DateTime.Now.AddMinutes(-10);
                var now = DateTime.Now;
                var result = await (from a in efDbContext.Goods.Where(x => x.LastUpdateTime >= lastUpdateTime && x.ShelfState)//取10分钟以为更新的数据
                                    join b in efDbContext.GoodsCategory on a.CategoryId equals b.Id
                                    join c in efDbContext.LimitedTimeActivitie.DefaultIfEmpty().Where(x => x.ShelfState && x.StartTime < now && x.EndTime > now) on a.Id equals c.GoodsId into tmp1
                                    from c1 in tmp1.DefaultIfEmpty()
                                    select new EsGoodsDto()
                                    {
                                        CategoryId = a.CategoryId,
                                        CategoryName = b.CategoryName,
                                        Id = a.Id,
                                        Name = a.GoodsName,
                                        Price = c1 == null ? a.Price : c1.ActivitiePrice,
                                        OldPrice = a.Price,
                                        SellCount = 0,
                                        Icon = a.GoodsImage,
                                    }).ToArrayAsync();
                if (result.Any())
                {
                    var sellCount = (await orderQueryService.GetOrderSellCountByGoodsId(new GetOrderSellCountByGoodsIdDto() { GoodsIds = result.Select(x => x.Id).ToArray(), ExpressTime = DateTime.Now.AddMonths(-1) })).GetData<List<GetOrderSellCountResponse>>();
                    foreach (var esgoods in result)
                    {
                        var esgoodssellcount = sellCount.FirstOrDefault(x => x.GoodsId == esgoods.Id);
                        esgoods.SellCount = esgoodssellcount?.CellCount ?? 0;
                    }
                }
                await searchRepository.GetRepo("goods").SaveData(result);
            });
        }
    }
}
