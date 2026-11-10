SELECT name
FROM sys.databases;
SELECT DB_NAME() AS CurrentDatabase;
-- GO


USE ecommercedb;
SELECT name
FROM sys.tables;
-- DROP TABLE dbo.Product;
-- SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
select * from dbo.Product;
GO