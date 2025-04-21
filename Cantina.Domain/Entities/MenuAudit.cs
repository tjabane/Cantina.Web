using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Domain.Entities
{
    public class MenuAudit
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int MenuItemId { get; set; }
        public int ActionId { get; set; }
        public DateTime Timestamp { get; set; }
        public Action Action { get; set; }
        public ICollection<ApplicationUser> AdminUser { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
    }
}
