CREATE TABLE dbo.Product
( ProductId INT PRIMARY KEY IDENTITY(1,1) Not Null,
  ProductName VARCHAR(100),
  Deleted bit not null 
);