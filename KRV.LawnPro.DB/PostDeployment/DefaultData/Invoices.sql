BEGIN
	DECLARE @InvoiceCustomerId uniqueidentifier;

	SELECT @InvoiceCustomerId = Id from tblCustomer where LastName = 'Bailey'
	INSERT INTO DBO.tblInvoice (Id, CustomerId, ServiceName, ServiceRate, ServiceDate, EmployeeName, Status)
	VALUES	(NEWID(), @InvoiceCustomerId, 'Mow', 0.0015, '20210426 9:30:00 AM', 'Tamminga, Adam', 'Paid')

	SELECT @InvoiceCustomerId = Id from tblCustomer where LastName = 'Smith'
	INSERT INTO DBO.tblInvoice (Id, CustomerId, ServiceName, ServiceRate, ServiceDate, EmployeeName, Status)
	VALUES	(NEWID(), @InvoiceCustomerId, 'Mow', 0.0015, '20210426 10:30:00 AM', 'Tamminga, Adam', 'Issued')

	SELECT @InvoiceCustomerId = Id from tblCustomer where LastName = 'Jones'
	INSERT INTO DBO.tblInvoice (Id, CustomerId, ServiceName, ServiceRate, ServiceDate, EmployeeName, Status)
	VALUES	(NEWID(), @InvoiceCustomerId, 'Mow', 0.0015, '20210427 9:00:00 AM', 'Vicchiollo, Ken', 'Paid')

	SELECT @InvoiceCustomerId = Id from tblCustomer where LastName = 'Jones'
	INSERT INTO DBO.tblInvoice (Id, CustomerId, ServiceName, ServiceRate,ServiceDate, EmployeeName, Status)
	VALUES	(NEWID(), @InvoiceCustomerId, 'Mow', 0.0013, '20210428 9:00:00 AM', 'Vicchiollo, Ken', 'Issued')

END