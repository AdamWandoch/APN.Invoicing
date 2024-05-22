﻿CREATE TABLE [dbo].[INVOICE]
(
    [InvoiceID] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [CustomerID] INT NOT NULL, 
    [Date] DATETIMEOFFSET NOT NULL,
    [Month] SMALLINT NOT NULL, 
    [Year] SMALLINT NOT NULL
);
GO

CREATE NONCLUSTERED INDEX [IX_INVOICE_CustomerID] ON [dbo].[INVOICE] ([CustomerID]);
GO
CREATE NONCLUSTERED INDEX [IX_INVOICE_Month] ON [dbo].[INVOICE] ([Month]);
GO
CREATE NONCLUSTERED INDEX [IX_INVOICE_Year] ON [dbo].[INVOICE] ([Year]);
GO