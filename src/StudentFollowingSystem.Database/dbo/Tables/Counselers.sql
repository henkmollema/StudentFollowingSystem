CREATE TABLE [dbo].[Counselers] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (50) NOT NULL,
    [LastName]  NVARCHAR (50) NOT NULL,
    [Email]     NVARCHAR (50) NOT NULL,
    [Password]  NVARCHAR (68) NOT NULL,
    CONSTRAINT [PK_Counselers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

