using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.Entities
{
    public class Shopkeeper : Entity
    {
        public string NomeLoja { get; set; }
        public string Descricao { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
    }
}
