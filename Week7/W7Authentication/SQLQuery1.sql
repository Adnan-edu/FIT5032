CREATE TABLE [dbo].[Students] (
    [Id]                   int IDENTITY (1,1) NOT NULL,
    [FirstName]                NVARCHAR (max) NOT NULL,
    [LastName]       NVARCHAR (max) NOT NULL,
    [UserId]         NVARCHAR (max) NOT NULL
    PRIMARY KEY (Id) 	
);
GO

-- Creating table 'Units'

CREATE TABLE [dbo].[Units] (
    [Id]                   int IDENTITY (1,1) NOT NULL,
    [Name]                NVARCHAR (max) NOT NULL,
    [Description]       NVARCHAR (max) NOT NULL,
    [StudentId]         int NOT NULL
    PRIMARY KEY (Id),
    FOREIGN KEY (StudentId) REFERENCES Students(Id) 	
);
GO