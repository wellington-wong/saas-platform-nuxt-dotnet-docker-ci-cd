namespace Sazzle.Domain.Entities;

public class Role

{
    public Guid Id { get; private set; }
    public Guid? OrganizationId { get; private set; }
    public string Name { get; private set; }
    private readonly List<Permission> _permissions = new();
    public IReadOnlyCollection<Permission>  Permissions => _permissions.AsReadOnly();

    private Role() { }


    public Role(string name, Guid? organizationId = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        OrganizationId = organizationId;
    }

    public void GrantPermission(Permission permission)
    {
        
        if (!_permissions.Any(p => p.Id == permission.Id))
            _permissions.Add(permission);
    }
}