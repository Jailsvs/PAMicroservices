using SharedMicroservice.Models;

namespace UserMicroservice.Models
{
    public class User: AncestorTb
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Whats { get; set; }
        public string Password { get; set; } 
        public string UserType { get; set; }
        public int AvailableBids { get; set; }
    }
}
