ALTER TABLE [dbo].tblInvoice
	ADD CONSTRAINT [tblInvoice_CustomerId]
	FOREIGN KEY (CustomerId)
	REFERENCES tblCustomer (Id) ON DELETE CASCADE