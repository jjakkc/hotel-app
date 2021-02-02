CREATE PROCEDURE [dbo].[spBookings_Insert]
	@roomId int,
	@guestId int,
	@startDate date,
	@endDate date,
	@totalCost money
AS
BEGIN
	INSERT into dbo.Bookings (RoomId, GuestId, StartDate, EndDate, TotalCost)
	VALUES (@roomId, @guestId, @startDate, @endDate, @totalCost);
END
