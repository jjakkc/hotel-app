CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableTypes]
	@startDate date,
	@endDate date
AS
BEGIN
	set nocount on;

	SELECT [rt].[Id], [rt].[Title], [rt].[Description], [rt].[Price]
	FROM dbo.Rooms r
	inner join dbo.RoomTypes rt ON rt.Id = r.RoomTypeId
	WHERE r.id not in (
		--Querying rooms that are booked based on date range
		SELECT b.RoomId
		FROM dbo.Bookings b
		WHERE (@startDate <= b.EndDate and @endDate >= b.StartDate)
	)
	GROUP BY rt.Title, rt.Description, rt.Price;
END
