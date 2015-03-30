--if not exists (select Id from ExamUnits where Name = 'Beheer 1')
delete from ExamUnits
DBCC CHECKIDENT ('dbo.ExamUnits', RESEED, 0)
insert into ExamUnits
values	('Beheer 1'),
		('Computer organisatie 1'),
		('Databases 1'),
		('Discrete Wiskunde'),
		('Embedded Systems A'),
		('Engels Inf'),
		('Programmeren 1'),
		('Programmeren 2'),
		('Programmeren 3'),
		('Project P1P'),
		('Project P2P'),
		('Project P3P'),
		('Security'),
		('Software Engineering'),
		('Web Development'),
		('Wiskunde basis Inf')

delete from Classes
DBCC CHECKIDENT ('dbo.Classes', RESEED, 0)
insert into Classes
values	('I1A', 'Informatica', 1),
		('I1B', 'Informatica', 1),
		('I1C', 'Informatica', 1),
		('I1D', 'Informatica', 1)
