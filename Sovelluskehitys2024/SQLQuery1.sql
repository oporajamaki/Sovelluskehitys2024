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

ALTER TABLE toimitetut DROP CONSTRAINT FK_toimitetut_tilaus;
ALTER TABLE toimitetut
ADD CONSTRAINT FK_toimitetut_tilaus FOREIGN KEY (tilaus_id) REFERENCES tilaus (tilaus_id);

SELECT 
    CONSTRAINT_NAME
FROM 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE 
    TABLE_NAME = 'toimitetut' AND CONSTRAINT_TYPE = 'FOREIGN KEY';

ALTER TABLE toimitetut DROP CONSTRAINT FK__toimitetu__tilau__52593CB8;
ALTER TABLE toimitetut
ADD CONSTRAINT FK_toimitetut_tilaus FOREIGN KEY (tilaus_id) REFERENCES tilaus (tilaus_id);

ALTER TABLE toimitetut DROP CONSTRAINT FK_toimitetut_tilaus;
ALTER TABLE toimitetut
ADD CONSTRAINT FK_toimitetut_tilaus FOREIGN KEY (tilaus_id) REFERENCES tilaus (id);

EXEC sp_help 'toimitetut';

DROP TABLE IF EXISTS toimitetut;
DROP TABLE IF EXISTS tilaus;
DROP TABLE IF EXISTS asiakkaat;
DROP TABLE IF EXISTS myyjat;

-- Poistetaan olemassa olevat taulut, jos ne ovat jo tietokannassa
DROP TABLE IF EXISTS toimitetut;
DROP TABLE IF EXISTS tilaukset;
DROP TABLE IF EXISTS tilaus;
DROP TABLE IF EXISTS tuotteet;
DROP TABLE IF EXISTS asiakkaat;
DROP TABLE IF EXISTS myyjat;

-- Luodaan taulu myyjät
CREATE TABLE myyjat (
    id INTEGER IDENTITY(1,1) PRIMARY KEY,
    nimi VARCHAR(50)
);

-- Luodaan taulu asiakkaat
CREATE TABLE asiakkaat (
    id INTEGER IDENTITY(1,1) PRIMARY KEY,
    nimi VARCHAR(50),
    email VARCHAR(150),
    osoite VARCHAR(50)
);

-- Luodaan taulu tuotteet
CREATE TABLE tuotteet (
    id INTEGER IDENTITY(1,1) PRIMARY KEY,
    artisti VARCHAR(50),
    levyn_nimi VARCHAR(150),
    hinta DECIMAL
);

-- Luodaan taulu tilaus
CREATE TABLE tilaus (
    id INTEGER IDENTITY(1,1) PRIMARY KEY,
    asiakas_id INTEGER NOT NULL REFERENCES asiakkaat(id),
    myyja_id INTEGER NOT NULL REFERENCES myyjat(id),
    paiva DATETIME DEFAULT GETDATE()
);

-- Luodaan taulu tilaukset (tilauskohtaiset tuotteet)
CREATE TABLE tilaukset (
    id INTEGER IDENTITY(1,1) PRIMARY KEY,
    tuote_id INTEGER NOT NULL REFERENCES tuotteet(id),
    tilaus_id INTEGER NOT NULL REFERENCES tilaus(id),
    maara INTEGER NOT NULL
);

-- Luodaan taulu toimitetut
CREATE TABLE toimitetut (
    id INTEGER IDENTITY(1,1) PRIMARY KEY,
    tilaus_id INTEGER NOT NULL REFERENCES tilaus(id),
    tuote_id INTEGER NOT NULL REFERENCES tuotteet(id),
    toimituspaiva DATETIME DEFAULT GETDATE()
);

-- Lisää testidataa
INSERT INTO myyjat (nimi) VALUES ('Maija Myyjä');
INSERT INTO asiakkaat (nimi, email, osoite) VALUES ('Matti Meikäläinen', 'matti@esimerkki.com', 'Esimerkkitie 1');
INSERT INTO tuotteet (artisti, levyn_nimi, hinta) VALUES ('Artist A', 'Levy A', 19.99);

-- Lisää tilaus
INSERT INTO tilaus (asiakas_id, myyja_id) VALUES (1, 1);

-- Lisää tilauksen tuotteet
INSERT INTO tilaukset (tuote_id, tilaus_id, maara) VALUES (1, 1, 2);

-- Lisää toimitus
INSERT INTO toimitetut (tilaus_id, tuote_id) VALUES (1, 1);

-- Tarkista tiedot
SELECT * FROM myyjat;
SELECT * FROM asiakkaat;
SELECT * FROM tuotteet;
SELECT * FROM tilaus;
SELECT * FROM tilaukset;
SELECT * FROM toimitetut;





