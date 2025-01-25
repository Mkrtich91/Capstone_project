using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DataAccessLayer.Interfaces.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}
