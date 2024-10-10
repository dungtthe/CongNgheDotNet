CREATE DATABASE QuanLySanPham;
GO

USE QuanLySanPham;
GO

CREATE TABLE Catalog (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CatalogCode NVARCHAR(50) NULL,
    CatalogName NVARCHAR(250) NULL
);
GO

CREATE TABLE Product (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CatalogId INT NULL,
    ProductCode NVARCHAR(50) NULL,
    ProductName NVARCHAR(250) NULL,
    Picture NVARCHAR(MAX) NULL,
    UnitPrice FLOAT NULL,
    CONSTRAINT FK_Product_Catalog FOREIGN KEY (CatalogId) REFERENCES Catalog(Id)
);
GO

GO
INSERT INTO Catalog (CatalogCode, CatalogName)
VALUES
('DM01', N'Điện Thoại'),
('DM02', N'Máy Tính'),
('DM03', N'Thời Trang'),
('DM04', N'Gia Dụng'),
('CAT113', N'Hàng Cháy Nổ');
GO

GO
INSERT INTO Product (CatalogId, ProductCode, ProductName, Picture, UnitPrice)
VALUES
(1, 'PRO01', N'SamSung J7', 'PRO01.PNG', 10000000),
(1, 'PRO02', N'Iphone X', 'PRO02.PNG', 30000000),
(2, 'PRO03', N'Dell Inspiron 5', 'PRO03.PNG', 12000000),
(2, 'PRO04', N'Dell Inspiron 7', 'PRO04.PNG', 13000000),
(2, 'PRO05', N'Acer AS A315', 'PRO05.PNG', 9000000);

GO

