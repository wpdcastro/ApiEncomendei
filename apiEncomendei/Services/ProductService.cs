using apiEncomendei.Daos;
using apiEncomendei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.Services
{
    public class ProductService
    {
        public ProductDao Dao { get; set; }

        public ProductService(Context context = null, string cnxStr = null)
        {
            Dao = new ProductDao(context, cnxStr);
        }

        public List<Product> ListAll()
        {
            return Dao.ListAll();
        }
    }
}
