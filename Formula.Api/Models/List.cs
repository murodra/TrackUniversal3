using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackUniversal2.Entities;

namespace Formula.Api.Models
{
    public class List:BaseEntity
    {
        public bool Approve { get; set; }
        public string Description { get; set; }
        public List<ListProduct> ListProducts { get; set; }
        public List<CategoryList> CategoryLists { get; set; }

    }
}