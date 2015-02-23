-- AddGuardian (input all data at guardian creation)

INSERT INTO Guardian
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

UPDATE Guardian
   SET City = "Middle Earth", PhotoLocation = 'c:\happywizard.png'
 WHERE Guardian_ID = 4;

-- RetrieveGuardian (generic select, show all guardian info from a guardian id)

SELECT *
  FROM Guardian
 WHERE Guardian_ID = 4;

-- GuardiansAllowedChildren (show connections allowed based on guardian id)

SELECT Guardian.FirstName, Child.FirstName, Child.LastName
  FROM Child, AllowedConnections, Guardian
 WHERE     AllowedConnections.Guardian_ID = 1
       AND AllowedConnections.Child_ID = Child.Child_ID
GROUP BY Child.Child_ID;

-- AddChild (input all data at child creation)

INSERT INTO Child
VALUES (5,
        "Frodo",
        "Baggins",
        '1999-12-12',
        "",
        "",
        "");

-- EditChild (can edit each attribute individually, input child id and the fields you wish to change)

UPDATE Child
   SET PhotoLocation = 'c:\happyhobbit.png'
 WHERE Child_ID = 5;

-- RetrieveChild (generic select, show all child info from a child id)

SELECT *
  FROM Child
 WHERE Child_ID = 5;

-- ChildrensAllowedGuardians (show connections allowed based on child id)

SELECT Child.FirstName, Guardian.FirstName, Guardian.LastName
  FROM Guardian, AllowedConnections, child
 WHERE     AllowedConnections.Child_ID = 1
       AND AllowedConnections.Guardian_ID = Guardian.Guardian_ID
GROUP BY Guardian.Guardian_ID;

-- AddAdmin (add a new administrator with the associated attributes)

INSERT INTO Administrator
VALUES ("BillyTheKid",
        "snapedies",
        1,
        "snowshoes@email.com");

-- EditAdmin (edit an administrator based on username, choose fields)

UPDATE Administrator
   SET AccessLevel = 3
 WHERE AdministratorUN = "BillyTheKid";

-- RetrieveAdmin (currently no administrator id to select by so using name, maybe add an id field just for sql statements?)

SELECT *
  FROM Administrator
 WHERE AdministratorUN = "full access admin";

-- AddEvent

INSERT INTO EventData
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

UPDATE EventData
   SET EventWeekday = "Tuesday"
 WHERE Event_ID = 3;

-- RetrieveEvent (find event data by event ID)

SELECT *
  FROM EventData
 WHERE Event_ID = 1;

-- AddConnection (add a new connection with associated info)

INSERT INTO AllowedConnections
VALUES (6, 4, 4);

-- EditConnection (edit parts of a connection, either changing child id or guardian id)

UPDATE AllowedConnections
   SET Child_ID = 5
 WHERE Allowance_ID = 6;

-- RetrieveConnection (find connection data by connection ID)

SELECT *
  FROM AllowedConnections
 WHERE Allowance_ID = 1;

-- RetrieveTransaction (find childcaretransaction data by childcaretransaction ID)

SELECT *
  FROM ChildcareTransaction
 WHERE Transaction_ID = 1;

-- FindOpenTransaction (get open transactions based on guardian ID)

SELECT *
  FROM ChildcareTransaction, AllowedConnections
 WHERE     ChildcareTransaction.CheckedOut IS NULL
       AND ChildcareTransaction.Allowance_ID =
              AllowedConnections.Allowance_ID
       AND ChildcareTransaction.Allowance_ID = 1;

-- CheckIn/AddTransaction (insert a new childcaretransaction with a null checkout and total)

INSERT INTO ChildcareTransaction
VALUES (12,
        1,
        1,
        curdate(),
        curtime(),
        NULL,
        NULL);

-- CheckOut/EditTransaction (updates the childcaretransaction based on childcaretransaction id, need to create a method for total cost calculation probably)

UPDATE ChildcareTransaction
   SET CheckedOut = curtime(), TransactionTotal = 10
 WHERE Transaction_ID = 12;