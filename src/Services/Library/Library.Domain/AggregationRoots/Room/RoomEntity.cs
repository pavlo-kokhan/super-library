using FluentValidation;
using SuperLibrary.DomainAbstractions.Entities;
using SuperLibrary.Web.Extensions;
using SuperLibrary.Web.Results.Generic;

namespace Library.Domain.AggregationRoots.Room;

public class RoomEntity : PersistenceEntity
{
    private static readonly IValidator<RoomEntity> Validator = new RoomEntityValidator();
    
    private RoomEntity() { }

    private RoomEntity(int libraryId, int number, bool isAvailable)
    {
        LibraryId = libraryId;
        Number = number;
        IsAvailable = isAvailable;
    }
    
    public int Number { get; private set; }
    
    public bool IsAvailable { get; private set; }
    
    public int LibraryId { get; private set; }
    
    public static Result<RoomEntity> Create(int libraryId, int number, bool isAvailable)
    {
        return Validator.ToResult(new RoomEntity(libraryId, number, isAvailable));
    }
    
    public Result<RoomEntity> Update(int libraryId, int number, bool isAvailable)
    {
        LibraryId = libraryId;
        Number = number;
        IsAvailable = isAvailable;

        return Validator.ToResult(this);
    }
}