BEGIN
	DECLARE @CustomerId uniqueidentifier;
	DECLARE @EmployeeId uniqueidentifier;
	DECLARE @ServiceID uniqueidentifier;

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Bailey'
	SELECT @EmployeeId = Id from tblEmployee where LastName = 'Tamminga'
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210426 8:00:00 AM', '20210426 9:30:00 AM', @ServiceID, 'Completed')

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Smith'
	SELECT @EmployeeId = Id from tblEmployee where LastName = 'Tamminga'
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210426 10:00:00 AM', '20210426 10:30:00 AM', @ServiceID, 'Completed')

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Jones'
	SELECT @EmployeeId = Id from tblEmployee where LastName = 'Vicchiollo'
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210427 8:00:00 AM', '20210427 9:00:00 AM', @ServiceID, 'Completed')

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Smith'
	SELECT @EmployeeId = Id from tblEmployee where LastName = 'Vicchiollo'
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210427 10:00:00 AM', '20210427 11:00:00 AM', @ServiceID, 'Scheduled')

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Bailey'
	SELECT @EmployeeId = Id from tblEmployee where LastName = 'Vicchiollo'
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210427 11:00:00 AM', '20210427 12:00:00 PM', @ServiceID, 'Scheduled')

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Jones'
	SELECT @EmployeeId = Id from tblEmployee where LastName = 'Vicchiollo'
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210428 8:00:00 AM', '20210428 9:00:00 AM', @ServiceID, 'Completed')

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Smith'
	SELECT @EmployeeId = Id from tblEmployee where LastName = 'Vicchiollo'
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210428 10:00:00 AM', '20210428 11:00:00 AM', @ServiceID, 'Scheduled')

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Bailey'
	SELECT @EmployeeId = Id from tblEmployee where LastName = 'Vicchiollo'
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210428 11:00:00 AM', '20210428 12:00:00 PM', @ServiceID, 'Scheduled')

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Bailey'
	SELECT @EmployeeId = Id from tblEmployee where LastName = 'Foote'
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210528 11:00:00 AM', '20210528 12:00:00 PM', @ServiceID, 'Scheduled')

	SELECT @CustomerId = Id from tblCustomer where LastName = 'Bailey'
	SELECT @EmployeeId = null
	SELECT @ServiceID = Id from tblServiceType where Description = 'Mow'
	INSERT INTO DBO.tblAppointment (Id, CustomerId, EmployeeId, StartDateTime, EndDateTime, ServiceID, Status)
	VALUES	(NEWID(), @CustomerId, @EmployeeId, '20210522 11:00:00 AM', '20210522 11:00:00 AM', @ServiceID, 'Unscheduled')

END