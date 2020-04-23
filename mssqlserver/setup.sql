CREATE DATABASE UserManagementDB;
GO
USE UserManagementDB;
GO
CREATE TABLE users (
	id INT IDENTITY(1,1) NOT NULL,
	firstName VARCHAR(32),
	lastName VARCHAR(32),
	gender BIT,
	password VARCHAR(255),
	emailAddress VARCHAR(255),
	phoneNumber VARCHAR(32),
	isVerified BIT DEFAULT 'false',
	isDeleted BIT DEFAULT 'false',
	PRIMARY KEY (id)
);
GO