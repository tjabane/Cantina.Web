using Cantina.Core.Data.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.Data.Entities
{
    public class AdminMenuAudit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public Actions Action { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
