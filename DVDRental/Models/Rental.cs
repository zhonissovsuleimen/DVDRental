using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DVDRental.Models
{
    public class Rental
    {
        public int id { get; set; }

        //Rental info properties
        [Required]
        public int userId { get; set; }
        [Required]
        public int copyId { get; set; }

        //Time proporties
        public DateTime rentDate { get; set; }
        public DateTime? returnDate { get; set; }

        //NotMapped proporties
        [NotMapped]
        [ForeignKey("copyId")]
        public virtual Copy copy { get; set; }
        [NotMapped]
        [ForeignKey("userId")]
        public virtual IdentityUser user { get; set; }

        public Rental() { }
        public Rental(int userId, int copyId)
        {
            this.userId = userId;
            this.copyId = copyId;
            rentDate = DateTime.Now;
        }
    }
}
