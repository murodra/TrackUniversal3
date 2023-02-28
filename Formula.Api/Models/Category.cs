using Formula.Api.Models;

namespace TrackUniversal2.Entities
{
    public class Category:BaseEntity
    {
        public List<CategoryProduct> CategoryProducts { get; set; }
        public List<CategoryList> CategoryLists { get; set; }
    }
}