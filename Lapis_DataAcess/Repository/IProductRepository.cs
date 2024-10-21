using Lapis_DataAcess.Repository.IRepository;
using Lapis_Models;
using Lapis_Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lapis_DataAcess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;        
        }

        public IEnumerable<SelectListItem> GetAllDropDown(string obj)
        {
            if (obj== GlobalConst.ApplicationType)
            {
                _db.ApplicationTypes.
                Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }
            if (obj == GlobalConst.Category)
            {
                _db.Categories.
                Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product product)
        {
            _db.Update(product);
        }
    }
}
