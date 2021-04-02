using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.InputModels
{
    public class AddProductInputModel
    {
        public string Descricao { get; set; }
        public string Sku { get; set; }
        public decimal Preco { get; set; }
    }
}
