using apiEncomendei.Entities;
using apiEncomendei.InputModels;
using apiEncomendei.Persistence;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.Controllers
{
    [Route("api/consumidor")]
    public class CustomerController : ControllerBase
    {
        private readonly EncomendeiDbContext _dbContext;
        public CustomerController(EncomendeiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddCustomerInputModel model)
        {
            var customer = new Customer();
            customer.Nome = model.Nome;

            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public IActionResult RemoveById(int id)
        {
            return Ok();
        }
    }
}
