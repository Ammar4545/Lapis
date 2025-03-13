using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lapis_Models.ViewModel
{
    public class InquiruVM
    {
        public InquiryHeaders inquiryHeader { get; set; }
        public IEnumerable<InquiryDetails> inquiryDetails { get; set; }
    }
}
