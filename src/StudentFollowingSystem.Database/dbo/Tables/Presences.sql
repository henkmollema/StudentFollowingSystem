CREATE TABLE [dbo].[Presences]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [SubjectId] INT NOT NULL, 
    [StudentId] INT NOT NULL
)
