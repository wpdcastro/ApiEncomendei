using apiEncomendei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.Daos
{
    public class ProductDao : CrudDAO<Product>
    {
        public ProductDao(Context context = null, string cnxStr = null) : base(context, cnxStr)
        { }

        public List<Product> ListAll()
        {
            return DbSet
                .Where(prod => !prod.Removido)
                .ToList();
        }
    }
}
