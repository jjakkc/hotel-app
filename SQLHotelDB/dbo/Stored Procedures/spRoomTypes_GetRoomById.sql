CREATE PROCEDURE [dbo].[spRoomTypes_GetRoomById]
	@Id int
AS
BEGIN
	SELECT [Id], [Title], [Description], [Price]
	FROM dbo.RoomTypes
	WHERE Id = @Id;
END
