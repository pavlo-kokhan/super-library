using FluentValidation;
using SuperLibrary.DomainAbstractions.Entities;
using SuperLibrary.Web.Extensions;
using SuperLibrary.Web.Results.Generic;

namespace Booking.Domain.AggregationRoots.Booking;

public class BookingEntity : PersistenceEntity
{
    private static readonly IValidator<BookingEntity> Validator = new BookingEntityValidator();
    
    private BookingEntity(DateTime from, DateTime to, int roomId)
    {
        From = from;
        To = to;
        RoomId = roomId;
    }

    private BookingEntity() { }
    
    public DateTime From { get; private set; }
    
    public DateTime To { get; private set; }
    
    public int RoomId { get; private set; }

    public static Result<BookingEntity> Create(DateTime from, DateTime to, int roomId)
        => Validator.ToResult(new BookingEntity(from, to, roomId));

    public Result<BookingEntity> Update(DateTime from, DateTime to, int roomId)
    {
        From = from;
        To = to;
        RoomId = roomId;
        
        return Validator.ToResult(this);
    }
}