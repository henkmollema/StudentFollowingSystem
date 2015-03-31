CREATE TABLE [dbo].[Counselings] (
    [Id]            INT  IDENTITY (1, 1) NOT NULL,
    [AppointmentId] INT  NOT NULL,
    [Comment]       TEXT NULL,
    [Private]       BIT  DEFAULT ((0)) NOT NULL,
    [Status]        INT  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

