﻿CREATE TABLE [dbo].[tblUser]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [UserName] VARCHAR(25) NOT NULL, 
    [UserPass] VARCHAR(32) NOT NULL
)