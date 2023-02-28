using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TrackUniversal2.Entities;

namespace Formula.Api.Models
{
    public class ListProduct:BaseEntity
    {
        public Guid ListId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("ListId")]
        public List List { get; set; }
    }
}