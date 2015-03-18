INSERT into Guardian VALUES ("123450", "1234", "Bilbo", "Baggins", "555-123-4567", "myprecious@theshire.com",
								"123 Bag End", "Hole A", "Shire", "ME", "99999", "../../Pictures/Bilbo_Baggins.jpg", NULL);
				
INSERT into Child VALUES ("123451", "Frodo", "Baggins", 2005-02-15, "Rings of Power", "Addicted to the Ring of power",
								"../../Pictures/Frodo_Baggins.jpg");

INSERT into AllowedConnections VALUES ("000001", "123450", "123451", "12345");

Insert into ChildcareTransaction VALUES ("0000000001","000002", "000001", '2015-03-12 12:00:00', '12:00:00', '14:00:00', 10.00);

Insert into ChildcareTransaction VALUES ("0000000002","000002", "000001", '2015-03-12 12:00:00', '12:00:00', '14:00:00', 10.05);

Insert into ChildcareTransaction VALUES ("0000000003","000002", "000001", '2015-03-12 12:00:00', '12:00:00', '14:00:00', 10.50);

Insert into ChildcareTransaction VALUES ("0000000004","000002", "000001", '2015-03-12 12:00:00', '12:00:00', '14:00:00', 10.55);

Insert into ChildcareTransaction VALUES ("0000000005","000002", "000001", '2015-02-12 12:00:00', '12:00:00', '14:00:00', 10.55);