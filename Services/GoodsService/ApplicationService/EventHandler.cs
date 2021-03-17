using Domain.Entities;
using Domain.Repository;
using IApplicationService.AppEvent;
using Infrastructure.EfDataAccess;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Events;
using System;
using System.Collections.Generic;
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
        public EventHandler(IGoodsRepository repository, IGoodsCategoryRepository categoryRepository, ILimitedTimeActivitieRepository limitrepository, IEventBus eventBus, IStateManager stateManager, IUnitofWork unitofWork)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.limitrepository = limitrepository;
            this.unitofWork = unitofWork;
            this.eventBus = eventBus;
            this.stateManager = stateManager;
        }

        [EventHandlerFunc(EventTopicDictionary.Account.InitTestUserSuccess)]
        public async Task<DefaultEventHandlerResponse> EventHandleSetDefMallSetting(EventHandleRequest<string> input)
        {
            var categorys = new List<GoodsCategory>()
            {
                new GoodsCategory() { CategoryName = "æ´—°»»≤À", Sort = 0 },
                new GoodsCategory() { CategoryName = "À¨ø⁄¡π≤À", Sort = 1 },
                new GoodsCategory() { CategoryName = "–°≥‘÷˜ ≥", Sort = 2 },
                new GoodsCategory() { CategoryName = "Ãÿ…´÷‡∆∑", Sort = 3 }
            };
            categoryRepository.Add(categorys[0]);
            categoryRepository.Add(categorys[1]);
            categoryRepository.Add(categorys[2]);
            categoryRepository.Add(categorys[3]);
            Goods createGoods(string name, string url, decimal price, Guid categoryid)
            {
                var goods = new Goods();
                goods.CreateOrUpdateGoods(name, url, price, categoryid);
                goods.UpOrDownShelf(true);
                goods.ChangeStock(100);
                repository.Add(goods);
                return goods;
            }
            void createLimit(Goods goods)
            {
                var entity = new LimitedTimeActivitie();
                entity.CreateOrUpdate(goods.GoodsName, goods.Id, goods.Price - new Random(Guid.NewGuid().GetHashCode()).Next(1, 4), DateTime.Now, DateTime.Now.AddDays(30));
                limitrepository.Add(entity);
            }
            createGoods("ÕﬁÕﬁ≤ÀÏ¿∂π∏Ø", "http://fuss10.elemecdn.com/d/2d/b1eb45b305635d9dd04ddf157165fjpeg.jpeg?imageView2/1/w/114/h/114", 17, categorys[0].Id);
            createGoods(" ÷À∫∞¸≤À", "http://fuss10.elemecdn.com/9/c6/f3bc84468820121112e79583c24efjpeg.jpeg?imageView2/1/w/114/h/114", 16, categorys[0].Id);
            var goods1 = createGoods("œ„À÷ª∆Ω”„/3Ãı", "http://fuss10.elemecdn.com/4/e7/8277a6a2ea0a2e97710290499fc41jpeg.jpeg?imageView2/1/w/114/h/114", 11, categorys[0].Id);
            createGoods("∞À±¶Ω¥≤À", "http://fuss10.elemecdn.com/9/b5/469d8854f9a3a03797933fd01398bjpeg.jpeg?imageView2/1/w/114/h/114", 4, categorys[1].Id);
            createGoods("≈ƒª∆πœ", "http://fuss10.elemecdn.com/6/54/f654985b4e185f06eb07f8fa2b2e8jpeg.jpeg?imageView2/1/w/114/h/114", 9, categorys[1].Id);
            createGoods("±‚∂πÏÀ√Ê", "http://fuss10.elemecdn.com/c/6b/29e3d29b0db63d36f7c500bca31d8jpeg.jpeg?imageView2/1/w/114/h/114", 14, categorys[2].Id);
            createGoods("¥–ª®±˝", "http://fuss10.elemecdn.com/f/28/a51e7b18751bcdf871648a23fd3b4jpeg.jpeg?imageView2/1/w/114/h/114", 10, categorys[2].Id);
            var goods2 = createGoods("≈£»‚œ⁄±˝", "http://fuss10.elemecdn.com/d/b9/bcab0e8ad97758e65ae5a62b2664ejpeg.jpeg?imageView2/1/w/114/h/114", 14, categorys[2].Id);
            createGoods("’–≈∆÷Ì»‚∞◊≤Àπ¯Ã˘/10∏ˆ", "http://fuss10.elemecdn.com/7/72/9a580c1462ca1e4d3c07e112bc035jpeg.jpeg?imageView2/1/w/114/h/114", 7, categorys[2].Id);
            createGoods("∆§µ∞ ›»‚÷‡", "http://fuss10.elemecdn.com/c/cd/c12745ed8a5171e13b427dbc39401jpeg.jpeg?imageView2/1/w/114/h/114", 10, categorys[3].Id);
            var goods3 = createGoods("∫Ï∂πﬁ≤√◊√¿∑Ù÷‡", "http://fuss10.elemecdn.com/d/22/260bd78ee6ac6051136c5447fe307jpeg.jpeg?imageView2/1/w/114/h/114", 12, categorys[3].Id);
            createGoods("∫Ï‘Ê…Ω“©≤⁄√◊÷‡", "http://fuss10.elemecdn.com/9/b5/469d8854f9a3a03797933fd01398bjpeg.jpeg?imageView2/1/w/114/h/114", 10, categorys[3].Id);
            var goods4 = createGoods("œ  ﬂæ˙πΩ÷‡", "http://fuss10.elemecdn.com/e/a3/5317c68dd618929b6ac05804e429ajpeg.jpeg?imageView2/1/w/114/h/114", 11, categorys[3].Id);
            createLimit(goods1);
            createLimit(goods2);
            createLimit(goods3);
            createLimit(goods4);
            await unitofWork.CommitAsync();
            return DefaultEventHandlerResponse.Default();
        }
    }
}
