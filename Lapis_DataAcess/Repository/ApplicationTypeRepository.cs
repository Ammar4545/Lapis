using Lapis_DataAcess.Repository.IRepository;
using Lapis_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lapis_DataAcess.Repository
{
    public class ApplicationTypeRepository :Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ApplicationType applicationType)
        {
            var categoryFromDb = _db.ApplicationTypes.FirstOrDefault(x => x.Id == applicationType.Id);

            if (categoryFromDb is not null)
            {
                categoryFromDb.Name = applicationType.Name;
            }
        }
    }
}
