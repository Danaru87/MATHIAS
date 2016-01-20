﻿CREATE TABLE TRIGGERCMD
(
	ID INTEGER PRIMARY KEY AUTOINCREMENT,
	SENID INTEGER NOT NULL,
	CMDID INTEGER NOT NULL,
	FOREIGN KEY(SENID) REFERENCES SENTENCES(ID),
	FOREIGN KEY(CMDID) REFERENCES COMMAND(ID)
);

CREATE UNIQUE INDEX SEN_CMD_ID on TRIGGERCMD (SENID, CMDID);