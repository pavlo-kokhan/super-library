using Booking.Api.Application.Services.Abstract;
using Booking.Domain.AggregationRoots.Booking;
using Booking.Infrastructure.Persistence;
using MediatR;
using SuperLibrary.RabbitMQ.Models.Booking;
using SuperLibrary.Web.Results;
using SuperLibrary.Web.ValidationErrors;

namespace Booking.Api.Application.Commands.Booking;

public record CreateBookingCommandSynchronousApproach(DateTime From, DateTime To, int RoomId) : IRequest<Result>
{
    public class Handler : IRequestHandler<CreateBookingCommandSynchronousApproach, Result>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILibraryService _libraryService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<CreateBookingCommandSynchronousApproach> _logger;

        public Handler(
            ApplicationDbContext dbContext, 
            ILibraryService libraryService, 
            INotificationService notificationService, 
            ILogger<CreateBookingCommandSynchronousApproach> logger)
        {
            _dbContext = dbContext;
            _libraryService = libraryService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<Result> Handle(CreateBookingCommandSynchronousApproach request, CancellationToken cancellationToken)
        {
            var room = await _libraryService.GetRoomByIdAsync(request.RoomId, cancellationToken);

            if (room is null)
                ValidationError.CreateInvalidArgument("ROOM_NOT_FOUND", ResultStatus.NotFound);

            var createBookingResult = BookingEntity.Create(request.From, request.To, request.RoomId);

            if (createBookingResult.IsFailure)
                return createBookingResult;

            await _dbContext.AddAsync(createBookingResult.Data, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var bookingEntity = createBookingResult.Data;
            var result = await _notificationService.NotifyBookingCreatedAsync(
                new BookingCreatedMessage(
                    bookingEntity.Id, 
                    bookingEntity.From, 
                    bookingEntity.To, 
                    bookingEntity.RoomId),
                cancellationToken);
            
            _logger.LogInformation("Sending notification message (synchronous approach). Result: {Result}", result.IsSuccess);

            return Result.Success();
        }
    }
}