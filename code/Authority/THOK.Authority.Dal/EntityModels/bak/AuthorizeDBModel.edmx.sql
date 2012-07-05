
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 06/19/2012 17:01:59
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

IF OBJECT_ID(N'[dbo].[FK_RoleRoleSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleSystems] DROP CONSTRAINT [FK_RoleRoleSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleSystemRoleModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleModules] DROP CONSTRAINT [FK_RoleSystemRoleModule];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleModuleRoleFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleFunctions] DROP CONSTRAINT [FK_RoleModuleRoleFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_UserLoginLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LoginLogs] DROP CONSTRAINT [FK_UserLoginLog];
GO
IF OBJECT_ID(N'[dbo].[FK_UserUserSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSystems] DROP CONSTRAINT [FK_UserUserSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSystemUserModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserModules] DROP CONSTRAINT [FK_UserSystemUserModule];
GO
IF OBJECT_ID(N'[dbo].[FK_UserModuleUserFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFunctions] DROP CONSTRAINT [FK_UserModuleUserFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_UserUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_RoleUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_CityServer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Servers] DROP CONSTRAINT [FK_CityServer];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Modules] DROP CONSTRAINT [FK_SystemModule];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Functions] DROP CONSTRAINT [FK_ModuleFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_CityUserSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSystems] DROP CONSTRAINT [FK_CityUserSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_CityRoleSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleSystems] DROP CONSTRAINT [FK_CityRoleSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemRoleSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleSystems] DROP CONSTRAINT [FK_SystemRoleSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleRoleModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleModules] DROP CONSTRAINT [FK_ModuleRoleModule];
GO
IF OBJECT_ID(N'[dbo].[FK_FunctionRoleFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleFunctions] DROP CONSTRAINT [FK_FunctionRoleFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemUserSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSystems] DROP CONSTRAINT [FK_SystemUserSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleUserModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserModules] DROP CONSTRAINT [FK_ModuleUserModule];
GO
IF OBJECT_ID(N'[dbo].[FK_FunctionUserFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFunctions] DROP CONSTRAINT [FK_FunctionUserFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Modules] DROP CONSTRAINT [FK_ModuleModule];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Cities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cities];
GO
IF OBJECT_ID(N'[dbo].[Functions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Functions];
GO
IF OBJECT_ID(N'[dbo].[LoginLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LoginLogs];
GO
IF OBJECT_ID(N'[dbo].[Modules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Modules];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[RoleFunctions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleFunctions];
GO
IF OBJECT_ID(N'[dbo].[RoleModules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleModules];
GO
IF OBJECT_ID(N'[dbo].[RoleSystems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleSystems];
GO
IF OBJECT_ID(N'[dbo].[Servers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Servers];
GO
IF OBJECT_ID(N'[dbo].[Systems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Systems];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[UserFunctions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserFunctions];
GO
IF OBJECT_ID(N'[dbo].[UserModules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserModules];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[UserSystems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSystems];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'City'
CREATE TABLE [dbo].[City] (
    [CityID] uniqueidentifier  NOT NULL,
    [CityName] varchar(50)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Functions'
CREATE TABLE [dbo].[Functions] (
    [FunctionID] uniqueidentifier  NOT NULL,
    [FunctionName] varchar(50)  NOT NULL,
    [ControlName] varchar(50)  NOT NULL,
    [Module_ModuleID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'LoginLogs'
CREATE TABLE [dbo].[LoginLogs] (
    [LogID] uniqueidentifier  NOT NULL,
    [LoginPC] varchar(50)  NOT NULL,
    [LoginTime] varchar(30)  NOT NULL,
    [LogoutTime] varchar(30)  NULL,
    [User_UserID] uniqueidentifier  NOT NULL
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

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleID] uniqueidentifier  NOT NULL,
    [RoleName] varchar(50)  NOT NULL,
    [IsLock] bit  NOT NULL,
    [Memo] varchar(200)  NULL
);
GO

-- Creating table 'RoleFunctions'
CREATE TABLE [dbo].[RoleFunctions] (
    [RoleFunctionID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [RoleModule_RoleModuleID] uniqueidentifier  NOT NULL,
    [Function_FunctionID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'RoleModules'
CREATE TABLE [dbo].[RoleModules] (
    [RoleModuleID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [RoleSystem_RoleSystemID] uniqueidentifier  NOT NULL,
    [Module_ModuleID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'RoleSystems'
CREATE TABLE [dbo].[RoleSystems] (
    [RoleSystemID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Role_RoleID] uniqueidentifier  NOT NULL,
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

-- Creating table 'Systems'
CREATE TABLE [dbo].[Systems] (
    [SystemID] uniqueidentifier  NOT NULL,
    [SystemName] varchar(100)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Status] bit  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
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

-- Creating table 'UserFunctions'
CREATE TABLE [dbo].[UserFunctions] (
    [UserFunctionID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [UserModule_UserModuleID] uniqueidentifier  NOT NULL,
    [Function_FunctionID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserModules'
CREATE TABLE [dbo].[UserModules] (
    [UserModuleID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [UserSystem_UserSystemID] uniqueidentifier  NOT NULL,
    [Module_ModuleID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [UserRoleID] uniqueidentifier  NOT NULL,
    [User_UserID] uniqueidentifier  NOT NULL,
    [Role_RoleID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserSystems'
CREATE TABLE [dbo].[UserSystems] (
    [UserSystemID] uniqueidentifier  NOT NULL,
    [IsActive] bit  NOT NULL,
    [User_UserID] uniqueidentifier  NOT NULL,
    [City_CityID] uniqueidentifier  NOT NULL,
    [System_SystemID] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CityID] in table 'City'
ALTER TABLE [dbo].[City]
ADD CONSTRAINT [PK_City]
    PRIMARY KEY CLUSTERED ([CityID] ASC);
GO

-- Creating primary key on [FunctionID] in table 'Functions'
ALTER TABLE [dbo].[Functions]
ADD CONSTRAINT [PK_Functions]
    PRIMARY KEY CLUSTERED ([FunctionID] ASC);
GO

-- Creating primary key on [LogID] in table 'LoginLogs'
ALTER TABLE [dbo].[LoginLogs]
ADD CONSTRAINT [PK_LoginLogs]
    PRIMARY KEY CLUSTERED ([LogID] ASC);
GO

-- Creating primary key on [ModuleID] in table 'Module'
ALTER TABLE [dbo].[Module]
ADD CONSTRAINT [PK_Module]
    PRIMARY KEY CLUSTERED ([ModuleID] ASC);
GO

-- Creating primary key on [RoleID] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleID] ASC);
GO

-- Creating primary key on [RoleFunctionID] in table 'RoleFunctions'
ALTER TABLE [dbo].[RoleFunctions]
ADD CONSTRAINT [PK_RoleFunctions]
    PRIMARY KEY CLUSTERED ([RoleFunctionID] ASC);
GO

-- Creating primary key on [RoleModuleID] in table 'RoleModules'
ALTER TABLE [dbo].[RoleModules]
ADD CONSTRAINT [PK_RoleModules]
    PRIMARY KEY CLUSTERED ([RoleModuleID] ASC);
GO

-- Creating primary key on [RoleSystemID] in table 'RoleSystems'
ALTER TABLE [dbo].[RoleSystems]
ADD CONSTRAINT [PK_RoleSystems]
    PRIMARY KEY CLUSTERED ([RoleSystemID] ASC);
GO

-- Creating primary key on [ServerID] in table 'Server'
ALTER TABLE [dbo].[Server]
ADD CONSTRAINT [PK_Server]
    PRIMARY KEY CLUSTERED ([ServerID] ASC);
GO

-- Creating primary key on [SystemID] in table 'Systems'
ALTER TABLE [dbo].[Systems]
ADD CONSTRAINT [PK_Systems]
    PRIMARY KEY CLUSTERED ([SystemID] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [UserFunctionID] in table 'UserFunctions'
ALTER TABLE [dbo].[UserFunctions]
ADD CONSTRAINT [PK_UserFunctions]
    PRIMARY KEY CLUSTERED ([UserFunctionID] ASC);
GO

-- Creating primary key on [UserModuleID] in table 'UserModules'
ALTER TABLE [dbo].[UserModules]
ADD CONSTRAINT [PK_UserModules]
    PRIMARY KEY CLUSTERED ([UserModuleID] ASC);
GO

-- Creating primary key on [UserRoleID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([UserRoleID] ASC);
GO

-- Creating primary key on [UserSystemID] in table 'UserSystems'
ALTER TABLE [dbo].[UserSystems]
ADD CONSTRAINT [PK_UserSystems]
    PRIMARY KEY CLUSTERED ([UserSystemID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Role_RoleID] in table 'RoleSystems'
ALTER TABLE [dbo].[RoleSystems]
ADD CONSTRAINT [FK_RoleRoleSystem]
    FOREIGN KEY ([Role_RoleID])
    REFERENCES [dbo].[Roles]
        ([RoleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleRoleSystem'
CREATE INDEX [IX_FK_RoleRoleSystem]
ON [dbo].[RoleSystems]
    ([Role_RoleID]);
GO

-- Creating foreign key on [RoleSystem_RoleSystemID] in table 'RoleModules'
ALTER TABLE [dbo].[RoleModules]
ADD CONSTRAINT [FK_RoleSystemRoleModule]
    FOREIGN KEY ([RoleSystem_RoleSystemID])
    REFERENCES [dbo].[RoleSystems]
        ([RoleSystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleSystemRoleModule'
CREATE INDEX [IX_FK_RoleSystemRoleModule]
ON [dbo].[RoleModules]
    ([RoleSystem_RoleSystemID]);
GO

-- Creating foreign key on [RoleModule_RoleModuleID] in table 'RoleFunctions'
ALTER TABLE [dbo].[RoleFunctions]
ADD CONSTRAINT [FK_RoleModuleRoleFunction]
    FOREIGN KEY ([RoleModule_RoleModuleID])
    REFERENCES [dbo].[RoleModules]
        ([RoleModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleModuleRoleFunction'
CREATE INDEX [IX_FK_RoleModuleRoleFunction]
ON [dbo].[RoleFunctions]
    ([RoleModule_RoleModuleID]);
GO

-- Creating foreign key on [User_UserID] in table 'LoginLogs'
ALTER TABLE [dbo].[LoginLogs]
ADD CONSTRAINT [FK_UserLoginLog]
    FOREIGN KEY ([User_UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserLoginLog'
CREATE INDEX [IX_FK_UserLoginLog]
ON [dbo].[LoginLogs]
    ([User_UserID]);
GO

-- Creating foreign key on [User_UserID] in table 'UserSystems'
ALTER TABLE [dbo].[UserSystems]
ADD CONSTRAINT [FK_UserUserSystem]
    FOREIGN KEY ([User_UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserSystem'
CREATE INDEX [IX_FK_UserUserSystem]
ON [dbo].[UserSystems]
    ([User_UserID]);
GO

-- Creating foreign key on [UserSystem_UserSystemID] in table 'UserModules'
ALTER TABLE [dbo].[UserModules]
ADD CONSTRAINT [FK_UserSystemUserModule]
    FOREIGN KEY ([UserSystem_UserSystemID])
    REFERENCES [dbo].[UserSystems]
        ([UserSystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSystemUserModule'
CREATE INDEX [IX_FK_UserSystemUserModule]
ON [dbo].[UserModules]
    ([UserSystem_UserSystemID]);
GO

-- Creating foreign key on [UserModule_UserModuleID] in table 'UserFunctions'
ALTER TABLE [dbo].[UserFunctions]
ADD CONSTRAINT [FK_UserModuleUserFunction]
    FOREIGN KEY ([UserModule_UserModuleID])
    REFERENCES [dbo].[UserModules]
        ([UserModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserModuleUserFunction'
CREATE INDEX [IX_FK_UserModuleUserFunction]
ON [dbo].[UserFunctions]
    ([UserModule_UserModuleID]);
GO

-- Creating foreign key on [User_UserID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_UserUserRole]
    FOREIGN KEY ([User_UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserRole'
CREATE INDEX [IX_FK_UserUserRole]
ON [dbo].[UserRoles]
    ([User_UserID]);
GO

-- Creating foreign key on [Role_RoleID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_RoleUserRole]
    FOREIGN KEY ([Role_RoleID])
    REFERENCES [dbo].[Roles]
        ([RoleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUserRole'
CREATE INDEX [IX_FK_RoleUserRole]
ON [dbo].[UserRoles]
    ([Role_RoleID]);
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

-- Creating foreign key on [System_SystemID] in table 'Module'
ALTER TABLE [dbo].[Module]
ADD CONSTRAINT [FK_SystemModule]
    FOREIGN KEY ([System_SystemID])
    REFERENCES [dbo].[Systems]
        ([SystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemModule'
CREATE INDEX [IX_FK_SystemModule]
ON [dbo].[Module]
    ([System_SystemID]);
GO

-- Creating foreign key on [Module_ModuleID] in table 'Functions'
ALTER TABLE [dbo].[Functions]
ADD CONSTRAINT [FK_ModuleFunction]
    FOREIGN KEY ([Module_ModuleID])
    REFERENCES [dbo].[Module]
        ([ModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleFunction'
CREATE INDEX [IX_FK_ModuleFunction]
ON [dbo].[Functions]
    ([Module_ModuleID]);
GO

-- Creating foreign key on [City_CityID] in table 'UserSystems'
ALTER TABLE [dbo].[UserSystems]
ADD CONSTRAINT [FK_CityUserSystem]
    FOREIGN KEY ([City_CityID])
    REFERENCES [dbo].[City]
        ([CityID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CityUserSystem'
CREATE INDEX [IX_FK_CityUserSystem]
ON [dbo].[UserSystems]
    ([City_CityID]);
GO

-- Creating foreign key on [City_CityID] in table 'RoleSystems'
ALTER TABLE [dbo].[RoleSystems]
ADD CONSTRAINT [FK_CityRoleSystem]
    FOREIGN KEY ([City_CityID])
    REFERENCES [dbo].[City]
        ([CityID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CityRoleSystem'
CREATE INDEX [IX_FK_CityRoleSystem]
ON [dbo].[RoleSystems]
    ([City_CityID]);
GO

-- Creating foreign key on [System_SystemID] in table 'RoleSystems'
ALTER TABLE [dbo].[RoleSystems]
ADD CONSTRAINT [FK_SystemRoleSystem]
    FOREIGN KEY ([System_SystemID])
    REFERENCES [dbo].[Systems]
        ([SystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemRoleSystem'
CREATE INDEX [IX_FK_SystemRoleSystem]
ON [dbo].[RoleSystems]
    ([System_SystemID]);
GO

-- Creating foreign key on [Module_ModuleID] in table 'RoleModules'
ALTER TABLE [dbo].[RoleModules]
ADD CONSTRAINT [FK_ModuleRoleModule]
    FOREIGN KEY ([Module_ModuleID])
    REFERENCES [dbo].[Module]
        ([ModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleRoleModule'
CREATE INDEX [IX_FK_ModuleRoleModule]
ON [dbo].[RoleModules]
    ([Module_ModuleID]);
GO

-- Creating foreign key on [Function_FunctionID] in table 'RoleFunctions'
ALTER TABLE [dbo].[RoleFunctions]
ADD CONSTRAINT [FK_FunctionRoleFunction]
    FOREIGN KEY ([Function_FunctionID])
    REFERENCES [dbo].[Functions]
        ([FunctionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FunctionRoleFunction'
CREATE INDEX [IX_FK_FunctionRoleFunction]
ON [dbo].[RoleFunctions]
    ([Function_FunctionID]);
GO

-- Creating foreign key on [System_SystemID] in table 'UserSystems'
ALTER TABLE [dbo].[UserSystems]
ADD CONSTRAINT [FK_SystemUserSystem]
    FOREIGN KEY ([System_SystemID])
    REFERENCES [dbo].[Systems]
        ([SystemID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemUserSystem'
CREATE INDEX [IX_FK_SystemUserSystem]
ON [dbo].[UserSystems]
    ([System_SystemID]);
GO

-- Creating foreign key on [Module_ModuleID] in table 'UserModules'
ALTER TABLE [dbo].[UserModules]
ADD CONSTRAINT [FK_ModuleUserModule]
    FOREIGN KEY ([Module_ModuleID])
    REFERENCES [dbo].[Module]
        ([ModuleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleUserModule'
CREATE INDEX [IX_FK_ModuleUserModule]
ON [dbo].[UserModules]
    ([Module_ModuleID]);
GO

-- Creating foreign key on [Function_FunctionID] in table 'UserFunctions'
ALTER TABLE [dbo].[UserFunctions]
ADD CONSTRAINT [FK_FunctionUserFunction]
    FOREIGN KEY ([Function_FunctionID])
    REFERENCES [dbo].[Functions]
        ([FunctionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FunctionUserFunction'
CREATE INDEX [IX_FK_FunctionUserFunction]
ON [dbo].[UserFunctions]
    ([Function_FunctionID]);
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------