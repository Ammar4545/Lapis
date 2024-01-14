namespace Lapis.Models.ViewModel
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            product = new Product();
        }
        public Product product { get; set; }
        public bool IsExistsInCart { get; set; }
    }
}
