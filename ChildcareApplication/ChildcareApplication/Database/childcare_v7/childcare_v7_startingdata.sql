INSERT INTO Administrator
VALUES ("a",
	"a",
	"1",
	"tester@testmail.test",
	NULL);

INSERT INTO EventData
VALUES ("Late Fee",
        30,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
	NULL,
		NULL);

INSERT INTO EventData
VALUES ("Regular Childcare",
        5,
        2.25,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
	3,
		NULL);

INSERT INTO EventData
VALUES ("Adolescent Childcare",
        0,
        0,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
	3,
		NULL);

INSERT INTO EventData
VALUES ("Infant Childcare",
        6,
        3.25,
        NULL,
        NULL,
        NULL,
        NULL,
        NULL,
	2,
		NULL);

INSERT INTO EventData
VALUES ("Parent's Night Out",
        NULL,
        NULL,
        18,
        7,
        NULL,
        NULL,
        "Friday",
	NULL,
		NULL);

INSERT INTO EventData
VALUES ("Camp",
        NULL,
        NULL,
        36,
        26,
        NULL,
        NULL,
        NULL,
	NULL,
		NULL);

INSERT INTO OperatingHours
VALUES ("Monday", '08:00:00', '19:00:00');

INSERT INTO OperatingHours
VALUES ("Tuesday", '08:00:00', '19:00:00');

INSERT INTO OperatingHours
VALUES ("Wednesday", '08:00:00', '19:00:00');

INSERT INTO OperatingHours
VALUES ("Thursday", '08:00:00', '19:00:00');

INSERT INTO OperatingHours
VALUES ("Friday", '08:00:00', '20:00:00');

INSERT INTO OperatingHours
VALUES ("Saturday", '08:00:00', '12:30:00');

INSERT INTO ApplicationSettings
VALUES ("Billing Start Date", "20");

INSERT INTO ApplicationSettings
VALUES ("Billing End Date", "19");

INSERT INTO ApplicationSettings
VALUES ("Regular Care Maximum Monthly Charge", "100");

INSERT INTO ApplicationSettings
VALUES ("Days to Hold Expired and Deleted Records", "365");

INSERT INTO ApplicationSettings
VALUES ("Maximum Number of Hours Charged for Childcare", "3");

INSERT INTO ApplicationSettings
VALUES ("Infant Age End", "1");

INSERT INTO ApplicationSettings
VALUES ("Regular Age End", "8");