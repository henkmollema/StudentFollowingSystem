CREATE TABLE [dbo].[Appointments]
(
	[Id] INT IDENTITY PRIMARY KEY, 
    [StudentId] NCHAR(10) NOT NULL, 
    [CounselerId] NCHAR(10) NOT NULL, 
    [Datetime] DATETIME NOT NULL, 
    [Location] NVARCHAR(50) NOT NULL
)
