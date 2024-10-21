using Lapis_DataAcess.Repository.IRepository;
using Lapis_Models;
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
        public void Update(Product product)
        {
            _db.Update(product);
        }
    }
}
