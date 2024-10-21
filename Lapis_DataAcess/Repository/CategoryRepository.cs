using Lapis_DataAcess.Repository.IRepository;
using Lapis_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lapis_DataAcess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;        
        }
        public void Update(Category category)
        {
            var categoryFromDb = _db.Categories.FirstOrDefault(x => x.Id == category.Id);

            if (categoryFromDb is not null)
            {
                categoryFromDb.Name = category.Name;
                categoryFromDb.DisplayOrder = category.DisplayOrder;
            }
        }
    }
}
