namespace Store.Domain.Entities.Users
{
    public class UserInRole
    {
        public long Id { get; set; }
        public virtual  User user { get; set; }
        public long UserId { get; set; }
        public virtual Role role { get; set; }
        public long RoleId { get; set; }
    }
}
