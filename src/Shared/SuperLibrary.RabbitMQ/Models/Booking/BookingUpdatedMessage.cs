namespace SuperLibrary.RabbitMQ.Models.Booking;

public record BookingUpdatedMessage(int Id, DateTime From, DateTime To, int RoomId, DateTime UpdateTime);