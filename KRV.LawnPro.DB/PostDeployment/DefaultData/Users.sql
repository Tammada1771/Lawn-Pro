BEGIN

	INSERT INTO DBO.tblUser (Id, FirstName, LastName, UserName, UserPass)
	VALUES
		(NEWID(), 'Adam', 'Tamminga', 'atamminga', 'password'),
		(NEWID(), 'Ken', 'Vicchiollo', 'kvicchiollo', 'password'),
		(NEWID(), 'Brian', 'Foote', 'bfoote', 'maple'),
		(NEWID(), 'Bill', 'Smith', 'bsmith', 'password'),
		(NEWID(), 'Jill', 'Jones', 'jjones', 'password'),
		(NEWID(), 'George', 'Bailey', 'gbailey', 'password'),
		(NEWID(), 'Cindy', 'Brady', 'cbrady', 'password')
END