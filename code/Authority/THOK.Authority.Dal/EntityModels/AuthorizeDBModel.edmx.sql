
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 06/27/2012 17:09:00
-- Generated from EDMX file: D:\server\Authority\code\Authority\THOK.Authority.Dal\EntityModels\AuthorizeDBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Authorize];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SystemModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Module] DROP CONSTRAINT [FK_SystemModule];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Function] DROP CONSTRAINT [FK_ModuleFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Module] DROP CONSTRAINT [FK_ModuleModule];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleRoleSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleSystem] DROP CONSTRAINT [FK_RoleRoleSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleSystemRoleModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleModule] DROP CONSTRAINT [FK_RoleSystemRoleModule];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleModuleRoleFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleFunction] DROP CONSTRAINT [FK_RoleModuleRoleFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_RoleUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_UserUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_UserUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_UserUserSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSystem] DROP CONSTRAINT [FK_UserUserSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSystemUserModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserModule] DROP CONSTRAINT [FK_UserSystemUserModule];
GO
IF OBJECT_ID(N'[dbo].[FK_UserModuleUserFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFunction] DROP CONSTRAINT [FK_UserModuleUserFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_CityRoleSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleSystem] DROP CONSTRAINT [FK_CityRoleSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_CityUserSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSystem] DROP CONSTRAINT [FK_CityUserSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemRoleSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleSystem] DROP CONSTRAINT [FK_SystemRoleSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleRoleModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleModule] DROP CONSTRAINT [FK_ModuleRoleModule];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemUserSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSystem] DROP CONSTRAINT [FK_SystemUserSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleUserModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserModule] DROP CONSTRAINT [FK_ModuleUserModule];
GO
IF OBJECT_ID(N'[dbo].[FK_FunctionUserFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFunction] DROP CONSTRAINT [FK_FunctionUserFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_UserLoginLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LoginLog] DROP CONSTRAINT [FK_UserLoginLog];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemLoginLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LoginLog] DROP CONSTRAINT [FK_SystemLoginLog];
GO
IF OBJECT_ID(N'[dbo].[FK_FunctionRoleFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleFunction] DROP CONSTRAINT [FK_FunctionRoleFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_CityServer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Server] DROP CONSTRAINT [FK_CityServer];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Function]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Function];
GO
IF OBJECT_ID(N'[dbo].[LoginLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoginLog];
GO
IF OBJECT_ID(N'[dbo].[Module]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Module];
GO
IF OBJECT_ID(N'[dbo].[Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Role];
GO
IF OBJECT_ID(N'[dbo].[RoleFunction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleFunction];
GO
IF OBJECT_ID(N'[dbo].[RoleModule]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleModule];
GO
IF OBJECT_ID(N'[dbo].[RoleSystem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleSystem];
GO
IF OBJECT_ID(N'[dbo].[System]', 'U') IS NOT NULL
    DROP TABLE [dbo].[System];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[UserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRole];
GO
IF OBJECT_ID(N'[dbo].[UserFunction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserFunction];
GO
IF OBJECT_ID(N'[dbo].[UserModule]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserModule];
GO
IF OBJECT_ID(N'[dbo].[City]', 'U') IS NOT NULL
    DROP TABLE [dbo].[City];
GO
IF OBJECT_ID(N'[dbo].[UserSystem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSystem];
GO
IF OBJECT_ID(N'[dbo].[Server]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Server];
GO
IF OBJECT_ID(N'[dbo].[SystemEventLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemEventLogs];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Function'
CREATE TABLE [dbo].[Function] (
    [FunctionID] uniqueidentifier  NOT NULL,
    [FunctionName] varchar(50)  NOT NULL,
    [ControlName] varchar(50)  NOT NULL,
    [IndicateImage] varchar(100)  NULL,
    [Module_ModuleID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'LoginLog'
CREATE TABLE [dbo].[LoginLog] (
    [LogID] uniqueidentifier  NOT NULL,
    [LoginPC] varchar(50)  NOT NULL,
    [LoginTime] varchar(30)  NOT NULL,
    [LogoutTime] varchar(30)  NULL,
    [User_UserID] uniqueidentifier  NOT NULL,
    [System_SystemID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Module'
CREATE TABLE [dbo].[Module] (
    [ModuleID] uniqueidentifier  NOT NULL,
    [ModuleName] varchar(20)  NOT NULL,
    [ShowOrder] int  NOT NULL,
    [ModuleURL] varchar(100)  NOT NULL,
    [IndicateImage] varchar(100)  NULL,
    [DeskTopImage] varchar(100)  NULL,
    [System_SystemID] uniqueidentifier  NOT NULL,
    [ParentModule_ModuleID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Role'
CREATE TABLE [dbo].[Role] (
    [RoleID] uniqueidentifier  NOT NULL,
    [RoleName] varchar(50)  NOT NULL,
    [IsLock] bit  NOT NULL,
    [Memo] varchar(200)  NULL
);
GO

-- Creating table 'RoleFunction'
CREATE TABLE [dbo].[RoleFunction] (
    [RoleFunctionID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [RoleModule_RoleModuleID] uniqueidentifier  NOT NULL,
    [Function_FunctionID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'RoleModule'
CREATE TABLE [dbo].[RoleModule] (
    [RoleModuleID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [RoleSystem_RoleSystemID] uniqueidentifier  NOT NULL,
    [Module_ModuleID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'RoleSystem'
CREATE TABLE [dbo].[RoleSystem] (
    [RoleSystemID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Role_RoleID] uniqueidentifier  NOT NULL,
    [City_CityID] uniqueidentifier  NOT NULL,
    [System_SystemID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'System'
CREATE TABLE [dbo].[System] (
    [SystemID] uniqueidentifier  NOT NULL,
    [SystemName] varchar(100)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Status] bit  NOT NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [UserID] uniqueidentifier  NOT NULL,
    [UserName] varchar(50)  NOT NULL,
    [Pwd] varchar(50)  NOT NULL,
    [ChineseName] varchar(50)  NULL,
    [IsLock] bit  NOT NULL,
    [IsAdmin] bit  NOT NULL,
    [LoginPC] varchar(50)  NULL,
    [Memo] varchar(200)  NULL
);
GO

-- Creating table 'UserRole'
CREATE TABLE [dbo].[UserRole] (
    [UserRoleID] uniqueidentifier  NOT NULL,
    [Role_RoleID] uniqueidentifier  NOT NULL,
    [User_UserID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserFunction'
CREATE TABLE [dbo].[UserFunction] (
    [UserFunctionID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [UserModule_UserModuleID] uniqueidentifier  NOT NULL,
    [Function_FunctionID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserModule'
CREATE TABLE [dbo].[UserModule] (
    [UserModuleID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [UserSystem_UserSystemID] uniqueidentifier  NOT NULL,
    [Module_ModuleID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'City'
CREATE TABLE [dbo].[City] (
    [CityID] uniqueidentifier  NOT NULL,
    [CityName] varchar(50)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'UserSystem'
CREATE TABLE [dbo].[UserSystem] (
    [UserSystemID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [User_UserID] uniqueidentifier  NOT NULL,
    [City_CityID] uniqueidentifier  NOT NULL,
    [System_SystemID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Server'
CREATE TABLE [dbo].[Server] (
    [ServerID] uniqueidentifier  NOT NULL,
    [ServerName] varchar(50)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Url] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [City_CityID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'SystemEventLogs'
CREATE TABLE [dbo].[SystemEventLogs] (
    [EventLogID] uniqueidentifier  NOT NULL,
    [EventLogTime] varchar(30)  NOT NULL,
    [EventType] nvarchar(max)  NOT NULL,
    [EventName] nvarchar(max)  NOT NULL,
    [EventDescription] nvarchar(max)  NOT NULL,
    [FromPC] nvarchar(max)  NOT NULL,
    [OperateUser] nvarchar(max)  NOT NULL,
    [TargetSystem] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [FunctionID] in table 'Function'
ALTER TABLE [dbo].[Function]
ADD CONSTRAINT [PK_Function]
    PRIMARY KEY CLUSTERED ([FunctionID] ASC);
GO

-- Creating primary key on [LogID] in table 'LoginLog'
ALTER TABLE [dbo].[LoginLog]
ADD CONSTRAINT [PK_LoginLog]
    PRIMARY KEY CLUSTERED ([LogID] ASC);
GO

-- Creating primary key on [ModuleID] in table 'Module'
ALTER TABLE [dbo].[Module]
ADD CONSTRAINT [PK_Module]
    PRIMARY KEY CLUSTERED ([ModuleID] ASC);
GO

-- Creating primary key on [RoleID] in table 'Role'
ALTER TABLE [dbo].[Role]
ADD CONSTRAINT [PK_Role]
    PRIMARY KEY CLUSTERED ([RoleID] ASC);
GO

-- Creating primary key on [RoleFunctionID] in table 'RoleFunction'
ALTER TABLE [dbo].[RoleFunction]
ADD CONSTRAINT [PK_RoleFunction]
    PRIMARY KEY CLUSTERED ([RoleFunctionID] ASC);
GO

-- Creating primary key on [RoleModuleID] in table 'RoleModule'
ALTER TABLE [dbo].[RoleModule]
ADD CONSTRAINT [PK_RoleModule]
    PRIMARY KEY CLUSTERED ([RoleModuleID] ASC);
GO

-- Creating primary key on [RoleSystemID] in table 'RoleSystem'
ALTER TABLE [dbo].[RoleSystem]
ADD CONSTRAINT [PK_RoleSystem]
    PRIMARY KEY CLUSTERED ([RoleSystemID] ASC);
GO

-- Creating primary key on [SystemID] in table 'System'
ALTER TABLE [dbo].[System]
ADD CONSTRAINT [PK_System]
    PRIMARY KEY CLUSTERED ([SystemID] ASC);
GO

-- Creating primary key on [UserID] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [UserRoleID] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [PK_UserRole]
    PRIMARY KEY CLUSTERED ([UserRoleID] ASC);
GO

-- Creating primary key on [UserFunctionID] in table 'UserFunction'
ALTER TABLE [dbo].[UserFunction]
ADD CONSTRAINT [PK_UserFunction]
    PRIMARY KEY CLUSTERED ([UserFunctionID] ASC);
GO

-- Creating primary key on [UserModuleID] in table 'UserModule'
ALTER TABLE [dbo].[UserModule]
ADD CONSTRAINT [PK_UserModule]
    PRIMARY KEY CLUSTERED ([UserModuleID] ASC);
GO

-- Creating primary key on [CityID] in table 'City'
ALTER TABLE [dbo].[City]
ADD CONSTRAINT [PK_City]
    PRIMARY KEY CLUSTERED ([CityID] ASC);
GO

-- Creating primary key on [UserSystemID] in table 'UserSystem'
ALTER TABLE [dbo].[UserSystem]
ADD CONSTRAINT [PK_UserSystem]
    PRIMARY KEY CLUSTERED ([UserSystemID] ASC);
GO

-- Creating primary key on [ServerID] in table 'Server'
ALTER TABLE [dbo].[Server]
ADD CONSTRAINT [PK_Server]
    PRIMARY KEY CLUSTERED ([ServerID] ASC);
GO

-- Creating primary key on [EventLogID] in table 'SystemEventLogs'
ALTER TABLE [dbo].[SystemEventLogs]
ADD CONSTRAINT [PK_SystemEventLogs]
    PRIMARY KEY CLUSTERED ([EventLogID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [System_SystemID] in table 'Module'
ALTER TABLE [dbo].[Module]
ADD CONSTRAINT [FK_SystemModule]
    FOREIGN KEY ([System_SystemID])
    REFERENCES [dbo].[System]
        ([SystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemModule'
CREATE INDEX [IX_FK_SystemModule]
ON [dbo].[Module]
    ([System_SystemID]);
GO

-- Creating foreign key on [Module_ModuleID] in table 'Function'
ALTER TABLE [dbo].[Function]
ADD CONSTRAINT [FK_ModuleFunction]
    FOREIGN KEY ([Module_ModuleID])
    REFERENCES [dbo].[Module]
        ([ModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleFunction'
CREATE INDEX [IX_FK_ModuleFunction]
ON [dbo].[Function]
    ([Module_ModuleID]);
GO

-- Creating foreign key on [ParentModule_ModuleID] in table 'Module'
ALTER TABLE [dbo].[Module]
ADD CONSTRAINT [FK_ModuleModule]
    FOREIGN KEY ([ParentModule_ModuleID])
    REFERENCES [dbo].[Module]
        ([ModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleModule'
CREATE INDEX [IX_FK_ModuleModule]
ON [dbo].[Module]
    ([ParentModule_ModuleID]);
GO

-- Creating foreign key on [Role_RoleID] in table 'RoleSystem'
ALTER TABLE [dbo].[RoleSystem]
ADD CONSTRAINT [FK_RoleRoleSystem]
    FOREIGN KEY ([Role_RoleID])
    REFERENCES [dbo].[Role]
        ([RoleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleRoleSystem'
CREATE INDEX [IX_FK_RoleRoleSystem]
ON [dbo].[RoleSystem]
    ([Role_RoleID]);
GO

-- Creating foreign key on [RoleSystem_RoleSystemID] in table 'RoleModule'
ALTER TABLE [dbo].[RoleModule]
ADD CONSTRAINT [FK_RoleSystemRoleModule]
    FOREIGN KEY ([RoleSystem_RoleSystemID])
    REFERENCES [dbo].[RoleSystem]
        ([RoleSystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleSystemRoleModule'
CREATE INDEX [IX_FK_RoleSystemRoleModule]
ON [dbo].[RoleModule]
    ([RoleSystem_RoleSystemID]);
GO

-- Creating foreign key on [RoleModule_RoleModuleID] in table 'RoleFunction'
ALTER TABLE [dbo].[RoleFunction]
ADD CONSTRAINT [FK_RoleModuleRoleFunction]
    FOREIGN KEY ([RoleModule_RoleModuleID])
    REFERENCES [dbo].[RoleModule]
        ([RoleModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleModuleRoleFunction'
CREATE INDEX [IX_FK_RoleModuleRoleFunction]
ON [dbo].[RoleFunction]
    ([RoleModule_RoleModuleID]);
GO

-- Creating foreign key on [Role_RoleID] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [FK_RoleUserRole]
    FOREIGN KEY ([Role_RoleID])
    REFERENCES [dbo].[Role]
        ([RoleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUserRole'
CREATE INDEX [IX_FK_RoleUserRole]
ON [dbo].[UserRole]
    ([Role_RoleID]);
GO

-- Creating foreign key on [User_UserID] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [FK_UserUserRole]
    FOREIGN KEY ([User_UserID])
    REFERENCES [dbo].[User]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserRole'
CREATE INDEX [IX_FK_UserUserRole]
ON [dbo].[UserRole]
    ([User_UserID]);
GO

-- Creating foreign key on [User_UserID] in table 'UserSystem'
ALTER TABLE [dbo].[UserSystem]
ADD CONSTRAINT [FK_UserUserSystem]
    FOREIGN KEY ([User_UserID])
    REFERENCES [dbo].[User]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserSystem'
CREATE INDEX [IX_FK_UserUserSystem]
ON [dbo].[UserSystem]
    ([User_UserID]);
GO

-- Creating foreign key on [UserSystem_UserSystemID] in table 'UserModule'
ALTER TABLE [dbo].[UserModule]
ADD CONSTRAINT [FK_UserSystemUserModule]
    FOREIGN KEY ([UserSystem_UserSystemID])
    REFERENCES [dbo].[UserSystem]
        ([UserSystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSystemUserModule'
CREATE INDEX [IX_FK_UserSystemUserModule]
ON [dbo].[UserModule]
    ([UserSystem_UserSystemID]);
GO

-- Creating foreign key on [UserModule_UserModuleID] in table 'UserFunction'
ALTER TABLE [dbo].[UserFunction]
ADD CONSTRAINT [FK_UserModuleUserFunction]
    FOREIGN KEY ([UserModule_UserModuleID])
    REFERENCES [dbo].[UserModule]
        ([UserModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserModuleUserFunction'
CREATE INDEX [IX_FK_UserModuleUserFunction]
ON [dbo].[UserFunction]
    ([UserModule_UserModuleID]);
GO

-- Creating foreign key on [City_CityID] in table 'RoleSystem'
ALTER TABLE [dbo].[RoleSystem]
ADD CONSTRAINT [FK_CityRoleSystem]
    FOREIGN KEY ([City_CityID])
    REFERENCES [dbo].[City]
        ([CityID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CityRoleSystem'
CREATE INDEX [IX_FK_CityRoleSystem]
ON [dbo].[RoleSystem]
    ([City_CityID]);
GO

-- Creating foreign key on [City_CityID] in table 'UserSystem'
ALTER TABLE [dbo].[UserSystem]
ADD CONSTRAINT [FK_CityUserSystem]
    FOREIGN KEY ([City_CityID])
    REFERENCES [dbo].[City]
        ([CityID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CityUserSystem'
CREATE INDEX [IX_FK_CityUserSystem]
ON [dbo].[UserSystem]
    ([City_CityID]);
GO

-- Creating foreign key on [System_SystemID] in table 'RoleSystem'
ALTER TABLE [dbo].[RoleSystem]
ADD CONSTRAINT [FK_SystemRoleSystem]
    FOREIGN KEY ([System_SystemID])
    REFERENCES [dbo].[System]
        ([SystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemRoleSystem'
CREATE INDEX [IX_FK_SystemRoleSystem]
ON [dbo].[RoleSystem]
    ([System_SystemID]);
GO

-- Creating foreign key on [Module_ModuleID] in table 'RoleModule'
ALTER TABLE [dbo].[RoleModule]
ADD CONSTRAINT [FK_ModuleRoleModule]
    FOREIGN KEY ([Module_ModuleID])
    REFERENCES [dbo].[Module]
        ([ModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleRoleModule'
CREATE INDEX [IX_FK_ModuleRoleModule]
ON [dbo].[RoleModule]
    ([Module_ModuleID]);
GO

-- Creating foreign key on [System_SystemID] in table 'UserSystem'
ALTER TABLE [dbo].[UserSystem]
ADD CONSTRAINT [FK_SystemUserSystem]
    FOREIGN KEY ([System_SystemID])
    REFERENCES [dbo].[System]
        ([SystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemUserSystem'
CREATE INDEX [IX_FK_SystemUserSystem]
ON [dbo].[UserSystem]
    ([System_SystemID]);
GO

-- Creating foreign key on [Module_ModuleID] in table 'UserModule'
ALTER TABLE [dbo].[UserModule]
ADD CONSTRAINT [FK_ModuleUserModule]
    FOREIGN KEY ([Module_ModuleID])
    REFERENCES [dbo].[Module]
        ([ModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleUserModule'
CREATE INDEX [IX_FK_ModuleUserModule]
ON [dbo].[UserModule]
    ([Module_ModuleID]);
GO

-- Creating foreign key on [Function_FunctionID] in table 'UserFunction'
ALTER TABLE [dbo].[UserFunction]
ADD CONSTRAINT [FK_FunctionUserFunction]
    FOREIGN KEY ([Function_FunctionID])
    REFERENCES [dbo].[Function]
        ([FunctionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FunctionUserFunction'
CREATE INDEX [IX_FK_FunctionUserFunction]
ON [dbo].[UserFunction]
    ([Function_FunctionID]);
GO

-- Creating foreign key on [User_UserID] in table 'LoginLog'
ALTER TABLE [dbo].[LoginLog]
ADD CONSTRAINT [FK_UserLoginLog]
    FOREIGN KEY ([User_UserID])
    REFERENCES [dbo].[User]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserLoginLog'
CREATE INDEX [IX_FK_UserLoginLog]
ON [dbo].[LoginLog]
    ([User_UserID]);
GO

-- Creating foreign key on [System_SystemID] in table 'LoginLog'
ALTER TABLE [dbo].[LoginLog]
ADD CONSTRAINT [FK_SystemLoginLog]
    FOREIGN KEY ([System_SystemID])
    REFERENCES [dbo].[System]
        ([SystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemLoginLog'
CREATE INDEX [IX_FK_SystemLoginLog]
ON [dbo].[LoginLog]
    ([System_SystemID]);
GO

-- Creating foreign key on [Function_FunctionID] in table 'RoleFunction'
ALTER TABLE [dbo].[RoleFunction]
ADD CONSTRAINT [FK_FunctionRoleFunction]
    FOREIGN KEY ([Function_FunctionID])
    REFERENCES [dbo].[Function]
        ([FunctionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FunctionRoleFunction'
CREATE INDEX [IX_FK_FunctionRoleFunction]
ON [dbo].[RoleFunction]
    ([Function_FunctionID]);
GO

-- Creating foreign key on [City_CityID] in table 'Server'
ALTER TABLE [dbo].[Server]
ADD CONSTRAINT [FK_CityServer]
    FOREIGN KEY ([City_CityID])
    REFERENCES [dbo].[City]
        ([CityID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CityServer'
CREATE INDEX [IX_FK_CityServer]
ON [dbo].[Server]
    ([City_CityID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------