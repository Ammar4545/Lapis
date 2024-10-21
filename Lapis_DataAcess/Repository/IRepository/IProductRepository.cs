using Lapis_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lapis_DataAcess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product category);
        IEnumerable<SelectListItem> GetAllDropDown(string obj);
    }
}
