USE [master]
GO
CREATE LOGIN [SolPwr] WITH PASSWORD=N'SolPwr', DEFAULT_DATABASE=[SolarPower], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE [SolarPower]
GO
/****** Object:  User [SolPwr]    Script Date: 2025-02-10 14:31:58 ******/
CREATE USER [SolPwr] FOR LOGIN [SolPwr] WITH DEFAULT_SCHEMA=[spa]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [SolPwr]
GO
ALTER ROLE [db_datareader] ADD MEMBER [SolPwr]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [SolPwr]
GO
/****** Object:  Schema [spa]    Script Date: 2025-02-10 14:31:59 ******/
CREATE SCHEMA [spa]
GO
/****** Object:  Schema [spu]    Script Date: 2025-02-10 14:31:59 ******/
CREATE SCHEMA [spu]
GO
