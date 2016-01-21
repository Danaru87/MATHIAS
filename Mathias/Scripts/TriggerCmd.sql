﻿CREATE TABLE TRIGGERCMD
(
	ID INTEGER PRIMARY KEY AUTOINCREMENT,
	SENID INTEGER NOT NULL,
	CMDID INTEGER NOT NULL,
	FOREIGN KEY(SENID) REFERENCES SENTENCES(SENID),
	FOREIGN KEY(CMDID) REFERENCES COMMANDS(ID)
);
CREATE UNIQUE INDEX IndexUnique on TRIGGERCMD (SENID, CMDID);

INSERT INTO TRIGGERCMD (CMDID, SENID) VALUES (1,1),(1,2),(1,3),(1,4);
INSERT INTO TRIGGERCMD (CMDID, SENID) VALUES (2,5), (2,6), (2,7);