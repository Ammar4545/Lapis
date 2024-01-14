using System.ComponentModel.DataAnnotations;

namespace Lapis.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name ="Display Order")]
        [Range(1,int.MaxValue,ErrorMessage ="The Display order must be greater than (0) ")]
        public int DisplayOrder { get; set; }
    }
}
