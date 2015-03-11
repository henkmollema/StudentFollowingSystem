if not exists (select Id from Counselers where FirstName = 'david')
	insert into Counselers ([FirstName], [LastName], [Password], [Email])
	values ('David', 'Schweizer', 'AK0dgCVhobAtanfvGa457mOVkt6Q5gEfVBhZ6jY9ho4rIZOTcmxhX9hjS9z0Y9a8ig==', 'david.schweizer@nhl.nl')