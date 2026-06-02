namespace Store.Domain.Entities.Users
{
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserInRole> UserInRoles { get; set; }
    }
}
