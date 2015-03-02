CREATE TABLE [dbo].[Classes]
(
	[Id] INT IDENTITY PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Section] NVARCHAR(50) NULL, 
    [CounselerId] INT NOT NULL
)
