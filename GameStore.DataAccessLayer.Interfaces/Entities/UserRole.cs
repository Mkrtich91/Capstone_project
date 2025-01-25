using Microsoft.AspNetCore.Identity;
namespace GameStore.DataAccessLayer.Interfaces.Entities
{
    public class UserRole : IdentityRole<string>
    {
        public UserRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public UserRole(string roleName) : base(roleName)
        {
            Id = Guid.NewGuid().ToString();
        }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
