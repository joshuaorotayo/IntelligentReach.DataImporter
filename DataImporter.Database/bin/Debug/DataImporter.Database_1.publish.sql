﻿/*
Deployment script for DataImporterServiceDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "DataImporterServiceDB"
:setvar DefaultFilePrefix "DataImporterServiceDB"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Altering Procedure [dbo].[AddCompanys]...';


GO
ALTER PROCEDURE [dbo].[AddCompanys]
(
	@Company_ID INT,
	@Company_Name VARCHAR (MAX)
)
AS
IF (SELECT COUNT(*) FROM Company WHERE Company_ID = @Company_ID) < 1
BEGIN
INSERT INTO [Company] (Company_ID,Company_Name) values (@Company_ID, @Company_Name)
END
GO
PRINT N'Altering Procedure [dbo].[ClearProducts]...';


GO
ALTER PROCEDURE [dbo].[ClearProducts]

AS

DELETE FROM Product

RETURN
GO
PRINT N'Update complete.';


GO
