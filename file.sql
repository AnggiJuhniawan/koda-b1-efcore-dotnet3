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

SELECT i.name AS IndexName,
i.type_desc AS IndexType,
i.is_unique AS IsUnique
From sys.indexes AS i
WHERE i.object_id = OBJECT_ID(N'dbo.Product')
    AND i.name is NOT NULL
ORDER BY i.name;
GO