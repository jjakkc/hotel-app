CREATE PROCEDURE [dbo].[spRooms_GetAvailableRooms]
	@startDate date,
	@endDate date,
	@roomTypeId int
AS
BEGIN
	SET NOCOUNT ON

	SELECT [r].[Id], [r].[RoomNumber], [r].[RoomTypeId]
	FROM dbo.Rooms r
	inner join dbo.RoomTypes rt on rt.id = r.RoomTypeId
	WHERE r.RoomTypeId = @roomTypeId 
	and
	r.Id not in (
		SELECT [Id], [RoomId], [GuestId], [StartDate], [EndDate], [CheckedIn], [TotalCost]
		FROM dbo.Bookings b
		WHERE (b.StartDate > @startDate and b.EndDate < @endDate) -- S--|----|--E start and end encompass range
		or (b.StartDate <= @startDate and b.EndDate > @startDate)  -- |--S--|--E start intersects range
		or (b.StartDate <= @endDate and b.EndDate > @endDate) -- S--|--E--| end intersects range
	)
END
