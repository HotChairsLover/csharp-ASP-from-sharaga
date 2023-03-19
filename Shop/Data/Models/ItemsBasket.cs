using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data.Models
{
    public class ItemsBasket: Items
    {
        public int Count { get; set; }
        public ItemsBasket(int Count, Items item): base(item)
        {
            this.Count = Count;
        }
    }
}
