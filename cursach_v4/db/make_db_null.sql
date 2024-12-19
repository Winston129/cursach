-- CREATE DATABASE cursach_items;
-- DROP DATABASE cursach_items;
GO
USE cursach_items;
GO

-- Тип
CREATE TABLE ItemType (
    ItemTypeID INT PRIMARY KEY IDENTITY(1,1),
    NameType NVARCHAR(50)              -- Название типа вещи
);
GO

-- Клиент
CREATE TABLE Client (
    ClientID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) ,          -- Имя
    LastName NVARCHAR(50) ,           -- Фамилия
    MiddleName NVARCHAR(50) ,         -- Отчество
    PassportData NVARCHAR(50)         -- Паспортные данные
);
GO

-- Вещи, выставленных на продажу
CREATE TABLE Available (
    AvailableID INT PRIMARY KEY IDENTITY(1,1),
    DateListed DATE                    -- Дата
);
GO

-- Вещи в залоге
CREATE TABLE Reserved (
    ReservedID INT PRIMARY KEY IDENTITY(1,1),
    ClientID INT ,                     -- Идентификатор клиента
    ReservedDate DATE ,                -- Когда был сделан залог
    ExpirationDate DATE ,              -- Когда заканчивается срок залога
    InterestRate DECIMAL(5, 2) ,       -- Процентная ставка
);
GO

-- Проданные вещи
CREATE TABLE Sold (
    SoldID INT PRIMARY KEY IDENTITY(1,1),
    ClientID INT ,                      -- Идентификатор клиента
    SaleDate DATE ,                     -- Когда вещь была продана
);
GO

-- Вещи
CREATE TABLE Item (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(100) ,           -- Имя
    ItemTypeID INT ,                   -- Идентификатор типа
    Price DECIMAL(10, 2) ,             -- Цена вещи
    AvailableID INT NULL,                      -- Идентификатор продажи ? NULL
    ReservedID INT NULL,                       -- Идентификатор залога, может быть ? NULL
    SoldID INT NULL,                           -- Идентификатор проданной вещи ? NULL
    Status NVARCHAR(20) CHECK (Status IN 
		('Available', 'Reserved', 'Sold')),      --  Статус вещи
);
GO


-- Вещи в залоге
ALTER TABLE Reserved
ADD CONSTRAINT FK_Reserved_Client 
FOREIGN KEY (ClientID) REFERENCES Client(ClientID)
ON DELETE CASCADE;
GO

-- Проданные вещи
ALTER TABLE Sold 
ADD CONSTRAINT FK_Sold_Client 
FOREIGN KEY (ClientID) REFERENCES Client(ClientID)
ON DELETE CASCADE;
GO

-- Вещи
ALTER TABLE Item 
ADD CONSTRAINT FK_Item_ItemType 
FOREIGN KEY (ItemTypeID) REFERENCES ItemType(ItemTypeID)
ON DELETE CASCADE;
GO

ALTER TABLE Item
ADD CONSTRAINT FK_Item_Available
FOREIGN KEY (AvailableID) REFERENCES Available(AvailableID)
ON DELETE  SET NULL;
GO

ALTER TABLE Item
ADD CONSTRAINT FK_Item_Reserved
FOREIGN KEY (ReservedID) REFERENCES Reserved(ReservedID)
ON DELETE  SET NULL;
GO

ALTER TABLE Item
ADD CONSTRAINT FK_Item_Sold
FOREIGN KEY (SoldID) REFERENCES Sold(SoldID)
ON DELETE SET NULL;
GO

/*
ALTER TABLE Item
ADD ItemId INT IDENTITY(1,1) PRIMARY KEY;
*/