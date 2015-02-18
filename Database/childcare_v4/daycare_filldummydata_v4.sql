INSERT INTO Administrator
VALUES ("full access admin",
        "testadminpw",
        1,
        "admin@email.com");

INSERT INTO Administrator
VALUES ("second level admin",
        "testadminpw2",
        2,
        "employee@email.com");

INSERT INTO Administrator
VALUES ("third level admin",
        "amIneeded",
        3,
        "2deep4me@email.com");

INSERT INTO Child
VALUES (000001,
        "firstborn",
        "adam",
        '2000-01-01',
        "haha no allergies",
        "medical shmedical",
        "c:\somewhere.jpg");

INSERT INTO Child
VALUES (000002,
        "secondborn",
        "eve",
        '2000-01-01',
        "haha no allergies",
        "medical shmedical",
        "c:\somewhere.jpg");

INSERT INTO Child
VALUES (000003,
        "thirdborn",
        "missy elliot",
        '2000-01-01',
        "haha no allergies",
        "medical shmedical",
        "c:\somewhere.jpg");

INSERT INTO Child
VALUES (000004,
        "fourthborn",
        "organ donor",
        '2000-01-01',
        "haha no allergies",
        "medical shmedical",
        "c:\somewhere.jpg");

INSERT INTO Guardian
VALUES (000001,
        1234,
        "Mommy",
        "Dearest",
        "(509) 867-5309",
        "mom@email.com",
        "123 street",
        "apt a",
        "Spokane",
        "WA",
        "99999",
        "c:\somewhere.jpg");

INSERT INTO Guardian
VALUES (000002,
        1234,
        "Daddy",
        "Dearest",
        "(509) 867-5309",
        "dad@email.com",
        "123 street",
        "apt a",
        "Spokane",
        "WA",
        "99999",
        "c:\somewhere.jpg");

INSERT INTO Guardian
VALUES (000003,
        1234,
        "Uncle",
        "Fester",
        "(509) 123-4567",
        "uncle@email.com",
        "666 avenue blvd",
        "",
        "Spokane",
        "WA",
        "99999",
        "c:\somewhere.jpg");

INSERT INTO AllowedConnections
VALUES (1, 1, 1);

INSERT INTO AllowedConnections
VALUES (2, 1, 2);

INSERT INTO AallowedConnections
VALUES (3, 1, 3);

INSERT INTO AllowedConnections
VALUES (4, 2, 1);

INSERT INTO AllowedConnections
VALUES (5, 3, 4);

INSERT INTO EventData
VALUES (1,
        "the always event",
        1.5,
        1.2,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL);

INSERT INTO EventData
VALUES (2,
        "the sometimes event",
        NULL,
        NULL,
        4.5,
        3.5,
        NULL,
        NULL,
        "Friday");

INSERT INTO ChildCareTransaction
VALUES (1,
        1,
        1,
        curdate(),
        curtime(),
        curtime(),
        10);

INSERT INTO ChildCareTransaction
VALUES (2,
        2,
        5,
        curdate(),
        curtime(),
        curtime(),
        10);

INSERT INTO OperatingHours
VALUES ("monday", 0800, 1900);

INSERT INTO OperatingHours
VALUES ("Sunday", 0000, 0000);