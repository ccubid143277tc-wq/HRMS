-- =============================================
-- Room Related Stored Procedures
-- =============================================

-- 1. Add Room Procedure
DELIMITER //
CREATE PROCEDURE prcAddRoom(
    IN p_RoomNumber VARCHAR(20),
    IN p_RoomTypeID INT,
    IN p_BedConfiguration VARCHAR(50),
    IN p_MaximumOccupancy INT,
    IN p_RoomFloor INT,
    IN p_RoomStatusID INT,
    IN p_ViewType VARCHAR(50),
    IN p_RoomRate DECIMAL(10,2)
)
BEGIN
    INSERT INTO Rooms (
        RoomNumber, RoomTypeID, BedConfiguration, MaximumOccupancy, 
        RoomFloor, RoomStatusID, ViewType, RoomRate
    ) VALUES (
        p_RoomNumber, p_RoomTypeID, p_BedConfiguration, p_MaximumOccupancy, 
        p_RoomFloor, p_RoomStatusID, p_ViewType, p_RoomRate
    );
    
    SELECT LAST_INSERT_ID() AS RoomID;
END //
DELIMITER ;

-- 2. Update Room Procedure
DELIMITER //
CREATE PROCEDURE prcUpdateRoom(
    IN p_RoomID INT,
    IN p_RoomNumber VARCHAR(20),
    IN p_RoomTypeID INT,
    IN p_BedConfiguration VARCHAR(50),
    IN p_MaximumOccupancy INT,
    IN p_RoomFloor INT,
    IN p_RoomStatusID INT,
    IN p_ViewType VARCHAR(50),
    IN p_RoomRate DECIMAL(10,2)
)
BEGIN
    UPDATE Rooms 
    SET RoomNumber = p_RoomNumber,
        RoomTypeID = p_RoomTypeID,
        BedConfiguration = p_BedConfiguration,
        MaximumOccupancy = p_MaximumOccupancy,
        RoomFloor = p_RoomFloor,
        RoomStatusID = p_RoomStatusID,
        ViewType = p_ViewType,
        RoomRate = p_RoomRate
    WHERE RoomID = p_RoomID;
END //
DELIMITER ;

-- 3. Delete Room Procedure (with safety checks)
DELIMITER //
CREATE PROCEDURE prcDeleteRoom(
    IN p_RoomID INT
)
BEGIN
    DECLARE reservation_count INT DEFAULT 0;
    
    -- Check if room has active reservations
    SELECT COUNT(*) INTO reservation_count 
    FROM reservations 
    WHERE RoomID = p_RoomID 
    AND (Status = 'Confirmed' OR Status = 'Checked-In');
    
    -- If active reservations exist, return error
    IF reservation_count > 0 THEN
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Cannot delete room: active reservations exist';
    ELSE
        -- Delete from junction table first
        DELETE FROM ReservationRooms WHERE RoomID = p_RoomID;
        
        -- Delete main room record
        DELETE FROM Rooms WHERE RoomID = p_RoomID;
    END IF;
END //
DELIMITER ;

-- 4. Get All Rooms Procedure
DELIMITER //
CREATE PROCEDURE prcGetAllRooms()
BEGIN
    SELECT 
        r.RoomID,
        r.RoomNumber,
        r.RoomTypeID,
        r.BedConfiguration,
        r.MaximumOccupancy,
        r.RoomFloor,
        r.RoomStatusID,
        r.ViewType,
        r.RoomRate,
        rt.RoomType,
        rs.RoomStatus
    FROM Rooms r
    LEFT JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
    LEFT JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
    ORDER BY r.RoomNumber;
END //
DELIMITER ;

-- 5. Get Room By ID Procedure
DELIMITER //
CREATE PROCEDURE prcGetRoomById(
    IN p_RoomID INT
)
BEGIN
    SELECT 
        r.RoomID,
        r.RoomNumber,
        r.RoomTypeID,
        r.BedConfiguration,
        r.MaximumOccupancy,
        r.RoomFloor,
        r.RoomStatusID,
        r.ViewType,
        r.RoomRate,
        rt.RoomType,
        rs.RoomStatus
    FROM Rooms r
    LEFT JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
    LEFT JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
    WHERE r.RoomID = p_RoomID;
END //
DELIMITER ;

-- 6. Get Room Types Procedure
DELIMITER //
CREATE PROCEDURE prcGetRoomTypes()
BEGIN
    SELECT 
        RoomTypeID,
        RoomType,
        Description,
        BaseRate
    FROM RoomType
    ORDER BY RoomType;
END //
DELIMITER ;

-- 7. Get Room Statuses Procedure
DELIMITER //
CREATE PROCEDURE prcGetRoomStatuses()
BEGIN
    SELECT 
        RoomStatusID,
        RoomStatus,
        Description
    FROM RoomStatus
    ORDER BY RoomStatus;
END //
DELIMITER ;

-- 8. Search Rooms Procedure
DELIMITER //
CREATE PROCEDURE prcSearchRooms(
    IN p_SearchTerm VARCHAR(200),
    IN p_RoomTypeID INT,
    IN p_RoomStatusID INT
)
BEGIN
    SELECT 
        r.RoomID,
        r.RoomNumber,
        r.RoomTypeID,
        r.BedConfiguration,
        r.MaximumOccupancy,
        r.RoomFloor,
        r.RoomStatusID,
        r.ViewType,
        r.RoomRate,
        rt.RoomType,
        rs.RoomStatus,
        GROUP_CONCAT(a.Amenities SEPARATOR ', ') AS Amenities
    FROM Rooms r
    LEFT JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
    LEFT JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
    LEFT JOIN RoomAmenities ra ON r.RoomID = ra.RoomID
    LEFT JOIN Amenities a ON ra.AmenitiesID = a.AmenitiesID
    WHERE 
        (r.RoomNumber LIKE CONCAT('%', p_SearchTerm, '%') OR
         r.ViewType LIKE CONCAT('%', p_SearchTerm, '%') OR
         r.BedConfiguration LIKE CONCAT('%', p_SearchTerm, '%') OR
         rt.RoomType LIKE CONCAT('%', p_SearchTerm, '%') OR
         rs.RoomStatus LIKE CONCAT('%', p_SearchTerm, '%') OR
         a.Amenities LIKE CONCAT('%', p_SearchTerm, '%'))
        AND (p_RoomTypeID IS NULL OR p_RoomTypeID = 0 OR r.RoomTypeID = p_RoomTypeID)
        AND (p_RoomStatusID IS NULL OR p_RoomStatusID = 0 OR r.RoomStatusID = p_RoomStatusID)
    GROUP BY r.RoomID
    ORDER BY r.RoomNumber;
END //
DELIMITER ;

-- 9. Get Available Rooms By Date Range Procedure
DELIMITER //
CREATE PROCEDURE prcGetAvailableRoomsByDateRange(
    IN p_CheckInDate DATETIME,
    IN p_CheckOutDate DATETIME,
    IN p_RoomTypeID INT
)
BEGIN
    SELECT 
        r.RoomID,
        r.RoomNumber,
        r.RoomTypeID,
        r.BedConfiguration,
        r.MaximumOccupancy,
        r.RoomFloor,
        r.ViewType,
        r.RoomRate,
        rt.RoomType,
        rs.RoomStatus
    FROM Rooms r
    LEFT JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
    LEFT JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
    WHERE r.RoomStatusID = (SELECT RoomStatusID FROM RoomStatus WHERE RoomStatus = 'Available')
    AND (p_RoomTypeID IS NULL OR p_RoomTypeID = 0 OR r.RoomTypeID = p_RoomTypeID)
    AND r.RoomID NOT IN (
        SELECT DISTINCT RoomID 
        FROM reservations 
        WHERE (Check_InDate <= p_CheckOutDate AND Check_OutDate >= p_CheckInDate)
        AND Status IN ('Confirmed', 'Checked-In')
        UNION
        SELECT DISTINCT rr.RoomID 
        FROM ReservationRooms rr
        JOIN reservations r ON rr.ReservationID = r.ReservationID
        WHERE (r.Check_InDate <= p_CheckOutDate AND r.Check_OutDate >= p_CheckInDate)
        AND r.Status IN ('Confirmed', 'Checked-In')
    )
    ORDER BY r.RoomNumber;
END //
DELIMITER ;

-- 10. Filter Rooms Procedure
DELIMITER //
CREATE PROCEDURE prcFilterRooms(
    IN p_RoomTypeID INT,
    IN p_RoomStatusID INT,
    IN p_MinRate DECIMAL(10,2),
    IN p_MaxRate DECIMAL(10,2)
)
BEGIN
    SELECT 
        r.RoomID,
        r.RoomNumber,
        r.RoomTypeID,
        r.BedConfiguration,
        r.MaximumOccupancy,
        r.RoomFloor,
        r.RoomStatusID,
        r.ViewType,
        r.RoomRate,
        rt.RoomType,
        rs.RoomStatus,
        GROUP_CONCAT(a.Amenities SEPARATOR ', ') AS Amenities
    FROM Rooms r
    LEFT JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
    LEFT JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
    LEFT JOIN RoomAmenities ra ON r.RoomID = ra.RoomID
    LEFT JOIN Amenities a ON ra.AmenitiesID = a.AmenitiesID
    WHERE 
        (p_RoomTypeID IS NULL OR p_RoomTypeID = 0 OR r.RoomTypeID = p_RoomTypeID)
        AND (p_RoomStatusID IS NULL OR p_RoomStatusID = 0 OR r.RoomStatusID = p_RoomStatusID)
        AND (p_MinRate IS NULL OR r.RoomRate >= p_MinRate)
        AND (p_MaxRate IS NULL OR r.RoomRate <= p_MaxRate)
    GROUP BY r.RoomID
    ORDER BY r.RoomNumber;
END //
DELIMITER ;

-- 11. Get All Rooms With Details Procedure
DELIMITER //
CREATE PROCEDURE prcGetAllRoomsDetails()
BEGIN
    SELECT 
        r.RoomID,
        r.RoomNumber,
        r.RoomTypeID,
        r.BedConfiguration,
        r.MaximumOccupancy,
        r.RoomFloor,
        r.RoomStatusID,
        r.ViewType,
        r.RoomRate,
        rt.RoomType,
        rs.RoomStatus,
        GROUP_CONCAT(a.Amenities SEPARATOR ', ') AS Amenities
    FROM Rooms r
    LEFT JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
    LEFT JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
    LEFT JOIN RoomAmenities ra ON r.RoomID = ra.RoomID
    LEFT JOIN Amenities a ON ra.AmenitiesID = a.AmenitiesID
    GROUP BY r.RoomID
    ORDER BY r.RoomNumber;
END //
DELIMITER ;

-- 12. Get Room Grid Data Procedure (for DataGridView)
DELIMITER //
CREATE PROCEDURE prcGetRoomGridData()
BEGIN
    SELECT 
        r.RoomID,  
        r.RoomNumber,
        rt.RoomType,
        r.BedConfiguration,
        r.MaximumOccupancy,
        r.RoomFloor,
        rs.RoomStatus,
        r.ViewType,
        r.RoomRate,
        GROUP_CONCAT(a.Amenities SEPARATOR ', ') AS Amenities
    FROM Rooms r
    INNER JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
    INNER JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
    LEFT JOIN RoomAmenities ra ON r.RoomID = ra.RoomID
    LEFT JOIN Amenities a ON ra.AmenitiesID = a.AmenitiesID
    GROUP BY r.RoomID
    ORDER BY r.RoomNumber;
END //
DELIMITER ;

-- 13. Get Available Rooms By Type Procedure
DELIMITER //
CREATE PROCEDURE prcGetAvailableRoomsByType(
    IN p_RoomTypeName VARCHAR(100)
)
BEGIN
    SELECT 
        r.RoomID,
        r.RoomNumber,
        r.RoomTypeID,
        r.BedConfiguration,
        r.MaximumOccupancy,
        r.RoomFloor,
        r.RoomStatusID,
        r.ViewType,
        r.RoomRate,
        rt.RoomType,
        rs.RoomStatus
    FROM Rooms r
    INNER JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
    INNER JOIN RoomStatus rs ON r.RoomStatusID = rs.RoomStatusID
    WHERE rt.RoomType = p_RoomTypeName 
    AND rs.RoomStatus = 'Available'
    ORDER BY r.RoomNumber;
END //
DELIMITER ;

-- 14. Get Occupied Room Count By Date Procedure
DELIMITER //
CREATE PROCEDURE prcGetOccupiedRoomCountByDate(
    IN p_Date DATE
)
BEGIN
    SELECT COUNT(DISTINCT roomId) AS OccupiedCount
    FROM (
        SELECT rr.RoomID AS roomId
        FROM reservations r
        INNER JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
        WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
          AND r.Check_InDate <= p_Date
          AND r.Check_OutDate > p_Date
        UNION
        SELECT r.RoomID AS roomId
        FROM reservations r
        WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
          AND r.Check_InDate <= p_Date
          AND r.Check_OutDate > p_Date
    ) x;
END //
DELIMITER ;

-- 15. Get Check-In Count By Date Procedure
DELIMITER //
CREATE PROCEDURE prcGetCheckInCountByDate(
    IN p_Date DATE
)
BEGIN
    SELECT COUNT(DISTINCT r.ReservationID) AS CheckInCount
    FROM reservations r
    WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
      AND DATE(r.Check_InDate) = p_Date;
END //
DELIMITER ;

-- 16. Get Check-Out Count By Date Procedure
DELIMITER //
CREATE PROCEDURE prcGetCheckOutCountByDate(
    IN p_Date DATE
)
BEGIN
    SELECT COUNT(DISTINCT r.ReservationID) AS CheckOutCount
    FROM reservations r
    WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
      AND DATE(r.Check_OutDate) = p_Date;
END //
DELIMITER ;

-- 17. Check Room Availability Procedure
DELIMITER //
CREATE PROCEDURE prcIsRoomAvailable(
    IN p_RoomID INT,
    IN p_CheckInDate DATETIME,
    IN p_CheckOutDate DATETIME
)
BEGIN
    SELECT COUNT(*) AS ConflictCount
    FROM reservations 
    WHERE RoomID = p_RoomID 
    AND ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
    AND ((Check_InDate <= p_CheckInDate AND Check_OutDate > p_CheckInDate) 
         OR (Check_InDate < p_CheckOutDate AND Check_OutDate >= p_CheckOutDate)
         OR (Check_InDate >= p_CheckInDate AND Check_OutDate <= p_CheckOutDate));
END //
DELIMITER ;

-- 10. Update Room Status Procedure
DELIMITER //
CREATE PROCEDURE prcUpdateRoomStatus(
    IN p_RoomID INT,
    IN p_StatusName VARCHAR(50)
)
BEGIN
    UPDATE Rooms 
    SET RoomStatusID = (SELECT RoomStatusID FROM RoomStatus WHERE RoomStatus = p_StatusName)
    WHERE RoomID = p_RoomID;
END //
DELIMITER ;
