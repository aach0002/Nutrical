CREATE TABLE [dbo].[UserDetails] (
    [UserId] NVARCHAR (128) NOT NULL,
    [Height] FLOAT (53)     NOT NULL,
    [Age]    INT            NOT NULL,
    [Weight] FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_dbo.UserDetails] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_dbo.UserDetails_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Posts] (
    [UserId]   NVARCHAR (128) NOT NULL,
    [Post]     NCHAR (10)     NOT NULL,
    [Approved] NCHAR (10)     NOT NULL,
    [Date]     DATETIMEOFFSET     NOT NULL,
    [id]       INT            IDENTITY (1, 1) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_dbo.Posts_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Comments] (
    [Id]       INT        IDENTITY (1, 1) NOT NULL,
    [Comment]  NCHAR (10) NOT NULL,
    [PostId]   INT        NOT NULL,
    [DateTime] DATETIMEOFFSET NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Comments_dbo.AspNetUsers_UserId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts] ([id]) ON DELETE CASCADE
);


CREATE TABLE [dbo].[CaloriesEaten] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [CaloriesEaten] FLOAT (53)     NOT NULL,
    [Type]          NCHAR (1)      NOT NULL,
    [Date]          DATE           NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CaloriesEaten_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[CaloriesBurnt] (
    [Id]            INT            NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL,
    [CaloriesBurnt] FLOAT (53)     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.CaloriesBurnt_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

