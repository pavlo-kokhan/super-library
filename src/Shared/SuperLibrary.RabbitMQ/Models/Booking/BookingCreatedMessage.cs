namespace SuperLibrary.RabbitMQ.Models.Booking;

public record BookingCreatedMessage(DateTime From, DateTime To, int RoomId);