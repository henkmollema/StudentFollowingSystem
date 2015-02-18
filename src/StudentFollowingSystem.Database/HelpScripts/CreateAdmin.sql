/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
insert into Counselers ([FirstName], [LastName], [Password], [Email])
values ('admin', 'nhl', 'AK0dgCVhobAtanfvGa457mOVkt6Q5gEfVBhZ6jY9ho4rIZOTcmxhX9hjS9z0Y9a8ig==', 'admin@nhl.nl')