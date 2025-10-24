namespace SuperLibrary.DomainAbstractions.Entities;

public interface IPersistenceEntity
{
    bool IsDeleted { get; }

    DateTime? DeletedTime { get; }
}
