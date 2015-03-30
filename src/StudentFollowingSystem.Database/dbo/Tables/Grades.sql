CREATE TABLE [dbo].[Grades]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [ExamUnitId] INT NOT NULL, 
    [StudentId] INT NOT NULL, 
    [Result] DECIMAL(18, 2) NOT NULL, 
    [Notes] NVARCHAR(500) NULL
)
