CREATE PROCEDURE [dbo].[spBookings_Search]
	@lastName nvarchar(50),
	@startDate date
AS
BEGIN
	SELECT [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], [b].[EndDate], [b].[CheckedIn], [b].[TotalCost], 
		[g].[FirstName], [g].[LastName], 
		[r].[RoomNumber], [r].[RoomTypeId], 
		[rt].[Title], [rt].[Description], [rt].[Price]
	FROM dbo.Bookings b
	inner join dbo.Guests g ON g.Id = b.GuestId
	inner join dbo.Rooms r ON r.Id = b.RoomId
	inner join dbo.RoomTypes rt On rt.Id = r.RoomTypeId
	WHERE g.LastName = @lastName and b.StartDate >= @startDate
END
