namespace SalesDashboard.Domain.Managers;

public class Manager
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = null!;
    public string Team { get; private set; } = null!;
    public bool IsActive { get; private set; }
    
    private Manager() { }

    private Manager(string fullName, string team, bool isActive)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Team = team;
        IsActive = isActive;
    }

    public static Manager Create(string fullName, string team, bool isActive = true)
    {
        // не конфликтует с fullname = null!;?
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Manager full name is required");

        return new Manager(fullName, team, isActive);
    }
}