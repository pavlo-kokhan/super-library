namespace SuperLibrary.RabbitMQ.Models.Booking;

public record BookingDeletedMessage(int Id, DateTime From, DateTime To, int RoomId, DateTime DeleteTime);