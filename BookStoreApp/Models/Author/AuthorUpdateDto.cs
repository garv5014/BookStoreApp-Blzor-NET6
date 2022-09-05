using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Models.Author
{
    public class AuthorUpdateDto : BaseDto
    {
        public string Fristname { get; set; }
        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }
        [StringLength(50)]
        public string Bio { get; set; }
    }
}
