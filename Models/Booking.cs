using System.ComponentModel.DataAnnotations;

namespace Labb_2_Avancerad_fullstackutveckling.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field cannot be empty.")]
        public int NoOfCustomers { get; set; }

        [Required(ErrorMessage = "Field cannot be empty.")]
        public DateTime BookedDateTime { get; set; }
    }
}
