using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formula.Api.Core
{
    public interface IUnitOfWork
    {
        IDriverRepository Drivers {get;}
        IListRepository Lists {get;}
        IProductRepository Products {get;}
        ICategoryRepository Categorys {get;}
        IListProductRepository ListProducts {get;}
        ICategoryProductRepository CategoryProducts {get;}
        ICategoryListRepository CategoryLists {get;}
        Task CompleteAsync();
    }
}