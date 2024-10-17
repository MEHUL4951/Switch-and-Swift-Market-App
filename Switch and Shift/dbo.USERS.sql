CREATE TABLE [dbo].[USERS] (
    [UserID]     INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]  VARCHAR (50)  NOT NULL,
    [LastName]   VARCHAR (50)  NOT NULL,
    [District] VARCHAR (50)  NOT NULL,
    [Location]   VARCHAR (200) NOT NULL,
    [Email]      VARCHAR (100) NOT NULL,
    [Phone]      VARCHAR (20)  NOT NULL,
    [Password]   VARCHAR (30)  NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC),
    UNIQUE NONCLUSTERED ([LastName] ASC)
);

