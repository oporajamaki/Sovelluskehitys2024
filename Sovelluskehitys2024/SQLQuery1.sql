CREATE TABLE tuotteet (id INTEGER IDENTITY(1,1) PRIMARY KEY, nimi TEXT, hinta INTEGER);

INSERT INTO tuotteet (nimi, hinta) VALUES ('juusto', 6);

SELECT * FROM tuotteet;
