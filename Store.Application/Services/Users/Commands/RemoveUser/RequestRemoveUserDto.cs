namespace Store.Application.Services.Users.Commands.RemoveUser
{
    public class RequestRemoveUserDto
    {
        public long UserId { get; set; }
        public string Email { get; set; }
    }
}
