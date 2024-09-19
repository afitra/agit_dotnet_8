
--no 1


CREATE TABLE Fakultas (
    KodeFakultas INT,         
    NamaFakultas VARCHAR(30), 
    NamaDekan VARCHAR(50)     
);

--no 2

CREATE TABLE Prodi (
    KodeProdi INT,              
    KodeFakultas INT,           
    NamaProdi VARCHAR(30),      
    NamaKetuaProdi VARCHAR(50)  
);

INSERT INTO Prodi (KodeProdi, KodeFakultas, NamaProdi, NamaKetuaProdi)
VALUES (101, 10, 'Teknik Informatika', 'Dr. Budi Santoso');

-- no 3

CREATE TABLE Mahasiswa (
    NPM CHAR(8) PRIMARY KEY,    -- Field 1: NPM dengan tipe Text, panjang 8 karakter, dan menjadi PRIMARY KEY
    KodeProdi INT,              -- Field 2: KodeProdi dengan tipe Number (Integer)
    NamaMahasiswa VARCHAR(50),   -- Field 3: NamaMahasiswa dengan tipe Text (ukuran 50 karakter)
    TempatLahir VARCHAR(30),     -- Field 4: TempatLahir dengan tipe Text (ukuran 30 karakter)
    TanggalLahir DATE,           -- Field 5: TanggalLahir dengan tipe Date/Time
    Alamat VARCHAR(100)          -- Field 6: Alamat dengan tipe Text (ukuran 100 karakter)
);

INSERT INTO Mahasiswa (NPM, KodeProdi, NamaMahasiswa, TempatLahir, TanggalLahir, Alamat)
VALUES ('12345678', 101, 'Andi Kurniawan', 'Jakarta', '1999-03-15', 'Jl. Merdeka No. 10');

-- no4 
ALTER TABLE Fakultas
    ALTER COLUMN KodeFakultas INT NOT NULL;

ALTER TABLE Fakultas
    ADD CONSTRAINT PK_KodeFakultas PRIMARY KEY (KodeFakultas);

-- no 5
ALTER TABLE Prodi
ALTER COLUMN KodeProdi INT NOT NULL;

ALTER TABLE Prodi
ALTER COLUMN KodeFakultas INT NOT NULL;

ALTER TABLE Prodi
ADD CONSTRAINT PK_ProdiFakultas PRIMARY KEY (KodeProdi, KodeFakultas);


-- no 6 a

INSERT INTO Fakultas (KodeFakultas, NamaFakultas, NamaDekan)
VALUES
(1, 'Teknik', 'Ahmad Riyadi'),
(2, 'Pertanian', 'Paiman'),
(3, 'Ekonomi', 'Sukhemi'),
(4, 'Keguruan', 'Suharni');

-- 6b
INSERT INTO Prodi (KodeProdi, KodeFakultas, NamaProdi, NamaKetuaProdi)
VALUES
(11, 1, 'Teknik Informatika', 'Bachtiar Dwi Effendi'),
(21, 2, 'Agroteknologi', 'Bahrum'),
(31, 3, 'Manajemen', 'Vita'),
(32, 3, 'Akuntansi', 'Siti Maisaroh'),
(41, 4, 'PPKN', 'Sigit'),
(42, 4, 'Sejarah', 'Gunawan'),
(43, 4, 'Pendidikan Matematika', 'Tri'),
(44, 4, 'Bimbingan Konseling', 'Siswanti'),
(45, 4, 'PGSD', 'Haniek');

-- 6c
INSERT INTO Mahasiswa (NPM, KodeProdi, NamaMahasiswa, TempatLahir, TanggalLahir, Alamat)
VALUES
('08110167', 11, 'Andi', 'Jakarta', '1988-03-12', 'Gunung Kidul'),
('08110231', 11, 'Joko', 'Sleman', '1989-02-01', 'Sleman'),
('08210232', 21, 'Budi', 'Bantul', '1988-09-15', 'Bantul'),
('08210233', 21, 'Cici', 'Purwokerto', '1989-02-21', 'Bantul'),
('08310234', 31, 'Didi', 'Bandung', '1987-07-11', 'Kodya'),
('08320235', 32, 'Alfin', 'Makassar', '1986-09-22', 'Kodya'),
('08320236', 32, 'Dodi', 'Gunung Kidul', '1979-03-24', 'Kodya'),
('08320237', 32, 'Derri', 'Pangkal Pinang', '1984-09-09', 'Sleman'),
('08410121', 41, 'Dudung', 'Kebumen', '1985-02-25', 'Sleman'),
('08410122', 41, 'Afgan', 'Palembang', '1985-11-21', 'Kulon Progo'),
('08420123', 42, 'Didi', 'Kutoarjo', '1986-09-11', 'Kulon Progo'),
('08430124', 43, 'Firza', 'Purworejo', '1986-09-11', 'Bantul'),
('08440125', 44, 'Zahir', 'Temon', '1986-09-11', 'Kulon Progo');

-- 7 
ALTER TABLE Mahasiswa
ADD TanggalDaftar DATE;

-- 8
SELECT NamaMahasiswa, Alamat
FROM Mahasiswa
WHERE YEAR(TanggalLahir) BETWEEN 1970 AND 1979;

-- 9
SELECT 
    Mahasiswa.NamaMahasiswa, 
    Prodi.NamaProdi
FROM 
    Mahasiswa
JOIN 
    Prodi
ON 
    Mahasiswa.KodeProdi = Prodi.KodeProdi;

-- 10

SELECT TOP 3
    Mahasiswa.NamaMahasiswa,
    Mahasiswa.Alamat
FROM
    Mahasiswa
        JOIN
    Prodi ON Mahasiswa.KodeProdi = Prodi.KodeProdi
        JOIN
    Fakultas ON Prodi.KodeFakultas = Fakultas.KodeFakultas
WHERE
        Fakultas.NamaFakultas = 'Teknik'
ORDER BY
    Mahasiswa.TanggalLahir ASC;
    
    
-- 11
SELECT COUNT(*) AS JumlahMahasiswa
FROM Mahasiswa
WHERE Alamat = 'Sleman';

--12
UPDATE Mahasiswa
SET TanggalDaftar = '2013-09-03';

--13
SELECT *
FROM Mahasiswa
WHERE NamaMahasiswa LIKE 'D%';

-- 14
UPDATE Mahasiswa
SET TanggalLahir = '1990-01-20'
WHERE NamaMahasiswa = 'Joko';

--15

SELECT Prodi.NamaProdi, COUNT(Mahasiswa.NPM) AS JumlahMahasiswa
FROM Mahasiswa
JOIN Prodi ON Mahasiswa.KodeProdi = Prodi.KodeProdi
GROUP BY Prodi.NamaProdi;

