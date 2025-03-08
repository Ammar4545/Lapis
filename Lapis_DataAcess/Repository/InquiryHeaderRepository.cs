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
    public class InquiryHeaderRepository : Repository<InquiryHeaders>, IInquiryHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public InquiryHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;        
        }

        public void Update(InquiryHeaders inquiryHeaders)
        {
            _db.Update(inquiryHeaders);
        }
    }
}
