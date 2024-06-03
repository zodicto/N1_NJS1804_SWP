IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Account] (
    [id_Account] varchar(50) NOT NULL,
    [Username] varchar(50) NULL,
    [Password] varchar(50) NULL,
    [FisrtName] varchar(50) NULL,
    [LastName] varchar(50) NULL,
    [Gmail] varchar(50) NULL,
    [Birthdate] datetime2 NULL,
    [Gender] nvarchar(max) NULL,
    [Role] varchar(50) NULL,
    CONSTRAINT [PK__Account__ADA956212E0C4E76] PRIMARY KEY ([id_Account])
);
GO

CREATE TABLE [Field] (
    [id_Field] varchar(50) NOT NULL,
    [FieldName] varchar(50) NULL,
    CONSTRAINT [PK__Field__0CC408078A47A724] PRIMARY KEY ([id_Field])
);
GO

CREATE TABLE [Type of service] (
    [id_TypeOfService] varchar(50) NOT NULL,
    [NameService] varchar(50) NULL,
    CONSTRAINT [PK__Type of __0DF9946FAF2D937A] PRIMARY KEY ([id_TypeOfService])
);
GO

CREATE TABLE [Feedback] (
    [id_feedback] varchar(50) NOT NULL,
    [description] varchar(50) NULL,
    [id_Account] varchar(50) NULL,
    [id_Service] varchar(50) NULL,
    CONSTRAINT [PK__Feedback__36BC863086749877] PRIMARY KEY ([id_feedback]),
    CONSTRAINT [FK__Feedback__id_Acc__3DB3258D] FOREIGN KEY ([id_Account]) REFERENCES [Account] ([id_Account])
);
GO

CREATE TABLE [Messages] (
    [id_Chat] varchar(50) NOT NULL,
    [id_Account] varchar(50) NULL,
    CONSTRAINT [PK__Messages__316B3F44B536C3B4] PRIMARY KEY ([id_Chat]),
    CONSTRAINT [FK__Messages__id_Acc__54968AE5] FOREIGN KEY ([id_Account]) REFERENCES [Account] ([id_Account])
);
GO

CREATE TABLE [RefreshToken] (
    [Id] uniqueidentifier NOT NULL,
    [Token] varchar(50) NULL,
    [JwtId] varchar(50) NULL,
    [IsUsed] bit NULL,
    [IsRevoked] bit NULL,
    [IssuedAt] datetime NULL,
    [ExpiredAt] datetime NULL,
    [ID_Account] varchar(50) NOT NULL,
    CONSTRAINT [PK__RefreshT__3214EC0750897F01] PRIMARY KEY ([Id]),
    CONSTRAINT [FK__RefreshTo__ID_Ac__5A4F643B] FOREIGN KEY ([ID_Account]) REFERENCES [Account] ([id_Account])
);
GO

CREATE TABLE [Tutor] (
    [id_Tutor] varchar(50) NOT NULL,
    [SpecializedSkills] varchar(50) NULL,
    [Experience] int NULL,
    [id_Account] varchar(50) NULL,
    [Status] bit NULL,
    CONSTRAINT [PK__Tutor__93DA661D916EF89E] PRIMARY KEY ([id_Tutor]),
    CONSTRAINT [FK__Tutor__id_Accoun__2F650636] FOREIGN KEY ([id_Account]) REFERENCES [Account] ([id_Account])
);
GO

CREATE TABLE [Post] (
    [id_Post] varchar(50) NOT NULL,
    [Price] varchar(50) NULL,
    [Titile] varchar(50) NULL,
    [Description] varchar(200) NULL,
    [Status] bit NULL,
    [id_Account] varchar(50) NULL,
    [id_TypeOfService] varchar(50) NULL,
    CONSTRAINT [PK__Post__2BA425F7AC64A41D] PRIMARY KEY ([id_Post]),
    CONSTRAINT [FK__Post__id_Account__4924D839] FOREIGN KEY ([id_Account]) REFERENCES [Account] ([id_Account]),
    CONSTRAINT [FK__Post__id_TypeOfS__4A18FC72] FOREIGN KEY ([id_TypeOfService]) REFERENCES [Type of service] ([id_TypeOfService])
);
GO

CREATE TABLE [Service] (
    [id_Service] varchar(50) NOT NULL,
    [Price] real NULL,
    [Status] varchar(50) NULL,
    [id_TypeOfService] varchar(50) NULL,
    [Title] varchar(50) NULL,
    [Description] varchar(50) NULL,
    [id_feedback] varchar(50) NULL,
    CONSTRAINT [PK__Service__F6F54EA7038DCAB7] PRIMARY KEY ([id_Service]),
    CONSTRAINT [FK__Service__id_Type__4183B671] FOREIGN KEY ([id_TypeOfService]) REFERENCES [Type of service] ([id_TypeOfService]),
    CONSTRAINT [FK__Service__id_feed__4277DAAA] FOREIGN KEY ([id_feedback]) REFERENCES [Feedback] ([id_feedback])
);
GO

CREATE TABLE [EducationalQualifications] (
    [id_EducationalEualifications] varchar(50) NOT NULL,
    [id_Tutor] varchar(50) NULL,
    [CertificateName] varchar(50) NULL,
    [Organization] varchar(50) NULL,
    [img] varchar(50) NULL,
    [Type] varchar(50) NULL,
    CONSTRAINT [PK__Educatio__C1B293B0C50E68E8] PRIMARY KEY ([id_EducationalEualifications]),
    CONSTRAINT [FK__Education__id_Tu__324172E1] FOREIGN KEY ([id_Tutor]) REFERENCES [Tutor] ([id_Tutor])
);
GO

CREATE TABLE [Tutor_Field] (
    [id_Tutor_Fileld] varchar(50) NOT NULL,
    [id_Tutor] varchar(50) NULL,
    [id_Field] varchar(50) NULL,
    CONSTRAINT [PK__Tutor_Fi__CBB4B60C26DD204F] PRIMARY KEY ([id_Tutor_Fileld]),
    CONSTRAINT [FK__Tutor_Fie__id_Fi__37FA4C37] FOREIGN KEY ([id_Field]) REFERENCES [Field] ([id_Field]),
    CONSTRAINT [FK__Tutor_Fie__id_Tu__370627FE] FOREIGN KEY ([id_Tutor]) REFERENCES [Tutor] ([id_Tutor])
);
GO

CREATE TABLE [ResquestTutor] (
    [id_RequestTutor] varchar(50) NOT NULL,
    [Status] varchar(50) NULL,
    [id_Tutor] varchar(50) NULL,
    [id_Post] varchar(50) NULL,
    CONSTRAINT [PK__Resquest__61285A5E8EB2641D] PRIMARY KEY ([id_RequestTutor]),
    CONSTRAINT [FK__ResquestT__id_Po__51BA1E3A] FOREIGN KEY ([id_Post]) REFERENCES [Post] ([id_Post]),
    CONSTRAINT [FK__ResquestT__id_Tu__50C5FA01] FOREIGN KEY ([id_Tutor]) REFERENCES [Tutor] ([id_Tutor])
);
GO

CREATE TABLE [Schedule] (
    [id_Schedule] varchar(50) NOT NULL,
    [Date] date NULL,
    [TimeStart] time NULL,
    [TimeEnd] time NULL,
    [id_Tutor] varchar(50) NULL,
    [id_Service] varchar(50) NULL,
    CONSTRAINT [PK__Schedule__B70A1A1150945D1A] PRIMARY KEY ([id_Schedule]),
    CONSTRAINT [FK__Schedule__id_Ser__46486B8E] FOREIGN KEY ([id_Service]) REFERENCES [Service] ([id_Service]),
    CONSTRAINT [FK__Schedule__id_Tut__45544755] FOREIGN KEY ([id_Tutor]) REFERENCES [Tutor] ([id_Tutor])
);
GO

CREATE TABLE [Rent] (
    [id_Rent] varchar(50) NOT NULL,
    [Status] varchar(10) NULL,
    [id_Account] varchar(50) NULL,
    [id_Schedule] varchar(50) NULL,
    CONSTRAINT [PK__Rent__478EEDDFA4D65269] PRIMARY KEY ([id_Rent]),
    CONSTRAINT [FK__Rent__id_Account__4CF5691D] FOREIGN KEY ([id_Account]) REFERENCES [Account] ([id_Account]),
    CONSTRAINT [FK__Rent__id_Schedul__4DE98D56] FOREIGN KEY ([id_Schedule]) REFERENCES [Schedule] ([id_Schedule])
);
GO

CREATE INDEX [IX_EducationalQualifications_id_Tutor] ON [EducationalQualifications] ([id_Tutor]);
GO

CREATE INDEX [IX_Feedback_id_Account] ON [Feedback] ([id_Account]);
GO

CREATE UNIQUE INDEX [UQ__Feedback__F6F54EA6E556053E] ON [Feedback] ([id_Service]) WHERE [id_Service] IS NOT NULL;
GO

CREATE INDEX [IX_Messages_id_Account] ON [Messages] ([id_Account]);
GO

CREATE INDEX [IX_Post_id_Account] ON [Post] ([id_Account]);
GO

CREATE INDEX [IX_Post_id_TypeOfService] ON [Post] ([id_TypeOfService]);
GO

CREATE INDEX [IX_RefreshToken_ID_Account] ON [RefreshToken] ([ID_Account]);
GO

CREATE INDEX [IX_Rent_id_Account] ON [Rent] ([id_Account]);
GO

CREATE INDEX [IX_Rent_id_Schedule] ON [Rent] ([id_Schedule]);
GO

CREATE INDEX [IX_ResquestTutor_id_Post] ON [ResquestTutor] ([id_Post]);
GO

CREATE INDEX [IX_ResquestTutor_id_Tutor] ON [ResquestTutor] ([id_Tutor]);
GO

CREATE INDEX [IX_Schedule_id_Service] ON [Schedule] ([id_Service]);
GO

CREATE INDEX [IX_Schedule_id_Tutor] ON [Schedule] ([id_Tutor]);
GO

CREATE INDEX [IX_Service_id_TypeOfService] ON [Service] ([id_TypeOfService]);
GO

CREATE UNIQUE INDEX [UQ__Service__36BC8631BD63F430] ON [Service] ([id_feedback]) WHERE [id_feedback] IS NOT NULL;
GO

CREATE INDEX [IX_Tutor_id_Account] ON [Tutor] ([id_Account]);
GO

CREATE INDEX [IX_Tutor_Field_id_Field] ON [Tutor_Field] ([id_Field]);
GO

CREATE INDEX [IX_Tutor_Field_id_Tutor] ON [Tutor_Field] ([id_Tutor]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240603035323_First', N'8.0.5');
GO

COMMIT;
GO

