DROP TABLE Users;

--Creating a table for storing the user details 
CREATE TABLE Users (
  UserId INT PRIMARY KEY,
  UserName VARCHAR(25) CONSTRAINT [UK_UserName] UNIQUE,
  UserEmail VARCHAR(50) CONSTRAINT [UK_UserEmail] UNIQUE,
  Password VARCHAR(25),
  SecurityQn VARCHAR(225),
  SecurityAns VARCHAR(225),
  CreatedDate DATETIME DEFAULT GETDATE()
);

-- Add index on UserId column
CREATE INDEX IX_Users_UserId ON Users (UserId);

--For checking functionality I am entering the data manually
-- Inserting sample data into the Users table
INSERT INTO Users (UserId, UserName, UserEmail, Password, SecurityQn, SecurityAns, CreatedDate)
VALUES
  (1, 'John Doe', 'john.doe@example.com', 'password123', 'What is your favorite color?', 'Blue', GETDATE()),
  (2, 'Jane Smith', 'jane.smith@example.com', 'abc123', 'What is your pet name?', 'Max', GETDATE()),
  (3, 'David Johnson', 'david.johnson@example.com', 'qwerty', 'What city were you born in?', 'New York', GETDATE());

--Creating the procedure to check the User credentials
CREATE OR ALTER PROCEDURE ValidateLogin
@UserEmail Varchar(50),
@UserPassword VARCHAR(25)
AS
BEGIN 
SELECT * FROM Users WHERE UserEmail = @UserEmail AND Password = @UserPassword;
END;

--Creating the procedure to Creating a user profile
CREATE OR ALTER PROCEDURE CreateUser
@UserName VARCHAR(25),
@UserEmail VARCHAR(50),
@Password VARCHAR(25),
@SecurityQn VARCHAR(225),
@SecurityAns VARCHAR(225)
AS
BEGIN
INSERT INTO USERS 
VALUES (@UserName, @UserEmail, @Password,@SecurityQn, @SecurityQn,GETDATE());
END

--Creating Procedure to update the profile
CREATE OR ALTER PROCEDURE UpdateUser
