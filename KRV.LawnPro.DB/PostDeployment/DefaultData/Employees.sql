BEGIN
	DECLARE @UserId2 uniqueidentifier;

	SELECT @UserId2 = Id from tblUser where UserName = 'atamminga'
	INSERT INTO DBO.tblEmployee (Id, FirstName, LastName, StreetAddress, City, State, ZipCode, Email, Phone, UserId)
	VALUES (NEWID(), 'Adam', 'Tamminga', '674 2nd Street', 'Menasha', 'WI', '54945', 'ataminga@yahoo.com', '111-111-1111', @UserId2)

	SELECT @UserId2 = Id from tblUser where UserName = 'kvicchiollo'
	INSERT INTO DBO.tblEmployee (Id, FirstName, LastName, StreetAddress, City, State, ZipCode, Email, Phone, UserId)
	VALUES (NEWID(), 'Ken', 'Vicchiollo', 'N1348 Woodland Drive', 'Greenville', 'WI', '54942', 'kenVicchiollo@gmail.com', '920-544-2870', @UserId2)

	SELECT @UserId2 = Id from tblUser where UserName = 'bfoote'
	INSERT INTO DBO.tblEmployee (Id, FirstName, LastName, StreetAddress, City, State, ZipCode, Email, Phone, UserId)
	VALUES (NEWID(), 'Brian', 'Foote', '2345 W 6th Street', 'Oshkosh', 'WI', '54944', 'bfoote@gmail.com', '111-111-1111', @UserId2)
END