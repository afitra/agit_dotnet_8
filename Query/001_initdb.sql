CREATE TABLE production (
    id INT IDENTITY(1,1) PRIMARY KEY,
    senin INT,
    selasa INT,
    rabo INT,
    kamis INT,
    jumaat INT,
    sabtu INT,
    minggu INT,
    remap_senin INT,
    remap_selasa INT,
    remap_rabo INT,
    remap_kamis INT,
    remap_jumat INT,
    remap_sabtu INT,
    remap_minggu INT,
    created_at DATETIME DEFAULT GETDATE()
);
