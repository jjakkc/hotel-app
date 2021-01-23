CREATE PROCEDURE [dbo].[spGuests_Insert]
	@firstName nvarchar(50),
	@lastName nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON

	IF not exists (SELECT 1 FROM dbo.Guests WHERE FirstName = @firstName and LastName = @lastName)
	BEGIN
		INSERT INTO dbo.Guests (FirstName, LastName) 
		VALUES (@firstName, @lastName);
	END

	SELECT [Id], [FirstName], [LastName]
	FROM dbo.Guests
	WHERE FirstName = @firstName and LastName = @lastName;
END
