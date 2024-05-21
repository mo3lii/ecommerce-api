using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class AppUser:IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Hashed password
        public string Role { get; set; }
        public bool isDeleted { get ; set; }= false;
        public bool isActive { get; set; } = true;

    }
}
