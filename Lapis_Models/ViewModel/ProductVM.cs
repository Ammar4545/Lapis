using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Lapis_Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> SelectedCategories { get; set; }
        public IEnumerable<SelectListItem> SelectedApplicationType { get; set; }
    }
}
