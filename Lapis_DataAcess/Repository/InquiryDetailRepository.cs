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
    public class InquiryDetailRepository : Repository<InquiryDetails>, IInquiryDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public InquiryDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;        
        }


        public void Update(InquiryDetails inquiryDetails)
        {
            _db.Update(inquiryDetails);
        }
    }
}
