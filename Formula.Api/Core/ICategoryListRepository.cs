using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Formula.Api.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Formula.Api.Core
{
    public interface ICategoryListRepository:IGenericRepository<CategoryList>
    {
        
    }
}