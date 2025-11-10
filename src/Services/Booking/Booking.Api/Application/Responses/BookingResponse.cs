namespace Booking.Api.Application.Responses;

public record BookingResponse(int Id, DateTime From, DateTime To, RoomResponse Room);