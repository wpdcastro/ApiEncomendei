using apiEncomendei.Daos;
using apiEncomendei.Entities;
using apiEncomendei.InputModels;
using apiEncomendei.Persistence;
using apiEncomendei.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace apiEncomendei.Controllers
{
    public class ShopkeeperController : ControllerBase
    {
        private readonly EncomendeiDbContext _dbContext;
        public ShopkeeperDao Dao { get; set; }
        public ShopkeeperController(EncomendeiDbContext dbContext)
        {
            _dbContext = dbContext;
            Dao = new ShopkeeperDao(_dbContext);
        }

        [HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            Shopkeeper shop = Dao.FindById(id);

            if (shop == null)
            {
                return NotFound();
            }

            ShopkeeperViewModel shopView = new ShopkeeperViewModel();
            shopView.NomeLoja = shop.NomeLoja;
            shopView.Descricao = shop.Descricao;

            return Ok(shopView);
        }

        [HttpGet("{id}")]
        public IActionResult ListByCategoryId(int id)
        {
            Shopkeeper shop = Dao.ListByCategoryId(id);

            if (shop == null)
            {
                return NotFound();
            }

            ShopkeeperViewModel shopView = new ShopkeeperViewModel();
            shopView.NomeLoja = shop.NomeLoja;
            shopView.Descricao = shop.Descricao;

            return Ok(shopView);
        }

        [HttpGet]
        public IActionResult List()
        {
            List<Shopkeeper> shopList = Dao.ListAll();

            if (shopList == null)
            {
                return NotFound();
            }

            List<ShopkeeperViewModel> viewList = new List<ShopkeeperViewModel>();

            foreach(Shopkeeper shoper in shopList)
            {
                ShopkeeperViewModel shopView = new ShopkeeperViewModel();
                shopView.NomeLoja = shoper.NomeLoja;
                shopView.Descricao = shoper.Descricao;

                viewList.Add(shopView);
            }

            return Ok(viewList);
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddShopkeeperInputModel model)
        {
            var shopkeeper = new Shopkeeper();
            shopkeeper.NomeLoja = model.NomeLoja;

            _dbContext.Shopkeepers.Add(shopkeeper);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
