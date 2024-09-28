using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Labb_2_Avancerad_fullstackutveckling.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "Field cannot be empty.")]
        [StringLength(50, ErrorMessage = "The maximum number of characters allowed is 50.")]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Required(ErrorMessage = "Field cannot be empty.")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Field cannot be empty.")]
        public bool IsPopular { get; set; }
    }
}
