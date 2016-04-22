CREATE TABLE SENTENCES ( 
	SENID INTEGER PRIMARY KEY AUTOINCREMENT, 
	SENTENCE TEXT NOT NULL,
	LANG TEXT NOT NULL DEFAULT 'FR'
);

INSERT INTO SENTENCES (SENTENCE) VALUES ('Bonjour'), ('Salut'), ('Coucou'), ('Bonjour mathias');
INSERT INTO SENTENCES (SENTENCE) VALUES ('Fermeture'), ('Bonne nuit'), ('Fermeture de mathias');
INSERT INTO SENTENCES (SENTENCE) VALUES ('Tu vas bien'), ('ça va'), ('La forme');
INSERT INTO SENTENCES (SENTENCE) VALUES ('Mise en veille'), ('Ta gueule');
INSERT INTO SENTENCES (SENTENCE) VALUES ('Sortie de veille');
INSERT INTO SENTENCES (SENTENCE) VALUES ('Changement de contexte'),('Chargement du contexte'),('Modification du contexte');
INSERT INTO SENTENCES (SENTENCE) VALUES ('Messagerie'),('i mèl');
