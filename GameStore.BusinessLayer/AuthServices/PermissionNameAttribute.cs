namespace GameStore.BusinessLayer.AuthServices
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PermissionNameAttribute : Attribute
    {
        public string PermissionName { get; }

        public PermissionNameAttribute(string permissionName)
        {
            PermissionName = permissionName;
        }
    }
}
