
--Drop the existing database if it exists
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

INSERT INTO [Properties] (
    [HostId], [Title], [Description], [IsActive], [Address], [City], [State], 
    [Country], [ZipCode], [PropertyType], [PricePerNight], [AvailabilityCalendar], 
    [HouseRules], [InstantBooking], [CreatedAt], [UpdatedAt], 
    [ImagePath], [ImagePath2], [ImagePath3], [ImagePath4], [ImagePath5]
) 
VALUES
(3, 'Cozy Studio Apartment', 'A small yet cozy studio apartment, ideal for solo travelers.', 1, '45 MG Road', 'Mumbai', 'Maharashtra', 'India', '400001', 'Studio', 1500.00, '{"availability": "all year"}', 'No pets allowed. Quiet hours after 10 PM.', 1, GETDATE(),GETDATE() , 'image1.jpg', 'image2.jpg', 'image3.jpg', 'image4.jpg', 'image5.jpg'),
(5, 'Modern 2BHK Apartment', 'A modern 2BHK apartment in the heart of Bangalore.', 1, '17 Whitefield', 'Bangalore', 'Karnataka', 'India', '560066', 'Apartment', 3500.00, '{"availability": "all year"}', 'No smoking. No parties.', 1, GETDATE(), GETDATE(), 'image6.jpg', 'image7.jpg', 'image8.jpg', 'image9.jpg', 'image10.jpg'),
(6, 'Luxury Penthouse', 'A luxurious penthouse with a stunning city view in Hyderabad.', 1, '21 Jubilee Hills', 'Hyderabad', 'Telangana', 'India', '500033', 'Penthouse', 8500.00, '{"availability": "all year"}', 'No loud music. No pets.', 1, GETDATE(),GETDATE(), 'image11.jpg', 'image12.jpg', 'image13.jpg', 'image14.jpg', 'image15.jpg'),
(3, 'Heritage Villa', 'Experience royalty at this beautiful heritage villa in Jaipur.', 1, '9 Amer Fort Road', 'Jaipur', 'Rajasthan', 'India', '302002', 'Villa', 4500.00, '{"availability": "Jan-Mar, Oct-Dec"}', 'No smoking inside the property.', 1, GETDATE(),GETDATE(), 'image16.jpg', 'image17.jpg', 'image18.jpg', 'image19.jpg', 'image20.jpg'),
(5, 'Beachfront Cottage', 'A serene beachfront cottage perfect for a peaceful getaway.', 1, '12 Beach Road', 'Goa', 'Goa', 'India', '403001', 'Cottage', 6000.00, '{"availability": "all year"}', 'No bonfires. No parties.', 1, GETDATE(), GETDATE(), 'image21.jpg', 'image22.jpg', 'image23.jpg', 'image24.jpg', 'image25.jpg'),
(6, 'Cozy Hilltop Cabin', 'Charming cabin located in the serene hills of Manali.', 1, '23 Hilltop', 'Manali', 'Himachal Pradesh', 'India', '175131', 'Cabin', 3200.00, '{"availability": "Nov-Apr"}', 'Keep noise levels low after 9 PM.', 1, GETDATE(), GETDATE(), 'image26.jpg', 'image27.jpg', 'image28.jpg', 'image29.jpg', 'image30.jpg'),
(3, 'Downtown 1BHK Flat', 'A 1BHK flat located in the heart of New Delhi.', 1, '98 Connaught Place', 'New Delhi', 'Delhi', 'India', '110001', 'Apartment', 2800.00, '{"availability": "all year"}', 'No loud music. Keep the place tidy.', 1, GETDATE(), GETDATE(), 'image31.jpg', 'image32.jpg', 'image33.jpg', 'image34.jpg', 'image35.jpg'),
(5, 'Riverside Farmhouse', 'Relax by the river at this spacious farmhouse in Kerala.', 1, '77 River Road', 'Alleppey', 'Kerala', 'India', '688001', 'Farmhouse', 5000.00, '{"availability": "all year"}', 'No pets. No swimming after 8 PM.', 1, GETDATE(), GETDATE(), 'image36.jpg', 'image37.jpg', 'image38.jpg', 'image39.jpg', 'image40.jpg'),
(6, 'Modern Studio in Pune', 'A newly furnished studio apartment in Pune.', 1, '12 Koregaon Park', 'Pune', 'Maharashtra', 'India', '411001', 'Studio', 1800.00, '{"availability": "all year"}', 'No pets. No smoking.', 1, GETDATE(),GETDATE(), 'image41.jpg', 'image42.jpg', 'image43.jpg', 'image44.jpg', 'image45.jpg'),
(3, 'Scenic Lakeview Cabin', 'A peaceful cabin with a stunning lake view near Udaipur.', 1, '6 Lake Pichola Road', 'Udaipur', 'Rajasthan', 'India', '313001', 'Cabin', 3700.00, '{"availability": "Nov-Mar"}', 'No bonfires. No pets.', 1, GETDATE(), GETDATE(), 'image46.jpg', 'image47.jpg', 'image48.jpg', 'image49.jpg', 'image50.jpg'),
(5, 'Urban 3BHK Apartment', 'Spacious 3BHK apartment perfect for families in Chennai.', 1, '11 T Nagar', 'Chennai', 'Tamil Nadu', 'India', '600017', 'Apartment', 4000.00, '{"availability": "all year"}', 'No smoking. No parties.', 1, GETDATE(), GETDATE(), 'image51.jpg', 'image52.jpg', 'image53.jpg', 'image54.jpg', 'image55.jpg'),
(6, 'Luxury Villa with Pool', 'A luxurious villa with a private pool in Kochi.', 1, '88 MG Road', 'Kochi', 'Kerala', 'India', '682016', 'Villa', 7500.00, '{"availability": "all year"}', 'No parties. No pets.', 1, GETDATE(), GETDATE(), 'image56.jpg', 'image57.jpg', 'image58.jpg', 'image59.jpg', 'image60.jpg'),
(3, 'Budget Room in Kolkata', 'Affordable room for solo travelers in Kolkata.', 1, '45 Park Street', 'Kolkata', 'West Bengal', 'India', '700016', 'Room', 1200.00, '{"availability": "all year"}', 'No pets. Keep the place tidy.', 1, GETDATE(), GETDATE(), 'image61.jpg', 'image62.jpg', 'image63.jpg', 'image64.jpg', 'image65.jpg'),
(5, 'Rustic Cottage', 'Rustic yet modern cottage in the beautiful hills of Ooty.', 1, '23 Rose Garden Road', 'Ooty', 'Tamil Nadu', 'India', '643001', 'Cottage', 5500.00, '{"availability": "all year"}', 'No pets. No loud music.', 1, GETDATE(), GETDATE(), 'image66.jpg', 'image67.jpg', 'image68.jpg', 'image69.jpg', 'image70.jpg'),
(6, 'Heritage Haveli', 'Stay in a historic haveli with traditional charm in Jodhpur.', 1, '17 Mehrangarh Fort Road', 'Jodhpur', 'Rajasthan', 'India', '342001', 'Haveli', 4700.00, '{"availability": "Oct-Apr"}', 'No pets. No smoking.', 1, GETDATE(), GETDATE(), 'image71.jpg', 'image72.jpg', 'image73.jpg', 'image74.jpg', 'image75.jpg');