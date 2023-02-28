using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackUniversal2.Entities;

namespace Formula.Api.Models
{
    public class Driver:BaseEntity
    {
        public int DriverNumber { get; set; }
        public string Team { get; set; }
    }
}