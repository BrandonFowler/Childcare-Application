-- Created by Vertabelo (http://vertabelo.com)
-- Script type: create
-- Scope: [tables, references, sequences, views, procedures]
-- Generated at Mon Feb 23 21:17:17 UTC 2015




-- tables
-- Table Administrator
CREATE TABLE Administrator (
    AdministratorUN varchar(50)    NOT NULL ,
    AdministratorPW varchar(20)    NOT NULL ,
    AccessLevel int    NOT NULL ,
    AdministratorEmail varchar(50)    NOT NULL ,
    CONSTRAINT Administrator_pk PRIMARY KEY (AdministratorUN)
);

-- Table AllowedConnections
CREATE TABLE AllowedConnections (
    Allowance_ID int    NOT NULL ,
    Guardian_ID int    NOT NULL ,
    Child_ID int    NOT NULL ,
    Family_ID int    NOT NULL ,
    CONSTRAINT AllowedConnections_pk PRIMARY KEY (Allowance_ID)
);

-- Table Child
CREATE TABLE Child (
    Child_ID int    NOT NULL ,
    FirstName varchar(50)    NOT NULL ,
    LastName varchar(50)    NOT NULL ,
    Birthday date    NOT NULL ,
    Allergies varchar(200)    NULL ,
    Medical varchar(200)    NULL ,
    PhotoLocation varchar(50)    NOT NULL ,
    CONSTRAINT Child_pk PRIMARY KEY (Child_ID)
);

-- Table ChildcareTransaction
CREATE TABLE ChildcareTransaction (
    ChildcareTransaction_ID int    NOT NULL ,
    Event_ID int    NOT NULL ,
    Allowance_ID int    NOT NULL ,
    TransactionDate date    NOT NULL ,
    CheckedIn time    NOT NULL ,
    CheckedOut time    NULL ,
    TransactionTotal float    NULL ,
    CONSTRAINT ChildcareTransaction_pk PRIMARY KEY (ChildcareTransaction_ID)
);

-- Table EventData
CREATE TABLE EventData (
    Event_ID int    NOT NULL ,
    EventName varchar(50)    NOT NULL ,
    HourlyPrice float    NULL ,
    HourlyDiscount float    NULL ,
    DailyPrice float    NULL ,
    DailyDiscount float    NULL ,
    EventMonth int    NULL ,
    EventDay int    NULL ,
    EventWeekday varchar(10)    NULL ,
    CONSTRAINT EventData_pk PRIMARY KEY (Event_ID)
);

-- Table Family
CREATE TABLE Family (
    Family_ID int    NOT NULL ,
    FamilyTotal float    NULL ,
    CONSTRAINT Family_pk PRIMARY KEY (Family_ID)
);

-- Table Guardian
CREATE TABLE Guardian (
    Guardian_ID int    NOT NULL ,
    GuardianPIN int    NOT NULL ,
    FirstName varchar(50)    NOT NULL ,
    LastName varchar(50)    NOT NULL ,
    Phone varchar(15)    NOT NULL ,
    Email varchar(50)    NULL ,
    Address1 varchar(50)    NOT NULL ,
    Address2 varchar(50)    NULL ,
    City varchar(50)    NOT NULL ,
    StateAbrv varchar(2)    NOT NULL ,
    Zip varchar(10)    NOT NULL ,
    PhotoLocation varchar(50)    NOT NULL ,
    CONSTRAINT Guardian_pk PRIMARY KEY (Guardian_ID)
);

-- Table OperatingHours
CREATE TABLE OperatingHours (
    OperatingWeekday varchar(10)    NOT NULL ,
    Opening time    NOT NULL ,
    Closing time    NOT NULL ,
    CONSTRAINT OperatingHours_pk PRIMARY KEY (OperatingWeekday)
);





-- foreign keys
-- Reference:  AllowedConnections_Child (table: AllowedConnections)


ALTER TABLE AllowedConnections ADD CONSTRAINT AllowedConnections_Child FOREIGN KEY AllowedConnections_Child (Child_ID)
    REFERENCES Child (Child_ID);
-- Reference:  AllowedConnections_Guardian (table: AllowedConnections)


ALTER TABLE AllowedConnections ADD CONSTRAINT AllowedConnections_Guardian FOREIGN KEY AllowedConnections_Guardian (Guardian_ID)
    REFERENCES Guardian (Guardian_ID);
-- Reference:  Family_AllowedConnections (table: AllowedConnections)


ALTER TABLE AllowedConnections ADD CONSTRAINT Family_AllowedConnections FOREIGN KEY Family_AllowedConnections (Family_ID)
    REFERENCES Family (Family_ID);
-- Reference:  Transaction_AllowedConnections (table: ChildcareTransaction)


ALTER TABLE ChildcareTransaction ADD CONSTRAINT Transaction_AllowedConnections FOREIGN KEY Transaction_AllowedConnections (Allowance_ID)
    REFERENCES AllowedConnections (Allowance_ID);
-- Reference:  Transaction_BillingData (table: ChildcareTransaction)


ALTER TABLE ChildcareTransaction ADD CONSTRAINT Transaction_BillingData FOREIGN KEY Transaction_BillingData (Event_ID)
    REFERENCES EventData (Event_ID);



-- End of file.

