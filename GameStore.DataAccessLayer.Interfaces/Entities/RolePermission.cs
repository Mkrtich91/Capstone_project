
namespace GameStore.DataAccessLayer.Interfaces.Entities
{
    public class RolePermission
    {
        public string RoleId { get; set; }

        public Guid PermissionId { get; set; }

        public UserRole Role { get; set; }

        public Permission Permission { get; set; }
    }
}
