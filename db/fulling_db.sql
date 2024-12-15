USE cursach;
GO

SET NOCOUNT ON;

-- Объявление переменных
DECLARE @Symbol CHAR(52) = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz',
        @Position INT,
        @RowCount INT,
        @NameLimit INT,
        @FirstName NVARCHAR(50),
        @LastName NVARCHAR(50),
        @MiddleName NVARCHAR(50),
        @PassportData NVARCHAR(10),
        @NameType NVARCHAR(50),
        @DateListed DATE,
        @AvailableDate DATE,
        @ReservedDate DATE,
        @ExpirationDate DATE,
        @InterestRate DECIMAL(5, 2),
        @ItemName NVARCHAR(100),
        @Price DECIMAL(10, 2),
        @Status NVARCHAR(20),
        @ItemTypeID INT,
        @ClientID INT,
        @AvailableID INT,
        @ReservedID INT,
        @SoldID INT,
        @MaxItemTypeID INT,
        @MaxSoldID INT,
		@MaxAvailableID INT,
        @MaxReservedID INT,
        @MinSymbols INT = 5,
        @MaxSymbols INT = 30;


-- Начало транзакции
BEGIN TRAN;
-------------------------
-- Заполнение клиентов --
-------------------------
SET @RowCount = 1;

WHILE @RowCount <= 500
BEGIN
    -- Генерация случайных данных для клиента
    SET @FirstName = '';
    SET @LastName = '';
    SET @MiddleName = '';
    SET @PassportData = '';

    SET @NameLimit = @MinSymbols + RAND() * (@MaxSymbols - @MinSymbols);

    WHILE LEN(@FirstName) < @NameLimit
    BEGIN
        SET @Position = RAND() * 52 + 1;
        SET @FirstName += SUBSTRING(@Symbol, @Position, 1);
    END;

    WHILE LEN(@LastName) < @NameLimit
    BEGIN
        SET @Position = RAND() * 52 + 1;
        SET @LastName += SUBSTRING(@Symbol, @Position, 1);
    END;

    WHILE LEN(@MiddleName) < @NameLimit
    BEGIN
        SET @Position = RAND() * 52 + 1;
        SET @MiddleName += SUBSTRING(@Symbol, @Position, 1);
    END;

    WHILE LEN(@PassportData) < 10
    BEGIN
        SET @Position = RAND() * 52 + 1;
        SET @PassportData += SUBSTRING(@Symbol, @Position, 1);
    END;

    -- Вставка данных в таблицу Client
    INSERT INTO Client (FirstName, LastName, MiddleName, PassportData)
    VALUES (@FirstName, @LastName, @MiddleName, @PassportData);

    SET @RowCount += 1;
END;


----------------------------
-- Заполнение типов вещей --
----------------------------
SET @RowCount = 1;

WHILE @RowCount <= 10
BEGIN
    SET @NameType = '';
    SET @NameLimit = 5 + RAND() * 45;

    WHILE LEN(@NameType) < @NameLimit
    BEGIN
        SET @Position = RAND() * 52 + 1;
        SET @NameType += SUBSTRING(@Symbol, @Position, 1);
    END;

    INSERT INTO ItemType (NameType)
    VALUES (@NameType);

    SET @RowCount += 1;
END;


----------------------
-- Заполнение Available
----------------------
SET @RowCount = 1;

WHILE @RowCount <= 100
BEGIN
    -- Генерация случайной даты за последний год
    SET @DateListed = DATEADD(DAY, -RAND() * 365, GETDATE());

    -- Вставка данных в таблицу Available
    INSERT INTO Available (DateListed)
    VALUES (@DateListed);

    SET @RowCount += 1;
END;


----------------------
-- Заполнение Sold --
----------------------
SET @RowCount = 1;
WHILE @RowCount <= 200
BEGIN
    SET @AvailableDate = DATEADD(DAY, -RAND() * 365, GETDATE());
    SET @ClientID = CAST(1 + RAND() * 499 AS INT);

    INSERT INTO Sold (ClientID, SaleDate)
    VALUES (@ClientID, @AvailableDate);

    SET @RowCount += 1;
END;


----------------------
-- Заполнение Reserved --
----------------------
SET @RowCount = 1;
WHILE @RowCount <= 200
BEGIN
    SET @ReservedDate = DATEADD(DAY, -RAND() * 365, GETDATE());
    SET @ExpirationDate = DATEADD(DAY, RAND() * 365, @ReservedDate);
    SET @InterestRate = 5 + RAND() * 15;
    SET @ClientID = CAST(1 + RAND() * 499 AS INT);

    INSERT INTO Reserved (ClientID, ReservedDate, ExpirationDate, InterestRate)
    VALUES (@ClientID, @ReservedDate, @ExpirationDate, @InterestRate);

    SET @RowCount += 1;
END;


---------------------
-- Заполнение Item --
---------------------
-- Получение максимальных идентификаторов
SELECT @MaxItemTypeID = MAX(ItemTypeID) FROM ItemType;
SELECT @MaxAvailableID = MAX(AvailableID) FROM Available;
SELECT @MaxSoldID = MAX(SoldID) FROM Sold;
SELECT @MaxReservedID = MAX(ReservedID) FROM Reserved;

SET @RowCount = 1;

WHILE @RowCount <= 1000
BEGIN
    SET @ItemName = '';
    SET @NameLimit = 10 + RAND() * 90;
    SET @Price = 100 + RAND() * 10000;

    WHILE LEN(@ItemName) < @NameLimit
    BEGIN
        SET @Position = RAND() * 52 + 1;
        SET @ItemName += SUBSTRING(@Symbol, @Position, 1);
    END;

    SET @ItemTypeID = CAST(1 + RAND() * @MaxItemTypeID AS INT);

    SET @Status = CASE 
                    WHEN RAND() < 0.33 THEN 'Available'
                    WHEN RAND() < 0.66 THEN 'Sold'
                    ELSE 'Reserved'
                  END;

    IF @Status = 'Available'
    BEGIN
        SET @AvailableID = CAST(1 + RAND() * @MaxAvailableID AS INT);
        SET @ReservedID = NULL;
        SET @SoldID = NULL;
    END
    ELSE IF @Status = 'Sold'
    BEGIN
        SET @AvailableID = NULL;
        SET @ReservedID = NULL;
        SET @SoldID = CAST(1 + RAND() * @MaxSoldID AS INT);
    END
    ELSE
    BEGIN
        SET @AvailableID = NULL;
        SET @ReservedID = CAST(1 + RAND() * @MaxReservedID AS INT);
        SET @SoldID = NULL;
    END;

    INSERT INTO Item (ItemName, ItemTypeID, Price, AvailableID, ReservedID, SoldID, Status)
    VALUES (@ItemName, @ItemTypeID, @Price, @AvailableID, @ReservedID, @SoldID, @Status);

    SET @RowCount += 1;
END;

-- Завершение транзакции
COMMIT TRAN;
