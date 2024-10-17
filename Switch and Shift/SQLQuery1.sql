CREATE TABLE USERS
(
	UserID int IDENTITY(1,1) PRIMARY KEY,
	UserName varchar(50) NOT NULL,
	UserEmail varchar(50) NOT NULL UNIQUE,
	UserPhoneNumber varchar(50),
	UserRegion varchar(100),
	UserPassword varchar(100) NOT NULL
)