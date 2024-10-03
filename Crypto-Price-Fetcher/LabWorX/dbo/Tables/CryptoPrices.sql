﻿CREATE TABLE [dbo].[CryptoPrices]
(
	[Id] UniqueIdentifier NOT NULL  PRIMARY KEY DEFAULT(NEWID()),
	[Crypto] VARCHAR(10) NOT NULL,
	[Price] DECIMAL(10, 2) NOT NULL, 
    [TimeStamp] DATETIME NOT NULL DEFAULT(GETDATE())
)
