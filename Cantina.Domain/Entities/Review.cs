using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime TimeStamp { get; set; }
        public virtual MenuItem MenuItem { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
