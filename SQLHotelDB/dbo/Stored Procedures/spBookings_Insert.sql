CREATE PROCEDURE [dbo].[spBookings_Insert]
	@roomId int,
	@guestId int,
	@startDaate date,
	@endDate date,
	@totalCost money
AS
BEGIN
	INSERT into dbo.Bookings (RoomId, GuestId, StartDate, EndDate, TotalCost)
	VALUES (@roomId, @guestId, @startDaate, @endDate, @totalCost);
END
