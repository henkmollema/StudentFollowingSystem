CREATE TABLE [dbo].[Appointments]
(
	[Id] INT IDENTITY (1, 1) PRIMARY KEY, 
    [StudentId] NCHAR(10) NULL, 
    [CounselerId] NCHAR(10) NULL, 
    [Datetime] DATETIME NOT NULL, 
    [Location] NVARCHAR(50) NOT NULL, 
    [Accepted] BIT NOT NULL DEFAULT 0
)
