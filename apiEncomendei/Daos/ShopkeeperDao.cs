using apiEncomendei.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.Daos
{
    public class ShopkeeperDao : CrudDAO<Shopkeeper>
    {
        public ShopkeeperDao(Context context = null, string cnxStr = null) : base(context, cnxStr)
        { }

        public List<Shopkeeper> ListAll()
        {
            return DbSet
                .Where(shop => !shop.Removido)
                .ToList();
        }

        public List<Shopkeeper> ListByCategoryId(int id)
        {
            return DbSet
                .Where(shop => !shop.Removido && shop.CategoriaId == id)
                .ToList();
        }
        
        public Shopkeeper FindById(int id)
        {
            return DbSet
                .Where(shop => !shop.Removido && shop.Id == id)
                .FirstOrDefault();
        }
    }
}
