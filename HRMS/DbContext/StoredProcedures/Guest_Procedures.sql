-- Guest Stored Procedures
-- Add Guest
DELIMITER //
CREATE PROCEDURE prcAddGuest(
    IN p_FirstName VARCHAR(100),
    IN p_LastName VARCHAR(100),
    IN p_Email VARCHAR(150),
    IN p_PhoneNumber VARCHAR(50),
    IN p_BirthDate DATE,
    IN p_Address TEXT,
    IN p_Nationality VARCHAR(50),
    IN p_IDType VARCHAR(50),
    IN p_IDNumber VARCHAR(100),
    IN p_Classification VARCHAR(50)
)
BEGIN
    INSERT INTO Guest (
        FirstName, LastName, Email, PhoneNumber, BirthDate, Address,
        Nationality, IDTYPE, IDNumber, Classification
    )
    VALUES (
        p_FirstName, p_LastName, p_Email, p_PhoneNumber, p_BirthDate, p_Address,
        p_Nationality, p_IDType, p_IDNumber, p_Classification
    );
    
    SELECT LAST_INSERT_ID() AS GuestID;
END //
DELIMITER ;

-- Update Guest
DELIMITER //
CREATE PROCEDURE prcUpdateGuest(
    IN p_GuestID INT,
    IN p_FirstName VARCHAR(100),
    IN p_LastName VARCHAR(100),
    IN p_Email VARCHAR(150),
    IN p_PhoneNumber VARCHAR(50),
    IN p_BirthDate DATE,
    IN p_Address TEXT,
    IN p_Nationality VARCHAR(50),
    IN p_IDType VARCHAR(50),
    IN p_IDNumber VARCHAR(100),
    IN p_Classification VARCHAR(50)
)
BEGIN
    UPDATE Guest SET
        FirstName = p_FirstName,
        LastName = p_LastName,
        Email = p_Email,
        PhoneNumber = p_PhoneNumber,
        BirthDate = p_BirthDate,
        Address = p_Address,
        Nationality = p_Nationality,
        IDTYPE = p_IDType,
        IDNumber = p_IDNumber,
        Classification = p_Classification
    WHERE GuestID = p_GuestID;
END //
DELIMITER ;

-- Delete Guest
DELIMITER //
CREATE PROCEDURE prcDeleteGuest(
    IN p_GuestID INT
)
BEGIN
    DELETE FROM Guest WHERE GuestID = p_GuestID;
END //
DELIMITER ;

-- Get Guest By ID
DELIMITER //
CREATE PROCEDURE prcGetGuestById(
    IN p_GuestID INT
)
BEGIN
    SELECT 
        GuestID,
        FirstName,
        LastName,
        Email,
        PhoneNumber,
        BirthDate,
        Address,
        Nationality,
        IDTYPE,
        IDNumber,
        Classification
    FROM Guest
    WHERE GuestID = p_GuestID;
END //
DELIMITER ;

-- Get All Guests
DELIMITER //
CREATE PROCEDURE prcGetAllGuests()
BEGIN
    SELECT 
        GuestID,
        FirstName,
        LastName,
        Email,
        PhoneNumber,
        BirthDate,
        Address,
        Nationality,
        IDTYPE,
        IDNumber,
        Classification
    FROM Guest
    ORDER BY LastName, FirstName;
END //
DELIMITER ;

-- Search Guests
DELIMITER //
CREATE PROCEDURE prcSearchGuests(
    IN p_SearchTerm VARCHAR(255)
)
BEGIN
    SELECT 
        GuestID,
        FirstName,
        LastName,
        Email,
        PhoneNumber,
        BirthDate,
        Address,
        Nationality,
        IDTYPE,
        IDNumber,
        Classification
    FROM Guest
    WHERE 
        FirstName LIKE CONCAT('%', p_SearchTerm, '%') OR
        LastName LIKE CONCAT('%', p_SearchTerm, '%') OR
        Email LIKE CONCAT('%', p_SearchTerm, '%') OR
        PhoneNumber LIKE CONCAT('%', p_SearchTerm, '%') OR
        IDNumber LIKE CONCAT('%', p_SearchTerm, '%')
    ORDER BY LastName, FirstName;
END //
DELIMITER ;

-- Get Guest Grid Data (for DataGridView display)
DELIMITER //
CREATE PROCEDURE prcGetGuestGridData()
BEGIN
    SELECT 
        GuestID,
        CONCAT(FirstName, ' ', LastName) AS GuestName,
        Email,
        PhoneNumber,
        IDTYPE,
        Classification
    FROM Guest
    ORDER BY LastName, FirstName;
END //
DELIMITER ;
