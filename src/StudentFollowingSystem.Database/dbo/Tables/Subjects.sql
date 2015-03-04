CREATE TABLE [dbo].[Subjects]
(
	[Id] INT IDENTITY (1, 1) PRIMARY KEY, 
    [Name] NVARCHAR(50) NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL
)
