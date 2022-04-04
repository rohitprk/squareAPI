create database SquareAPI;
GO
use SquareAPI;
GO
create table UserPoints
(
	UserId int not null,
	X int not null,
	Y int not null,
	Constraint PK_UserPoints_UserId_X_Y PRIMARY KEY(UserId, X, Y)
)
GO
create table Users
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Name] VARCHAR(500) NOT NULL,
	[Password] VARCHAR(500) NOT NULL 
)

GO
create nonclustered index IX_Users_Name_Password 
      on Users ([Name] ASC, [Password] ASC) 
GO
Create table UserSquarePoints
(
	UserId INT NOT NULL,
	SquarePointJson TEXT NOT NULL,
	LastUpdateTime DATETIME NULL,
	CONSTRAINT PK_UserSquarePoints_UserId PRIMARY KEY(UserId)
)