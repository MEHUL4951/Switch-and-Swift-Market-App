
Create Table ADMIN
(    
    Admin_Name varchar(50) NOT NULL,
	Admin_Email varchar(50) NOT NULL UNIQUE,
    Admin_Password varchar(100) NOT NULL
)

Create Table COMMENTS
(
    Comment_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY ,
	UserID int NOT NULL  Foreign key references USERS(UserID),
	Post_ID int NOT NULL Foreign key references POSTS(Post_ID),
	Comment varchar(300) NOT NULL,
	Comment_date_time DATETIME NOT NULL
)


Create Table POSTS
(
	Post_ID int NOT NULL IDENTITY(1,1) PRIMARY KEY ,
    UserID int NOT NULL  Foreign key references USERS(UserID),
    Product_ID int NOT NULL  Foreign key references PRODUCTS(Product_ID),
	Post_date_time DATETIME NOT NULL,
	
)

CREATE TABLE PRODUCTS
(
	Product_ID int IDENTITY(1,1) PRIMARY KEY,
	UserID int NOT NULL FOREIGN KEY REFERENCES USERS(UserID),
	product_category varchar(50) NOT NULL,
	product_price int NOT NULL,
	product_brand varchar(50) NOT NULL,
	product_model varchar(50) NOT NULL,
	product_details varchar(200),
	product_warranty varchar(50),
	product_usage varchar(50),
	product_condition varchar(50),
)


CREATE TABLE ORDERS
(	
	order_id int IDENTITY(1,1) PRIMARY KEY,
	post_id int NOT NULL FOREIGN KEY REFERENCES Posts(post_id),
	buyer_id int NOT NULL FOREIGN KEY REFERENCES USERS(UserId), 
	seller_id int NOT NULL FOREIGN KEY REFERENCES USERS(UserId),
	selling_date datetime NOT NULL,
	selling_price int NOT NULL,
)


CREATE TABLE USERREVIEW
(
	review_id int IDENTITY(1,1) PRIMARY KEY,
	reviewee_id int NOT NULL FOREIGN KEY REFERENCES USERS(UserID),
	reviewer_id int NOT NULL FOREIGN KEY REFERENCES USERS(UserID), 
	review_description varchar(200) NOT NULL
)



CREATE TABLE [dbo].[USERS] (
    [UserID]    INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (50)  NOT NULL,
    [LastName]  VARCHAR (50)  NOT NULL,
    [District]  VARCHAR (50)  NOT NULL,
    [Location]  VARCHAR (200) NOT NULL,
    [Email]     VARCHAR (100) NOT NULL,
    [Phone]     VARCHAR (20)  NOT NULL,
    [Password]  VARCHAR (30)  NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC),
    UNIQUE NONCLUSTERED ([LastName] ASC)
);

