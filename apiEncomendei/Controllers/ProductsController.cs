using apiEncomendei.Entities;
using apiEncomendei.InputModels;
using apiEncomendei.Services;
using apiEncomendei.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.Controllers
{
    [Route("api/produtos")]
    public class ProductsController : ControllerBase
    {
        public ProductService ProductService { get; set; }
        public ProductsController()
        {
            ProductService = new ProductService();
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Product> productList = ProductService.ListAll();

            if (lista == null)
            {
                return NotFound();
            }
            
            List<ProductViewModel> viewList = new List<ProductViewModel>;
            
            foreach (Product product in productList)
            {
                ProductViewModel productViewModel = new ProductViewModel
                {
                    Descricao = product.Descricao,
                };

                viewList.Add(productViewModel);
            }

            return Ok(viewList);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddProductInputModel model)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveById(int id)
        {
            return Ok();
        }
    }
}
