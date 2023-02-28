using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Formula.Api.Models;

namespace Formula.Api.Core
{
    public interface IDriverRepository:IGenericRepository<Driver>
    {
        Task<Driver?> GetByDriverNo(int driverNo);
    }
}