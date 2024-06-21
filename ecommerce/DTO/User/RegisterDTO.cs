namespace ecommerce.DTO.User
{
    public class RegisterDTO
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password{ get; set; } // Hashed password
    }
}
