-- Created by Vertabelo (http://vertabelo.com)
-- Script type: drop
-- Scope: [tables, references, sequences, views, procedures]
-- Generated at Wed Mar 04 06:24:05 UTC 2015



-- foreign keys
ALTER TABLE AllowedConnections DROP FOREIGN KEY AllowedConnections_Child;
ALTER TABLE AllowedConnections DROP FOREIGN KEY AllowedConnections_Guardian;
ALTER TABLE AllowedConnections DROP FOREIGN KEY Family_AllowedConnections;
ALTER TABLE ChildcareTransaction DROP FOREIGN KEY Transaction_AllowedConnections;
ALTER TABLE ChildcareTransaction DROP FOREIGN KEY Transaction_BillingData;

-- tables
-- Table Administrator
DROP TABLE Administrator;
-- Table AllowedConnections
DROP TABLE AllowedConnections;
-- Table Child
DROP TABLE Child;
-- Table ChildcareTransaction
DROP TABLE ChildcareTransaction;
-- Table EventData
DROP TABLE EventData;
-- Table Family
DROP TABLE Family;
-- Table Guardian
DROP TABLE Guardian;
-- Table OperatingHours
DROP TABLE OperatingHours;



-- End of file.

