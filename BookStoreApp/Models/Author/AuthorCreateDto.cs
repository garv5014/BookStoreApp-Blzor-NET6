using System.ComponentModel.DataAnnotations;
namespace BookStoreApp.Models.Author
{
    public class AuthorCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Fristname { get; set; }
        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }
        [StringLength(50)]
        public string Bio { get; set; }



    }
}
