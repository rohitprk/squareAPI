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