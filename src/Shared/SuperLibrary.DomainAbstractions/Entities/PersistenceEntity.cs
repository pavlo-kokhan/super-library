namespace SuperLibrary.DomainAbstractions.Entities;

public class PersistenceEntity : Entity, IPersistenceEntity
{
    public bool IsDeleted { get; private set; }

    public DateTime? DeletedTime { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
        DeletedTime = DateTime.UtcNow;
    }
}
