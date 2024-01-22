using System.Collections.Generic;

namespace Lapis_Models.ViewModel
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            applicationUser = new ApplicationUser();
            ProductList = new List<Product>();
        }
        public ApplicationUser applicationUser { get; set; }
        public List<Product> ProductList { get; set; }
    }
}
