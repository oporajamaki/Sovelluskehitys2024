CREATE TABLE tuotteet (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50), hinta INTEGER);

CREATE TABLE asiakkaat(id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50), osoite VARCHAR(150), puhelin varchar(50));

CREATE TABLE tilaukset(id INTEGER IDENTITY(1,1) PRIMARY KEY, asiakas_id INTEGER REFERENCES asiakkaat ON DELETE CASCADE, tuote_id INTEGER REFERENCES tuotteet ON DELETE CASCADE);

INSERT INTO asiakkaat (nimi,osoite,puhelin) VALUES ('Matti Meikäläinen','Kehätie 13','0400875454');

INSERT INTO tilaukset (asiakas_id,tuote_id) VALUES (1,1);

INSERT INTO tuotteet (nimi, hinta) VALUES ('juusto', 6);

SELECT * FROM tuotteet;
SELECT * FROM asiakkaat;
SELECT * FROM tilaukset;

DELETE FROM tuotteet WHERE id=1;
DROP TABLE tuotteet;