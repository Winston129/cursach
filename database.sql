-- Создание базы данных для ломбарда
CREATE DATABASE LombardDB;
GO

-- Использование базы данных LombardDB
USE LombardDB;
GO

-- Таблица Клиенты
CREATE TABLE Clients (
    ClientID INT PRIMARY KEY IDENTITY(1,1),    -- Уникальный идентификатор клиента (PRIMARY KEY)
    FullName NVARCHAR(100) NOT NULL,           -- ФИО клиента
    PassportDetails NVARCHAR(50) NOT NULL      -- Паспортные данные клиента
);
GO

-- Таблица Типы вещей
CREATE TABLE ItemTypes (
    ItemTypeID INT PRIMARY KEY IDENTITY(1,1),  -- Уникальный идентификатор типа (PRIMARY KEY)
    ItemTypeName NVARCHAR(50) NOT NULL         -- Название типа вещи (например, драгоценный металл, украшение и т.д.)
);
GO

-- Добавление типов вещей
INSERT INTO ItemTypes (ItemTypeName) VALUES
('Драгоценный металл'),
('Украшение'),
('Бытовая техника'),
('Часы'),
('Картины');
GO

-- Таблица Статусы
CREATE TABLE ItemStatus (
    StatusID INT PRIMARY KEY IDENTITY(1,1),    -- Уникальный идентификатор статуса (PRIMARY KEY)
    StatusName NVARCHAR(50) NOT NULL           -- Название статуса (например, "в залоге", "на продаже")
);
GO

-- Добавление статусов вещей
INSERT INTO ItemStatus (StatusName) VALUES
('В залоге'),
('На продаже');
GO

-- Таблица Вещи
CREATE TABLE Items (
    ItemID INT PRIMARY KEY IDENTITY(1,1),      -- Уникальный идентификатор вещи (PRIMARY KEY)
    ItemName NVARCHAR(100) NOT NULL,           -- Название вещи
    ItemTypeID INT NOT NULL,                   -- Тип вещи (FOREIGN KEY на таблицу ItemTypes)
    StatusID INT NOT NULL,                     -- Статус вещи (FOREIGN KEY на таблицу ItemStatus),
    FOREIGN KEY (ItemTypeID) REFERENCES ItemTypes(ItemTypeID), -- Внешний ключ на типы вещей
    FOREIGN KEY (StatusID) REFERENCES ItemStatus(StatusID)     -- Внешний ключ на статусы вещей
);
GO

-- Таблица Заложенные вещи
CREATE TABLE PledgedItems (
    PledgeID INT PRIMARY KEY IDENTITY(1,1),    -- Уникальный идентификатор записи залога (PRIMARY KEY)
    ItemID INT NOT NULL,                       -- Вещь (FOREIGN KEY на таблицу Items)
    ClientID INT NOT NULL,                     -- Клиент (FOREIGN KEY на таблицу Clients)
    PledgeDate DATE NOT NULL,                  -- Дата залога
    EndDate DATE NOT NULL,                     -- Дата истечения кредита
    PledgeAmount DECIMAL(10, 2) NOT NULL,      -- Цена залога
    InterestRate DECIMAL(5, 2) NOT NULL,       -- Процентная ставка
    FOREIGN KEY (ItemID) REFERENCES Items(ItemID),  -- Внешний ключ на таблицу Items
    FOREIGN KEY (ClientID) REFERENCES Clients(ClientID) -- Внешний ключ на таблицу Clients
);
GO

-- Таблица Проданные вещи
CREATE TABLE SoldItems (
    SaleID INT PRIMARY KEY IDENTITY(1,1),      -- Уникальный идентификатор продажи (PRIMARY KEY)
    ItemID INT NOT NULL,                       -- Вещь (FOREIGN KEY на таблицу Items)
    BuyerID INT NOT NULL,                      -- Покупатель (клиент) (FOREIGN KEY на таблицу Clients)
    SaleDate DATE NOT NULL,                    -- Дата продажи
    FOREIGN KEY (ItemID) REFERENCES Items(ItemID),  -- Внешний ключ на таблицу Items
    FOREIGN KEY (BuyerID) REFERENCES Clients(ClientID) -- Внешний ключ на покупателя (клиент)
);
GO




-- \   kjhhhhhhhhhhhhhhhhhhhhhunhhcccccccccccccccccccccccccccccccccccccccccccccccccccccccccg,yr7gazsx3e4