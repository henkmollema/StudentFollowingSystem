﻿CREATE TABLE [dbo].[Students] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
	[StudentNr]    INT			  NOT NULL,
    [FirstName]    NVARCHAR (50)  NOT NULL,
    [LastName]     NVARCHAR (50)  NOT NULL,
    [Email]        NVARCHAR (50)  NULL,
    [SchoolEmail]  NVARCHAR (50)  NULL,
    [Password]     NVARCHAR (68)  NOT NULL,
    [ClassId]      INT            NULL,
    [Telephone]    NVARCHAR (50)  NULL,
    [StreetName]   NVARCHAR (50)  NULL,
    [StreetNumber] INT            NULL,
    [ZipCode]      NVARCHAR (50)  NULL,
    [City]         NVARCHAR (50)  NULL,
    [BirthDate]    DATETIME       NULL,
    [EnrollDate]   DATETIME       NULL,
    [PreStudy]     NVARCHAR (50)  NULL,
    [Status]       INT            NULL,
	[Active]	   BIT			  DEFAULT(1) NOT NULL,
    [Details]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([Id] ASC)
);

