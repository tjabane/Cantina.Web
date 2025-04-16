using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.Dto
{
    public class MenuItem
    {
        // Dishes & drinks must have a name, description, price, and image.
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public bool IsDrink { get; set; }
    }
}
