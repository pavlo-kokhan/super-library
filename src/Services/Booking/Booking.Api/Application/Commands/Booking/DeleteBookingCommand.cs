using Booking.Api.Application.Services.Abstract;
using Booking.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuperLibrary.RabbitMQ.Abstractions;
using SuperLibrary.RabbitMQ.Models.Booking;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.ValidationErrors;

namespace Booking.Api.Application.Commands.Booking;

public record DeleteBookingCommand(int Id) : IRequest<Result>;

public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, Result>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IRabbitMqPublisher<BookingDeletedMessage> _publisher;

    public DeleteBookingCommandHandler(ApplicationDbContext dbContext, IRabbitMqPublisher<BookingDeletedMessage> publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task<Result> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _dbContext.NonDeletedBookings
            .Where(b => b.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (booking is null)
            return ValidationError.CreateInvalidArgument("BOOKING_NOT_FOUND", ResultStatus.NotFound);
        
        booking.Delete();

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publisher.PublishAsync(
            new BookingDeletedMessage(
                booking.Id,
                booking.From,
                booking.To,
                booking.RoomId,
                DateTime.UtcNow),
            cancellationToken);

        return Result.Success();
    }
}