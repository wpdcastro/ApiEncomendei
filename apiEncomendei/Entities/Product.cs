﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiEncomendei.Entities
{
    public class Product : Entity
    {
        public string NomeProduto { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
