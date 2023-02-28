using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TrackUniversal2.Entities;

namespace Formula.Api.Models
{
    public class CategoryProduct:BaseEntity
    {
        public Guid CategoryId { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("ProductId")]
        public List Product { get; set; }
    }
}