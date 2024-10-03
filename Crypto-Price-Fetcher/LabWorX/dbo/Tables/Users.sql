CREATE TABLE [dbo].[Users] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [UserName]  NVARCHAR (50)    NOT NULL,
    [FirstName] NVARCHAR (50)    NOT NULL,
    [LastName]  NVARCHAR (50)    NOT NULL,
    [Email]     NVARCHAR (50)    NOT NULL,
    [Password]  NVARCHAR (50)    NOT NULL,
    [IsActive]  BIT              DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

