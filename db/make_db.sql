-- CREATE DATABASE cursach;
-- DROP DATABASE cursach;
GO
USE cursach;
GO

-- Тип
CREATE TABLE ItemType (
    ItemTypeID INT PRIMARY KEY IDENTITY(1,1),
    NameType NVARCHAR(50) NOT NULL             -- Название типа вещи
);
GO

-- Клиент
CREATE TABLE Client (
    ClientID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,          -- Имя
    LastName NVARCHAR(50) NOT NULL,           -- Фамилия
    MiddleName NVARCHAR(50) NOT NULL,         -- Отчество
    PassportData NVARCHAR(50) NOT NULL        -- Паспортные данные
);
GO

-- Вещи, выставленных на продажу
CREATE TABLE Available (
    AvailableID INT PRIMARY KEY IDENTITY(1,1),
    DateListed DATE NOT NULL                   -- Дата
);
GO

-- Вещи в залоге
CREATE TABLE Reserved (
    ReservedID INT PRIMARY KEY IDENTITY(1,1),
    ClientID INT NOT NULL,                     -- Идентификатор клиента
    ReservedDate DATE NOT NULL,                -- Когда был сделан залог
    ExpirationDate DATE NOT NULL,              -- Когда заканчивается срок залога
    InterestRate DECIMAL(5, 2) NOT NULL,       -- Процентная ставка
);
GO

-- Проданные вещи
CREATE TABLE Sold (
    SoldID INT PRIMARY KEY IDENTITY(1,1),
    ClientID INT NOT NULL,                      -- Идентификатор клиента
    SaleDate DATE NOT NULL,                     -- Когда вещь была продана
);
GO

-- Вещи
CREATE TABLE Item (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(100) NOT NULL,           -- Имя
    ItemTypeID INT NOT NULL,                   -- Идентификатор типа
    Price DECIMAL(10, 2) NOT NULL,             -- Цена вещи
    AvailableID INT NULL,                      -- Идентификатор продажи ? NULL
    ReservedID INT NULL,                       -- Идентификатор залога, может быть ? NULL
    SoldID INT NULL,                           -- Идентификатор проданной вещи ? NULL
    Status NVARCHAR(20) CHECK (Status IN 
		('Available', 'Sold', 'Reserved')),      --  Статус вещи
);
GO


-- Вещи в залоге
ALTER TABLE Reserved
ADD CONSTRAINT FK_Reserved_Client 
FOREIGN KEY (ClientID) REFERENCES Client(ClientID);
GO

-- Проданные вещи
ALTER TABLE Sold 
ADD CONSTRAINT FK_Sold_Client 
FOREIGN KEY (ClientID) REFERENCES Client(ClientID);
GO

-- Вещи
ALTER TABLE Item 
ADD CONSTRAINT FK_Item_ItemType 
FOREIGN KEY (ItemTypeID) REFERENCES ItemType(ItemTypeID);
GO

ALTER TABLE Item
ADD CONSTRAINT FK_Item_Available
FOREIGN KEY (AvailableID) REFERENCES Available(AvailableID);
GO

ALTER TABLE Item
ADD CONSTRAINT FK_Item_Reserved
FOREIGN KEY (ReservedID) REFERENCES Reserved(ReservedID);
GO

ALTER TABLE Item
ADD CONSTRAINT FK_Item_Sold
FOREIGN KEY (SoldID) REFERENCES Sold(SoldID);
GO

/*
ALTER TABLE Item
ADD ItemId INT IDENTITY(1,1) PRIMARY KEY;
*/