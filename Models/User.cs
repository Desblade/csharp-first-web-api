using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? NickName { get; set; }
        [Required]
        public string? Email { get; set; }  
        public string? Comments { get; set; }  
        public DateTime CreateData { get; set; }
    }
}
