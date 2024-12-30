CREATE TABLE tuotteet (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50), hinta INTEGER);

CREATE TABLE asiakkaat (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50), osoite VARCHAR(150), puhelin VARCHAR(50));

CREATE TABLE tilaukset (id INTEGER IDENTITY(1,1) PRIMARY KEY, asiakas_id INTEGER REFERENCES asiakkaat ON DELETE CASCADE, tuote_id INTEGER REFERENCES tuotteet ON DELETE CASCADE, toimitettu BIT DEFAULT 0);

INSERT INTO tuotteet (nimi, hinta) VALUES ('juusto', 6);
INSERT INTO asiakkaat (nimi, osoite, puhelin) VALUES ('Masa', 'Kuusikuja 6', '050882682');
INSERT INTO tilaukset (asiakas_id, tuote_id) VALUES (1,1); 

DELETE FROM asiakkaat WHERE id=3;

SELECT * FROM tuotteet;
SELECT * FROM asiakkaat;
SELECT * FROM tilaukset;
SELECT * FROM tilaus;
SELECT * FROM toimitetut;

UPDATE tilaukset SET toimitettu=1 WHERE id=1

SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id

DELETE FROM tuotteet WHERE nimi="kinkku";

DROP TABLE Asiakkaat;
DROP TABLE Myyjat;
DROP TABLE Tuotteet;

CREATE TABLE myyjat (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50));
CREATE TABLE asiakkaat (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50), email VARCHAR(150), osoite VARCHAR(50));
CREATE TABLE tuotteet (id INTEGER IDENTITY(1,1) PRIMARY KEY, artisti VARCHAR(50), levyn_nimi VARCHAR(150), hinta DECIMAL);
CREATE TABLE tilaus (id INTEGER IDENTITY(1,1) PRIMARY KEY, asiakas_id INTEGER REFERENCES asiakkaat ON DELETE CASCADE, myyja_id INTEGER REFERENCES myyjat ON DELETE CASCADE, paiva DATETIME);
CREATE TABLE tilaukset (id INTEGER IDENTITY(1,1) PRIMARY KEY, tuote_id INTEGER REFERENCES tuotteet ON DELETE CASCADE, tilaus_id INTEGER REFERENCES tilaus ON DELETE CASCADE, maara INTEGER);
CREATE TABLE toimitetut (
    id INTEGER IDENTITY(1,1) PRIMARY KEY,
    tilaus_id INTEGER REFERENCES tilaus ON DELETE CASCADE,
    tuote_id INTEGER REFERENCES tuotteet ON DELETE CASCADE,
    toimituspaiva DATETIME DEFAULT GETDATE()
);
CREATE TABLE toimitetut (
    id INTEGER IDENTITY(1,1) PRIMARY KEY,
    tilaus_id INTEGER REFERENCES tilaus ON DELETE CASCADE,
    toimituspaiva DATETIME DEFAULT GETDATE()
);
select tuote_id from tilaukset;
DROP TABLE toimitetut;