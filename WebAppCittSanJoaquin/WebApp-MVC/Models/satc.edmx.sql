
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/30/2018 11:24:16
-- Generated from EDMX file: C:\Users\nobodynuf\Source\Repos\ProyectoAsistenciaCitt2018\WebAppCittSanJoaquin\WebApp-MVC\Models\satc.edmx
-- --------------------------------------------------
--CREATE DATABASE [SATC];
--GO



SET QUOTED_IDENTIFIER OFF;
GO
USE [satc];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_det_asist_asistencia_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[det_asist] DROP CONSTRAINT [FK_det_asist_asistencia_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_det_asist_horario_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[det_asist] DROP CONSTRAINT [FK_det_asist_horario_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_det_asist_usuario_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[det_asist] DROP CONSTRAINT [FK_det_asist_usuario_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_horario_taller_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[horario] DROP CONSTRAINT [FK_horario_taller_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_horario_usuario_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[horario] DROP CONSTRAINT [FK_horario_usuario_fk];
GO
IF OBJECT_ID(N'[dbo].[FK_taller_usuario_fk]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[taller] DROP CONSTRAINT [FK_taller_usuario_fk];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[asistencia]', 'U') IS NOT NULL
    DROP TABLE [dbo].[asistencia];
GO
IF OBJECT_ID(N'[dbo].[det_asist]', 'U') IS NOT NULL
    DROP TABLE [dbo].[det_asist];
GO
IF OBJECT_ID(N'[dbo].[horario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[horario];
GO
IF OBJECT_ID(N'[dbo].[log_acciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[log_acciones];
GO
IF OBJECT_ID(N'[dbo].[taller]', 'U') IS NOT NULL
    DROP TABLE [dbo].[taller];
GO
IF OBJECT_ID(N'[dbo].[usuario]', 'U') IS NOT NULL
    DROP TABLE [dbo].[usuario];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'asistencia'
CREATE TABLE [dbo].[asistencia] (
    [id_asistencia] int IDENTITY(1,1) NOT NULL,
    [fecha] datetime  NOT NULL
);
GO

-- Creating table 'det_asist'
CREATE TABLE [dbo].[det_asist] (
    [asistencia_id_asistencia] int  NOT NULL,
    [usuario_id_usuario] int  NOT NULL,
    [horario_id_horario] int  NOT NULL
);
GO

-- Creating table 'horario'
CREATE TABLE [dbo].[horario] (
    [id_horario] int IDENTITY(1,1) NOT NULL,
    [hora_inicio] datetime  NOT NULL,
    [hora_termino] datetime  NOT NULL,
    [dia_semana] varchar(20)  NOT NULL,
    [cupo] int  NOT NULL,
    [taller_id_taller] int  NOT NULL,
    [usuario_id_usuario] int  NOT NULL
);
GO

-- Creating table 'log_acciones'
CREATE TABLE [dbo].[log_acciones] (
    [id_acciones] int IDENTITY(1,1) NOT NULL,
    [fecha] datetime  NOT NULL,
    [accion] varchar(100)  NOT NULL,
    [nombre_ejecutor] varchar(30)  NOT NULL,
    [id_ejecutor] int  NOT NULL
);
GO

-- Creating table 'taller'
CREATE TABLE [dbo].[taller] (
    [id_taller] int IDENTITY(1,1) NOT NULL,
    [nombre] varchar(30)  NOT NULL,
    [descripcion] varchar(100)  NULL,
    [id_encargado] int  NOT NULL
);
GO

-- Creating table 'usuario'
CREATE TABLE [dbo].[usuario] (
    [id_usuario] int IDENTITY(1,1) NOT NULL,
    [nombre] varchar(50)  NOT NULL,
    [apellidos] varchar(60)  NOT NULL,
    [correo] varchar(50)  NOT NULL,
    [password] char(32)  NOT NULL,
    [habilitado] tinyint  NOT NULL,
    [tipo_usuario] char(1)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id_asistencia] in table 'asistencia'
ALTER TABLE [dbo].[asistencia]
ADD CONSTRAINT [PK_asistencia]
    PRIMARY KEY CLUSTERED ([id_asistencia] ASC);
GO

-- Creating primary key on [asistencia_id_asistencia], [usuario_id_usuario], [horario_id_horario] in table 'det_asist'
ALTER TABLE [dbo].[det_asist]
ADD CONSTRAINT [PK_det_asist]
    PRIMARY KEY CLUSTERED ([asistencia_id_asistencia], [usuario_id_usuario], [horario_id_horario] ASC);
GO

-- Creating primary key on [id_horario] in table 'horario'
ALTER TABLE [dbo].[horario]
ADD CONSTRAINT [PK_horario]
    PRIMARY KEY CLUSTERED ([id_horario] ASC);
GO

-- Creating primary key on [id_acciones] in table 'log_acciones'
ALTER TABLE [dbo].[log_acciones]
ADD CONSTRAINT [PK_log_acciones]
    PRIMARY KEY CLUSTERED ([id_acciones] ASC);
GO

-- Creating primary key on [id_taller] in table 'taller'
ALTER TABLE [dbo].[taller]
ADD CONSTRAINT [PK_taller]
    PRIMARY KEY CLUSTERED ([id_taller] ASC);
GO

-- Creating primary key on [id_usuario] in table 'usuario'
ALTER TABLE [dbo].[usuario]
ADD CONSTRAINT [PK_usuario]
    PRIMARY KEY CLUSTERED ([id_usuario] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [asistencia_id_asistencia] in table 'det_asist'
ALTER TABLE [dbo].[det_asist]
ADD CONSTRAINT [FK_det_asist_asistencia_fk]
    FOREIGN KEY ([asistencia_id_asistencia])
    REFERENCES [dbo].[asistencia]
        ([id_asistencia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [horario_id_horario] in table 'det_asist'
ALTER TABLE [dbo].[det_asist]
ADD CONSTRAINT [FK_det_asist_horario_fk]
    FOREIGN KEY ([horario_id_horario])
    REFERENCES [dbo].[horario]
        ([id_horario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_det_asist_horario_fk'
CREATE INDEX [IX_FK_det_asist_horario_fk]
ON [dbo].[det_asist]
    ([horario_id_horario]);
GO

-- Creating foreign key on [usuario_id_usuario] in table 'det_asist'
ALTER TABLE [dbo].[det_asist]
ADD CONSTRAINT [FK_det_asist_usuario_fk]
    FOREIGN KEY ([usuario_id_usuario])
    REFERENCES [dbo].[usuario]
        ([id_usuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_det_asist_usuario_fk'
CREATE INDEX [IX_FK_det_asist_usuario_fk]
ON [dbo].[det_asist]
    ([usuario_id_usuario]);
GO

-- Creating foreign key on [taller_id_taller] in table 'horario'
ALTER TABLE [dbo].[horario]
ADD CONSTRAINT [FK_horario_taller_fk]
    FOREIGN KEY ([taller_id_taller])
    REFERENCES [dbo].[taller]
        ([id_taller])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_horario_taller_fk'
CREATE INDEX [IX_FK_horario_taller_fk]
ON [dbo].[horario]
    ([taller_id_taller]);
GO

-- Creating foreign key on [usuario_id_usuario] in table 'horario'
ALTER TABLE [dbo].[horario]
ADD CONSTRAINT [FK_horario_usuario_fk]
    FOREIGN KEY ([usuario_id_usuario])
    REFERENCES [dbo].[usuario]
        ([id_usuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_horario_usuario_fk'
CREATE INDEX [IX_FK_horario_usuario_fk]
ON [dbo].[horario]
    ([usuario_id_usuario]);
GO

-- Creating foreign key on [id_encargado] in table 'taller'
ALTER TABLE [dbo].[taller]
ADD CONSTRAINT [FK_taller_usuario_fk]
    FOREIGN KEY ([id_encargado])
    REFERENCES [dbo].[usuario]
        ([id_usuario])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_taller_usuario_fk'
CREATE INDEX [IX_FK_taller_usuario_fk]
ON [dbo].[taller]
    ([id_encargado]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------