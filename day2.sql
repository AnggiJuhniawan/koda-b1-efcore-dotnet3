BEGIN TRANSACTION;
ALTER TABLE [dbo].[Product] ADD [Description] nvarchar(500) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261109062257_AddDescriptionToProduct', N'10.0.14');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [dbo].[Orders] (
    [OrderId] bigint NOT NULL IDENTITY,
    [OrderNumber] nvarchar(35) NOT NULL,
    [OrderDate] datetimeoffset NOT NULL,
    [Status] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
    CONSTRAINT [CK_Orders_Status] CHECK ([Status] In ('Pending', 'Confirmed))
);

CREATE TABLE [dbo].[OrderItem] (
    [OrderItemId] bigint NOT NULL IDENTITY,
    [OrderId] bigint NOT NULL,
    [ProductCode] nvarchar(30) NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [Subtotal] AS [Quantity] * [UnitPrice] PERSISTED,
    CONSTRAINT [PK_OrderItem] PRIMARY KEY ([OrderItemId]),
    CONSTRAINT [CK_OrderItem_Quantity] CHECK ([Quantity] > 0),
    CONSTRAINT [CK_OrderItem_UnitPrice] CHECK ([UnitPrice] >= 0),
    CONSTRAINT [FK_OrderItem_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([OrderId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_OrderItem_Product_ProductCode] FOREIGN KEY ([ProductCode]) REFERENCES [dbo].[Product] ([ProductCode]) ON DELETE NO ACTION
);

CREATE UNIQUE INDEX [IX_OrderItem_OrderId_ProductCode] ON [dbo].[OrderItem] ([OrderId], [ProductCode]);

CREATE INDEX [IX_OrderItem_ProductCode] ON [dbo].[OrderItem] ([ProductCode]);

CREATE INDEX [IX_Orders_OrderDate] ON [dbo].[Orders] ([OrderDate]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261110040726_AddOrderAndOrderItem', N'10.0.14');

COMMIT;
GO

