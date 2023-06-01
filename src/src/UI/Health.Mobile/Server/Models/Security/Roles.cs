namespace Health.Mobile.Server.Models.Security;

public static class Roles
{
    public static List<Role> GetAllRoles()
    {
        var roles = new List<Role>
        {
            new Role{RoleName="administrator",RoleDescription= "Administrator"},
          new Role {RoleName="HealthOfficer", RoleDescription="Phlebotomist"},
          new Role {RoleName="LaboratoryTechnologist", RoleDescription="Laboratory Technologist"},
          new Role {RoleName="user", RoleDescription="User"},
          new Role  {RoleName="supervisor", RoleDescription="Supervisor"}
        };
        return  roles;
    }
}

public class Role
{
    public string? RoleName { get; set; }
    public string? RoleDescription { get; set; }
}