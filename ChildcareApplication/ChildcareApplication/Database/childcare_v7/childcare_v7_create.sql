-- Created by Vertabelo (http://vertabelo.com)
-- Script type: create
-- Scope: [tables, references, sequences, views, procedures]
-- Generated at Tue Mar 17 20:37:22 UTC 2015



-- tables
-- Table: Administrator
CREATE TABLE Administrator (
    AdministratorUN varchar(50) NOT NULL  PRIMARY KEY,
    AdministratorPW varchar(200) NOT NULL,
    AccessLevel varchar(1) NOT NULL,
    AdministratorEmail varchar(100) NOT NULL,
	AdminDeletionDate date
);

-- Table: AllowedConnections
CREATE TABLE AllowedConnections (
    Allowance_ID varchar(6) NOT NULL  PRIMARY KEY,
    Guardian_ID varchar(6) NOT NULL,
    Child_ID varchar(6) NOT NULL,
    Family_ID varchar(6) NOT NULL,
	ConnectionDeletionDate date,
    FOREIGN KEY (Guardian_ID) REFERENCES Guardian (Guardian_ID),
    FOREIGN KEY (Child_ID) REFERENCES Child (Child_ID),
    FOREIGN KEY (Family_ID) REFERENCES Family (Family_ID)
);

-- Table: Child
CREATE TABLE Child (
    Child_ID varchar(6) NOT NULL  PRIMARY KEY,
    FirstName varchar(50) NOT NULL,
    LastName varchar(50) NOT NULL,
    Birthday date NOT NULL,
    Allergies varchar(200),
    Medical varchar(200),
    PhotoLocation varchar(200) NOT NULL,
    ChildDeletionDate date
);

-- Table: ChildcareTransaction
CREATE TABLE ChildcareTransaction (
    ChildcareTransaction_ID varchar(10) NOT NULL  PRIMARY KEY,
    EventName varchar(50) NOT NULL,
    Allowance_ID varchar(6) NOT NULL,
    TransactionDate date NOT NULL,
    CheckedIn time NOT NULL,
    CheckedOut time,
    TransactionTotal float,
    FOREIGN KEY (Allowance_ID) REFERENCES AllowedConnections (Allowance_ID),
    FOREIGN KEY (EventName) REFERENCES EventData (EventName)
);

-- Table: EventData
CREATE TABLE EventData (
    EventName varchar(50) NOT NULL PRIMARY KEY,
    HourlyPrice float,
    HourlyDiscount float,
    DailyPrice float,
    DailyDiscount float,
    EventMonth int,
    EventDay int,
    EventWeekday varchar(10),
    EventMaximumHours int,
    EventDeletionDate date
);

-- Table: Family
CREATE TABLE Family (
    Family_ID varchar(5) NOT NULL  PRIMARY KEY,
    FamilyTotal float
);

-- Table: Guardian
CREATE TABLE Guardian (
    Guardian_ID varchar(6) NOT NULL  PRIMARY KEY,
    GuardianPIN varchar(200) NOT NULL,
    FirstName varchar(50) NOT NULL,
    LastName varchar(50) NOT NULL,
    Phone varchar(15) NOT NULL,
    Email varchar(50),
    Address1 varchar(50) NOT NULL,
    Address2 varchar(50),
    City varchar(50) NOT NULL,
    StateAbrv varchar(2) NOT NULL,
    Zip varchar(10) NOT NULL,
    PhotoLocation varchar(200) NOT NULL,
    GuardianDeletionDate date
);



-- End of file.

