CREATE TABLE tuotteet (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50), hinta INTEGER);

CREATE TABLE asiakkaat (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi VARCHAR(50), osoite VARCHAR(150), puhelin VARCHAR(50));

CREATE TABLE tilaukset (id INTEGER IDENTITY(1,1) PRIMARY KEY, asiakas_id INTEGER REFERENCES asiakkaat ON DELETE CASCADE, tuote_id INTEGER REFERENCES tuotteet ON DELETE CASCADE, toimitettu BIT DEFAULT 0);

INSERT INTO tuotteet (nimi, hinta) VALUES ('juusto', 6);
INSERT INTO asiakkaat (nimi, osoite, puhelin) VALUES ('Masa', 'Kuusikuja 6', '050882682');
INSERT INTO tilaukset (asiakas_id, tuote_id) VALUES (1,1); 

DELETE FROM tuotteet WHERE id=5;

SELECT * FROM tuotteet;
SELECT * FROM asiakkaat;
SELECT * FROM tilaukset;

UPDATE tilaukset SET toimitettu=1 WHERE id=1

SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id

DELETE FROM tuotteet WHERE nimi="kinkku";

DROP TABLE tilaukset;

CREATE TABLE Tuotteet (
    id INTEGER IDENTITY(1,1) PRIMARY KEY,
    Artisti VARCHAR(100) NOT NULL,
    Levyn_Nimi VARCHAR(100) NOT NULL,
    Hinta DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Asiakkaat (
    id INT IDENTITY(1,1) PRIMARY KEY,
    Nimi VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL
);

CREATE TABLE Tilaukset (
    id INT IDENTITY(1,1) PRIMARY KEY,
    AsiakasID INT NOT NULL,
    TuoteID INT NOT NULL,
    Tilauspäivä DATETIME NOT NULL,
    Määrä INT NOT NULL,
    Kokonaishinta DECIMAL(10, 2) NOT NULL,
    Myyjä INT NOT NULL,
    FOREIGN KEY (Myyjä) REFERENCES Myyjat(id),
    FOREIGN KEY (AsiakasID) REFERENCES Asiakkaat(id),
    FOREIGN KEY (TuoteID) REFERENCES Tuotteet(id)
);

CREATE TABLE Myyjat (
    id INT IDENTITY(1,1) PRIMARY KEY,
    Nimi VARCHAR(50) NOT NULL
);

ALTER TABLE Asiakkaat
ADD osoite VARCHAR(50) NOT NULL;






