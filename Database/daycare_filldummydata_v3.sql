INSERT INTO administrator
VALUES ("full access admin",
        "testadminpw",
        1,
        "admin@email.com");

INSERT INTO administrator
VALUES ("second level admin",
        "testadminpw2",
        2,
        "employee@email.com");

INSERT INTO administrator
VALUES ("third level admin",
        "amIneeded",
        3,
        "2deep4me@email.com");

INSERT INTO child
VALUES (000001,
        "firstborn",
        "adam",
        '2000-01-01',
        "haha no allergies",
        "medical shmedical",
        "c:\somewhere.jpg");

INSERT INTO child
VALUES (000002,
        "secondborn",
        "eve",
        '2000-01-01',
        "haha no allergies",
        "medical shmedical",
        "c:\somewhere.jpg");

INSERT INTO child
VALUES (000003,
        "thirdborn",
        "missy elliot",
        '2000-01-01',
        "haha no allergies",
        "medical shmedical",
        "c:\somewhere.jpg");

INSERT INTO child
VALUES (000004,
        "fourthborn",
        "organ donor",
        '2000-01-01',
        "haha no allergies",
        "medical shmedical",
        "c:\somewhere.jpg");

INSERT INTO guardian
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

INSERT INTO guardian
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

INSERT INTO guardian
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

INSERT INTO allowedconnections
VALUES (1, 1, 1);

INSERT INTO allowedconnections
VALUES (2, 1, 2);

INSERT INTO allowedconnections
VALUES (3, 1, 3);

INSERT INTO allowedconnections
VALUES (4, 2, 1);

INSERT INTO allowedconnections
VALUES (5, 3, 4);

INSERT INTO eventdata
VALUES (1,
        "the always event",
        1.5,
        1.2,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL);

INSERT INTO eventdata
VALUES (2,
        "the sometimes event",
        NULL,
        NULL,
        4.5,
        3.5,
        NULL,
        NULL,
        "Friday");

INSERT INTO transaction
VALUES (1,
        1,
        1,
        curdate(),
        curtime(),
        curtime(),
        10);

INSERT INTO transaction
VALUES (2,
        2,
        5,
        curdate(),
        curtime(),
        curtime(),
        10);

INSERT INTO operatinghours
VALUES ("monday", 0800, 1900);

INSERT INTO operatinghours
VALUES ("Sunday", 0000, 0000);