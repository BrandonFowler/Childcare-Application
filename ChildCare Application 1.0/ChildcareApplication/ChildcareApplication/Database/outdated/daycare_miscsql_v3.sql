-- AddGuardian (input all data at guardian creation)

INSERT INTO guardian
VALUES (4,
        1234,
        "Gandalf",
        "Wizard",
        "no phone",
        "no email",
        "no address",
        "",
        "no city",
        "no",
        "no zip",
        "no photo");

-- EditGuardian (can edit each attribute individually, input guardian id and the fields you wish to change)

UPDATE guardian
   SET City = "Middle Earth", PhotoLocation = 'c:\happywizard.png'
 WHERE Guardian_ID = 4;

-- RetrieveGuardian (generic select, show all guardian info from a guardian id)

SELECT *
  FROM guardian
 WHERE Guardian_ID = 4;

-- GuardiansAllowedChildren (show connections allowed based on guardian id)

SELECT guardian.FirstName, child.FirstName, child.LastName
  FROM child, allowedconnections, guardian
 WHERE     allowedconnections.Guardian_ID = 1
       AND allowedconnections.Child_ID = child.Child_ID
GROUP BY child.Child_ID;

-- AddChild (input all data at child creation)

INSERT INTO child
VALUES (5,
        "Frodo",
        "Baggins",
        '1999-12-12',
        "",
        "",
        "");

-- EditChild (can edit each attribute individually, input child id and the fields you wish to change)

UPDATE child
   SET PhotoLocation = 'c:\happyhobbit.png'
 WHERE Child_ID = 5;

-- RetrieveChild (generic select, show all child info from a child id)

SELECT *
  FROM child
 WHERE Child_ID = 5;

-- ChildrensAllowedGuardians (show connections allowed based on child id)

SELECT child.FirstName, guardian.FirstName, guardian.LastName
  FROM guardian, allowedconnections, child
 WHERE     allowedconnections.Child_ID = 1
       AND allowedconnections.Guardian_ID = guardian.Guardian_ID
GROUP BY guardian.Guardian_ID;

-- AddAdmin (add a new administrator with the associated attributes)

INSERT INTO administrator
VALUES ("BillyTheKid",
        "snapedies",
        1,
        "snowshoes@email.com");

-- EditAdmin (edit an administrator based on username, choose fields)

UPDATE administrator
   SET AccessLevel = 3
 WHERE AdministratorUN = "BillyTheKid";

-- RetrieveAdmin (currently no administrator id to select by so using name, maybe add an id field just for sql statements?)

SELECT *
  FROM administrator
 WHERE AdministratorUN = "full access admin";

-- AddEvent

INSERT INTO eventdata
VALUES (3,
        "the new event",
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL);

-- EditEvent

UPDATE eventdata
   SET Weekday = "Tuesday"
 WHERE Event_ID = 3;

-- RetrieveEvent (find event data by event ID)

SELECT *
  FROM eventdata
 WHERE Event_ID = 1;

-- AddConnection (add a new connection with associated info)

INSERT INTO allowedconnections
VALUES (6, 4, 4);

-- EditConnection (edit parts of a connection, either changing child id or guardian id)

UPDATE allowedconnections
   SET Child_ID = 5
 WHERE Connection_ID = 6;

-- RetrieveConnection (find connection data by connection ID)

SELECT *
  FROM allowedconnections
 WHERE Connection_ID = 1;

-- RetrieveTransaction (find childcaretransaction data by childcaretransaction ID)

SELECT *
  FROM childcaretransaction
 WHERE Transaction_ID = 1;

-- FindOpenTransaction (get open transactions based on guardian ID)

SELECT *
  FROM childcaretransaction, allowedconnections
 WHERE     childcaretransaction.CheckedOut IS NULL
       AND childcaretransaction.Connection_ID = allowedconnections.Connection_ID
       AND childcaretransaction.Connection_ID = 1;

-- CheckIn/AddTransaction (insert a new childcaretransaction with a null checkout and total)

INSERT INTO childcaretransaction
VALUES (12,
        1,
        1,
        curdate(),
        curtime(),
        NULL,
        NULL);

-- CheckOut/EditTransaction (updates the childcaretransaction based on childcaretransaction id, need to create a method for total cost calculation probably)

UPDATE childcaretransaction
   SET CheckedOut = curtime(), TransactionTotal = 10
 WHERE Transaction_ID = 12;