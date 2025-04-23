using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.SyncService.Data
{
    public class Review
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
