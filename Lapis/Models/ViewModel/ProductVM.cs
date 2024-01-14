using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Lapis.Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> SelectCategories { get; set; }
        public IEnumerable<SelectListItem> SelectedApplicationType { get; set; }
    }
}
