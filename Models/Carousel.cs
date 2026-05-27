using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Carousel
    {
        [Key]
        public int CarouselId { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string LinkUrl { get; set; } = null!;

        [Required]
        public int Order { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
