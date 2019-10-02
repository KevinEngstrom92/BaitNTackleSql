using System;
using System.Collections.Generic;
using System.Text;

namespace BaitNTackleSQL.Domain
{
    class Product
    {
        public string Name { get; private set; }
        public int Price { get; private set; }
        public int id { get; set; }

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}
