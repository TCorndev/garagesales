namespace garagesales.Models.dto
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Confirm {  get; set; } = null!;
    }
}
