using System.ComponentModel.DataAnnotations;

namespace Labb_2_Avancerad_fullstackutveckling.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        [Required(ErrorMessage = "Field cannot be empty.")]
        public int NoOfCustomers { get; set; }

        [Required(ErrorMessage = "Field cannot be empty.")]
        public DateTime BookedDateTime { get; set; }
        public DateTime BookingEnds { get; set; }
        public string PhoneNo { get; set; }
        public string Name { get; set; }
        public int TableId { get; set; }
    }
}
