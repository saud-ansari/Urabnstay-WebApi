
-- Drop the existing database if it exists
--DROP DATABASE [Urbanstay]
--GO

-- If the database is not deleting showing error "Cannot drop database "Urbanstay" because it is currently in use".
--ALTER DATABASE Urbanstay SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

-- Create a new database
CREATE DATABASE [Urbanstay]
GO

USE [Urbanstay]

--****************************************************************************************************
-- Create the Role table
CREATE TABLE [Role]
(
    [Id] INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_Role_Id PRIMARY KEY,
    [Name] VARCHAR(50) NOT NULL CONSTRAINT uk_Role_Name UNIQUE,
    [Description] VARCHAR(100) NOT NULL,
    [IsActive] BIT NOT NULL,
    [AddedDate] DATETIME NOT NULL,
    [AddedById] INT,
    [ModifiedDate] DATETIME,
    [ModifiedById] INT
)

-- Insert initial data into the Role table
INSERT INTO [Role] (Name, Description, IsActive, AddedDate, AddedById, ModifiedDate, ModifiedById) 
VALUES

('Super Admin', 'Highest level of control, managing other admins, including system-wide settings.', 1, GETDATE(), 1, NULL, NULL),
('Admin', 'Full access to all data and functionality', 1, GETDATE(), 1, NULL, NULL),
('Landlords', 'Permissions to list properties, manage bookings, respond to guest inquiries, and manage reviews.', 1, GETDATE(), 1, NULL, NULL),
('Tenants', 'Permissions to search, book properties, leave reviews, and manage their bookings.', 1, GETDATE(), 1, NULL, NULL);
SELECT * FROM [Role]

--****************************************************************************************************
-- Create the User table
CREATE TABLE [User]
(
    [Id] INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_User_Id PRIMARY KEY,
    [FirstName] VARCHAR(20) NOT NULL,
    [LastName] VARCHAR(20) NOT NULL,
    [Username] VARCHAR(20) NOT NULL CONSTRAINT uk_User_Username UNIQUE,
    [Password] VARCHAR(32) NOT NULL,
    [MobileNo] VARCHAR(12),
    [Email] VARCHAR(50),
    [RoleId] INT CONSTRAINT fk_User_RoleId FOREIGN KEY REFERENCES [Role](Id),
    [ProfilePic] VARCHAR(50),
    [SysAdmin] BIT,
    [IsActive] BIT NOT NULL,
    [AddedDate] DATETIME NOT NULL,
    [AddedById] INT CONSTRAINT fk_User_AddedById FOREIGN KEY REFERENCES [User](Id),
    [ModifiedDate] DATETIME,
    [ModifiedById] INT CONSTRAINT fk_User_ModifiedById FOREIGN KEY REFERENCES [User](Id)
)

-- Insert initial data into the User table
INSERT INTO [User] (FirstName, LastName, Username, Password, MobileNo, Email, RoleId, ProfilePic, SysAdmin, IsActive, AddedDate, AddedById, ModifiedDate, ModifiedById) 
VALUES
('Saud', 'Ansari', 'saud', '123', '9999999999', 'saud@gmail.com', 1, 'saud.jpg', 1, 1, GETDATE(), 1, NULL, NULL),
('John', 'Doe', 'johndoe', 'pass123', '1999999999', 'johndoe@gmail.com', 2, 'john.jpg', 1, 1, GETDATE(), 1, NULL, NULL),
('Jane', 'Smith', 'janesmith', 'pass123', '2999999999', 'janesmith@gmail.com', 3, 'jane.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('Michael', 'Johnson', 'mjohnson', 'pass123', '3999999999', 'mjohnson@gmail.com', 4, 'michael.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('Emily', 'Davis', 'emilyd', 'pass123', '4999999999', 'emilyd@gmail.com', 3, 'emily.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('David', 'Williams', 'davidw', 'pass123', '5999999999', 'davidw@gmail.com', 3, 'david.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('Sophia', 'Brown', 'sophiab', 'pass123', '6999999999', 'sophiab@gmail.com', 4, 'sophia.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('James', 'Jones', 'jamesj', 'pass123', '7999999999', 'jamesj@gmail.com', 4, 'james.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('Olivia', 'Garcia', 'oliviag', 'pass123', '8999999999', 'oliviag@gmail.com', 4, 'olivia.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('William', 'Martinez', 'william', 'pass123', '0999999999', 'william@gmail.com', 4, 'william.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('Ava', 'Rodriguez', 'avar', 'pass123', '1099999999', 'avar@gmail.com', 4, 'ava.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('Liam', 'Miller', 'liamm', 'pass123', '1199999999', 'liamm@gmail.com', 4, 'liam.jpg', 0, 1, GETDATE(), 1, NULL, NULL),
('Mia', 'Wilson', 'miaw', 'pass123', '1299999999', 'miaw@gmail.com', 4, 'mia.jpg', 0, 1, GETDATE(), 1, NULL, NULL);

SELECT * FROM [User]

--****************************************************************************************************
-- Create the Properties table
CREATE TABLE [Properties]
(
    [PropertyId] INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_Property_Id PRIMARY KEY,
    [HostId] INT NOT NULL CONSTRAINT fk_Property_HostId FOREIGN KEY REFERENCES [User](Id) ON DELETE CASCADE,
    [Title] VARCHAR(255) NOT NULL,
    [Description] TEXT NOT NULL,
	[IsActive] BIT NOT NULL,
    [Address] VARCHAR(255) NOT NULL,
    [City] VARCHAR(100) NOT NULL,
    [State] VARCHAR(100) NOT NULL,
    [Country] VARCHAR(100) NOT NULL,
    [ZipCode] VARCHAR(20) NOT NULL,
    [PropertyType] VARCHAR(50) NOT NULL,
    [PricePerNight] DECIMAL(10,2) NOT NULL,
    [AvailabilityCalendar] NVARCHAR(MAX) NOT NULL, -- JSON data
    [HouseRules] TEXT,
    [InstantBooking] BIT DEFAULT 0,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [UpdatedAt] DATETIME DEFAULT GETDATE(),
    [ImagePath] VARCHAR(50),
    [ImagePath2] VARCHAR(50),
    [ImagePath3] VARCHAR(50),
    [ImagePath4] VARCHAR(50),
    [ImagePath5] VARCHAR(50)
)

--SELECT * FROM [Properties]

--****************************************************************************************************
-- Create the Bookings table
CREATE TABLE [Bookings]
(
	[BookingId] INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_Booking_Id PRIMARY KEY,
	[PropertyId] INT NOT NULL CONSTRAINT fk_Booking_PropertyId FOREIGN KEY REFERENCES [Properties](PropertyId) ON DELETE CASCADE,
	[GuestId] INT NOT NULL CONSTRAINT fk_Booking_GuestId FOREIGN KEY REFERENCES [User](Id),
	[HostId] INT NOT NULL CONSTRAINT fk_Booking_HostId FOREIGN KEY REFERENCES [User](Id),
	[CheckInDate] DATE NOT NULL,
	[CheckOutDate] DATE NOT NULL,
	[NumberOfGuests] INT NOT NULL,
	[TotalPrice] DECIMAL(10,2) NOT NULL,
	[Status] VARCHAR(20) NOT NULL DEFAULT 'pending',
	[CreatedAt] DATETIME DEFAULT GETDATE(),
	[UpdatedAt] DATETIME DEFAULT GETDATE()
)

--****************************************************************************************************
-- Create the Payments table
CREATE TABLE [Payments]
(
	[PaymentId] INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_Payment_Id PRIMARY KEY,
	[BookingId] INT NOT NULL CONSTRAINT fk_Payment_BookingId FOREIGN KEY REFERENCES [Bookings](BookingId) ON DELETE CASCADE,
	[Amount] DECIMAL(10,2) NOT NULL,
	[PaymentMethod] VARCHAR(50) NOT NULL,
	[PaymentStatus] VARCHAR(20) NOT NULL DEFAULT 'pending',
	[TransactionId] VARCHAR(100) NOT NULL CONSTRAINT uk_Payment_TransactionId UNIQUE,
	[CreatedAt] DATETIME DEFAULT GETDATE(),
	[UpdatedAt] DATETIME DEFAULT GETDATE()
)

--****************************************************************************************************
-- Create the Reviews table
CREATE TABLE [Reviews]
(
	[ReviewId] INT NOT NULL IDENTITY(1,1) CONSTRAINT pk_Review_Id PRIMARY KEY,
	[PropertyId] INT NOT NULL CONSTRAINT fk_Review_PropertyId FOREIGN KEY REFERENCES [Properties](PropertyId),
	[ReviewerId] INT NOT NULL CONSTRAINT fk_Review_ReviewerId FOREIGN KEY REFERENCES [User](Id),
	[Comment] TEXT,
	[CreatedAt] DATETIME DEFAULT GETDATE()
)

