using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TrackUniversal2.Entities;

namespace Formula.Api.Models
{
    public class Product:BaseEntity
    {
        public string Description { get; set; }
        public List<ListProduct> ListProducts { get; set; }
        public List<CategoryProduct> CategoryProducts { get; set; }
    }
}