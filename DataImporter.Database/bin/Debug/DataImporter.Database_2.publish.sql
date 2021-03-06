/*
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
PRINT N'Altering Procedure [dbo].[AddFeeds]...';


GO
ALTER PROCEDURE [dbo].[AddFeeds]
(
	@Feed_ID VARCHAR (MAX),
	@Feed_Name VARCHAR (MAX)
)
AS 
IF (SELECT COUNT(*) FROM Feed WHERE Feed_ID = @Feed_ID) < 1
BEGIN
INSERT INTO [Feed] (Feed_ID,Feed_Name) values (@Feed_ID, @Feed_Name)
END
GO
PRINT N'Altering Procedure [dbo].[AddProducts]...';


GO
ALTER PROCEDURE [dbo].[AddProducts]
(
	@Product_UniqueID INT,
	@Product_Name VARCHAR (MAX),
	@Product_Brand VARCHAR (MAX),
	@Product_Description VARCHAR (MAX),
	@Company_ID INT,
	@Feed_ID INT
)
as
IF (SELECT COUNT(*) FROM Product WHERE Product_UniqueID = @Product_UniqueID) < 1
BEGIN
INSERT INTO [Product] (Product_UniqueID, Product_Name, Product_Brand, Product_Description, Company_ID, Feed_ID) 
		values (@Product_UniqueID, @Product_Name,@Product_Brand,@Product_Description,@Company_ID,@Feed_ID)
END
GO
PRINT N'Update complete.';


GO
