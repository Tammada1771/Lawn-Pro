BEGIN
	DECLARE @UserId uniqueidentifier;

	SELECT @UserId = Id from tblUser where UserName = 'bsmith'
	INSERT INTO DBO.tblCustomer (Id, FirstName, LastName, StreetAddress, City, State, ZipCode, Email, Phone, PropertySqFt, UserId)
	VALUES (NEWID(), 'Bill', 'Smith', 'N1348 Woodland Drive', 'Greenville', 'WI', '54942', 'bsmith@yahoo.com', '999-999-9999', 33000, @UserId)

	SELECT @UserId = Id from tblUser where UserName = 'jjones'
	INSERT INTO DBO.tblCustomer (Id, FirstName, LastName, StreetAddress, City, State, ZipCode, Email, Phone, PropertySqFt, UserId)
	VALUES (NEWID(), 'Jill', 'Jones', '1825 N Bluemound Dr', 'Appleton', 'WI', '54912', 'jill_jones@gmail.com', '333-333-3333', 24000, @UserId)

	SELECT @UserId = Id from tblUser where UserName = 'gbailey'
	INSERT INTO DBO.tblCustomer (Id, FirstName, LastName, StreetAddress, City, State, ZipCode, Email, Phone, PropertySqFt, UserId)
	VALUES (NEWID(), 'George', 'Bailey', '625 Weatherstone Dr', 'Oshkosh', 'WI', '54901', 'georgebailey@yahoo.com', '555-555-5555', 31000, @UserId)

	SELECT @UserId = Id from tblUser where UserName = 'cbrady'
	INSERT INTO DBO.tblCustomer (Id, FirstName, LastName, StreetAddress, City, State, ZipCode, Email, Phone, PropertySqFt, UserId)
	VALUES (NEWID(), 'Cindy', 'Brady', '4567 East Street', 'Appleton', 'WI', '54912', 'cindybrady@yahoo.com', '222-222-2222', 0, @UserId)
END