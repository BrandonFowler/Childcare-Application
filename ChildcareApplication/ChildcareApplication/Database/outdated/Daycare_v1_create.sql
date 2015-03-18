-- Created by Vertabelo (http://vertabelo.com)
-- Script type: create
-- Scope: [tables, references, sequences, views, procedures]
-- Generated at Wed Jan 28 20:32:34 UTC 2015




-- tables
-- Table AllowedConnections
CREATE TABLE AllowedConnections (
    Connection_ID int    NOT NULL ,
    Guardian_ID int    NOT NULL ,
    Child_ID int    NOT NULL ,
    CONSTRAINT AllowedConnections_pk PRIMARY KEY (Connection_ID)
);

-- Table BillingData
CREATE TABLE BillingData (
    Event_ID int    NOT NULL ,
    EventName varchar(50)    NOT NULL ,
    EventPrice int    NOT NULL ,
    CONSTRAINT BillingData_pk PRIMARY KEY (Event_ID)
);

-- Table Child
CREATE TABLE Child (
    Child_ID int    NOT NULL ,
    FirstName varchar(50)    NOT NULL ,
    LastName varchar(50)    NOT NULL ,
    Birthday date    NOT NULL ,
    Allergies varchar(200)    NOT NULL ,
    Medical varchar(200)    NOT NULL ,
    PhotoLocation varchar(50)    NOT NULL ,
    CONSTRAINT Child_pk PRIMARY KEY (Child_ID)
);

-- Table Guardian
CREATE TABLE Guardian (
    Guardian_ID int    NOT NULL ,
    FirstName varchar(50)    NOT NULL ,
    LastName varchar(50)    NOT NULL ,
    Phone varchar(15)    NOT NULL ,
    Email varchar(50)    NOT NULL ,
    Address1 varchar(50)    NOT NULL ,
    Address2 varchar(50)    NOT NULL ,
    City varchar(50)    NOT NULL ,
    State varchar(2)    NOT NULL ,
    Zip varchar(10)    NOT NULL ,
    PhotoLocation varchar(50)    NOT NULL ,
    CONSTRAINT Guardian_pk PRIMARY KEY (Guardian_ID)
);

-- Table Transaction
CREATE TABLE Transaction (
    Transaction_ID int    NOT NULL ,
    Event_ID int    NOT NULL ,
    Connection_ID int    NOT NULL ,
    Date date    NOT NULL ,
    CheckedIn time    NOT NULL ,
    CheckedOut time    NOT NULL ,
    CONSTRAINT Transaction_pk PRIMARY KEY (Transaction_ID)
);





-- foreign keys
-- Reference:  AllowedConnections_Child (table: AllowedConnections)


ALTER TABLE AllowedConnections ADD CONSTRAINT AllowedConnections_Child FOREIGN KEY AllowedConnections_Child (Child_ID)
    REFERENCES Child (Child_ID);
-- Reference:  AllowedConnections_Guardian (table: AllowedConnections)


ALTER TABLE AllowedConnections ADD CONSTRAINT AllowedConnections_Guardian FOREIGN KEY AllowedConnections_Guardian (Guardian_ID)
    REFERENCES Guardian (Guardian_ID);
-- Reference:  Transaction_AllowedConnections (table: Transaction)


ALTER TABLE Transaction ADD CONSTRAINT Transaction_AllowedConnections FOREIGN KEY Transaction_AllowedConnections (Connection_ID)
    REFERENCES AllowedConnections (Connection_ID);
-- Reference:  Transaction_BillingData (table: Transaction)


ALTER TABLE Transaction ADD CONSTRAINT Transaction_BillingData FOREIGN KEY Transaction_BillingData (Event_ID)
    REFERENCES BillingData (Event_ID);



-- End of file.

