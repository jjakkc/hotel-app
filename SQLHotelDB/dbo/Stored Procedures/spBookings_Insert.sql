CREATE PROCEDURE [dbo].[spBookings_Insert]
	@roomId int,
	@guestId int,
	@startDate date,
	@endDate date,
	@checkedIn bit,
	@totalCost money
AS
BEGIN
	INSERT into dbo.Bookings (RoomId, GuestId, StartDate, EndDate, CheckedIn, TotalCost)
	VALUES (@roomId, @guestId, @startDate, @endDate, @checkedIn, @totalCost);
END
