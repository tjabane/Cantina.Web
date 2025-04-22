using Cantina.Domain.Contants;

namespace Cantina.Web.Dto
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public MenuItemType Type { get; set; }
    }
}
