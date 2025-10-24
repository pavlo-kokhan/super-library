using FluentValidation;
using Library.Domain.AggregationRoots.ValueObjects;
using SuperLibrary.DomainAbstractions.Entities;
using SuperLibrary.Web.Extensions;
using SuperLibrary.Web.Results.Generic;

namespace Library.Domain.AggregationRoots.Library;

public class LibraryEntity : PersistenceEntity
{
    private static readonly IValidator<LibraryEntity> Validator = new LibraryEntityValidator(
        new AddressValueObjectValidator(), 
        new WeekScheduleValueObjectValidator());
    
    private LibraryEntity(string name, AddressValueObject address, WeekScheduleValueObject weekSchedule, int librarianUserId)
    {
        Name = name;
        Address = address;
        WeekSchedule = weekSchedule;
        LibrarianUserId = librarianUserId;
    }
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private LibraryEntity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    
    public string Name { get; private set; }
    
    public AddressValueObject Address { get; private set; }
    
    public WeekScheduleValueObject WeekSchedule { get; private set; }
    
    public int LibrarianUserId { get; private set; }

    public static Result<LibraryEntity> Create(string name, AddressValueObject address,
        WeekScheduleValueObject weekSchedule, int librarianUserId)
    {
        return Validator.ToResult(new LibraryEntity(name, address, weekSchedule, librarianUserId));
    }
    
    public Result<LibraryEntity> Update(string name, AddressValueObject address, WeekScheduleValueObject weekSchedule, int librarianUserId)
    {
        Name = name;
        Address = address;
        WeekSchedule = weekSchedule;
        LibrarianUserId = librarianUserId;

        return Validator.ToResult(this);
    }
}
