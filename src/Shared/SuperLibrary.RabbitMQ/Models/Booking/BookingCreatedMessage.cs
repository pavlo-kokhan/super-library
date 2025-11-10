namespace SuperLibrary.RabbitMQ.Models.Booking;

public record BookingCreatedMessage(int Id, DateTime From, DateTime To, int RoomId);