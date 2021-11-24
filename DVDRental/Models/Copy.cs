using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DVDRental.Models
{
    public class Copy
    {
        public int id { get; set; }
        [Required]
        public bool available { get; set; }

        //Copy info properties
        [Required]
        public int movieId { get; set; }

        //NotMapped proporties
        [NotMapped]
        public virtual Movie movie { get; set; }

        public Copy() {}
        public Copy(int id, bool available, int movieId)
        {
            this.id = id;
            this.available = available;
            this.movieId = movieId;
        }
    }
}
