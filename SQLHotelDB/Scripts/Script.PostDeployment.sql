/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
if not exists (select 1 from dbo.RoomTypes)
begin
    INSERT into dbo.RoomTypes (Title, Description, Price) 
    VALUES 
    ('King Size Bed', 'A room with a king-size bed and toilet.', 75),
    ('Two Queen Size Beds', 'A room with a two queen beds and tree.', 120),
    ('Executive Suite', 'A luxurious room with multiple rooms.', 300);
end

if not exists (select 1 from dbo.Rooms)
begin
    declare @roomId1 int;
    declare @roomId2 int;
    declare @roomId3 int;

    SELECT @roomId1 = Id from dbo.RoomTypes WHERE Title = 'King Size Bed';
    SELECT @roomId2 = Id from dbo.RoomTypes WHERE Title = 'Two Queen Size Beds';
    SELECT @roomId3 = Id from dbo.RoomTypes WHERE Title = 'Executive Suite';

    INSERT into dbo.Rooms (RoomNumber, RoomTypeId)
    VALUES
    ('101', @roomId1),
    ('105', @roomId1),
    ('201', @roomId2),
    ('203', @roomId2),
    ('301', @roomId3);
end