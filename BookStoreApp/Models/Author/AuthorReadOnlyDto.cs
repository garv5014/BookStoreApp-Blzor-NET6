using System.ComponentModel.DataAnnotations;
namespace BookStoreApp.Models.Author
{
    public class AuthorReadOnlyDto : BaseDto
    {
        public string Fristname { get; set; }
        public string Lastname { get; set; }
        public string Bio { get; set; }
    }
}
