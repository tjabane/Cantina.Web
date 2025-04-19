using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.Dto
{
    public class ReviewView : Review
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
