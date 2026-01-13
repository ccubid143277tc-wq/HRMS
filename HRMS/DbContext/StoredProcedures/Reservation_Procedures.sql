DELIMITER //

CREATE PROCEDURE prcAddReservation(
    IN p_GuestID INT,
    IN p_Check_InDate DATETIME,
    IN p_Check_OutDate DATETIME,
    IN p_NumAdult INT,
    IN p_NumChildren INT,
    IN p_SpecialRequest VARCHAR(500),
    IN p_ReservationStatus VARCHAR(50),
    IN p_ReservationType VARCHAR(50),
    IN p_RoomID INT,
    IN p_numberOfNights INT,
    IN p_BookingReferences VARCHAR(50)
)
BEGIN
    INSERT INTO reservations (
        GuestID,
        Check_InDate,
        Check_OutDate,
        NumAdult,
        NumChildren,
        SpecialRequest,
        ReservationStatus,
        ReservationType,
        RoomID,
        numberOfNights,
        BookingReferences
    )
    VALUES (
        p_GuestID,
        p_Check_InDate,
        p_Check_OutDate,
        p_NumAdult,
        p_NumChildren,
        COALESCE(p_SpecialRequest, ''),
        COALESCE(p_ReservationStatus, 'Confirmed'),
        COALESCE(p_ReservationType, ''),
        p_RoomID,
        p_numberOfNights,
        COALESCE(p_BookingReferences, '')
    );

    SELECT LAST_INSERT_ID() AS ReservationID;
END //

CREATE PROCEDURE prcUpdateReservation(
    IN p_ReservationID INT,
    IN p_GuestID INT,
    IN p_Check_InDate DATETIME,
    IN p_Check_OutDate DATETIME,
    IN p_NumAdult INT,
    IN p_NumChildren INT,
    IN p_SpecialRequest VARCHAR(500),
    IN p_ReservationStatus VARCHAR(50),
    IN p_ReservationType VARCHAR(50),
    IN p_RoomID INT,
    IN p_numberOfNights INT,
    IN p_BookingReferences VARCHAR(50)
)
BEGIN
    UPDATE reservations
    SET
        GuestID = p_GuestID,
        Check_InDate = p_Check_InDate,
        Check_OutDate = p_Check_OutDate,
        NumAdult = p_NumAdult,
        NumChildren = p_NumChildren,
        SpecialRequest = COALESCE(p_SpecialRequest, ''),
        ReservationStatus = COALESCE(p_ReservationStatus, 'Confirmed'),
        ReservationType = COALESCE(p_ReservationType, ''),
        RoomID = p_RoomID,
        numberOfNights = p_numberOfNights,
        BookingReferences = COALESCE(p_BookingReferences, '')
    WHERE ReservationID = p_ReservationID;
END //

CREATE PROCEDURE prcCancelReservation(
    IN p_ReservationID INT
)
BEGIN
    UPDATE reservations
    SET ReservationStatus = 'Cancelled'
    WHERE ReservationID = p_ReservationID;
END //

CREATE PROCEDURE prcAddReservationRoom(
    IN p_ReservationID INT,
    IN p_RoomID INT
)
BEGIN
    INSERT INTO ReservationRooms (ReservationID, RoomID)
    VALUES (p_ReservationID, p_RoomID);
END //

CREATE PROCEDURE prcGetRoomIdsByReservation(
    IN p_ReservationID INT
)
BEGIN
    SELECT RoomID
    FROM ReservationRooms
    WHERE ReservationID = p_ReservationID;
END //

CREATE PROCEDURE prcDeleteReservation(
    IN p_ReservationID INT
)
BEGIN
    DECLARE payment_exists INT DEFAULT 0;

    SELECT COUNT(*) INTO payment_exists
    FROM payment
    WHERE ReservationID = p_ReservationID
    LIMIT 1;

    IF payment_exists > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Cannot delete this reservation because it has payment records. Please delete/void the payments first or cancel the reservation instead.';
    END IF;

    UPDATE Rooms rm
    INNER JOIN ReservationRooms rr ON rm.RoomID = rr.RoomID
    SET rm.RoomStatusID = (SELECT RoomStatusID FROM RoomStatus WHERE RoomStatus = 'Available' LIMIT 1)
    WHERE rr.ReservationID = p_ReservationID;

    UPDATE Rooms
    SET RoomStatusID = (SELECT RoomStatusID FROM RoomStatus WHERE RoomStatus = 'Available' LIMIT 1)
    WHERE RoomID = (SELECT RoomID FROM reservations WHERE ReservationID = p_ReservationID LIMIT 1)
      AND (SELECT COUNT(*) FROM ReservationRooms WHERE ReservationID = p_ReservationID) = 0;

    DELETE FROM ReservationRooms WHERE ReservationID = p_ReservationID;
    DELETE FROM reservations WHERE ReservationID = p_ReservationID;
END //

CREATE PROCEDURE prcSearchReservations(
    IN p_Keyword VARCHAR(255)
)
BEGIN
    SELECT r.*, g.FirstName, g.LastName, rm.RoomNumber
    FROM reservations r
    LEFT JOIN Guest g ON r.GuestID = g.GuestID
    LEFT JOIN Rooms rm ON r.RoomID = rm.RoomID
    WHERE g.FirstName LIKE CONCAT('%', p_Keyword, '%')
       OR g.LastName LIKE CONCAT('%', p_Keyword, '%')
       OR g.Email LIKE CONCAT('%', p_Keyword, '%')
       OR g.PhoneNumber LIKE CONCAT('%', p_Keyword, '%')
       OR rm.RoomNumber LIKE CONCAT('%', p_Keyword, '%')
       OR r.ReservationStatus LIKE CONCAT('%', p_Keyword, '%')
    ORDER BY r.Check_InDate DESC;
END //

CREATE PROCEDURE prcGetReservationById(
    IN p_ReservationID INT
)
BEGIN
    SELECT r.*, g.FirstName, g.LastName, g.Email, g.PhoneNumber, rm.RoomNumber, rm.RoomRate
    FROM reservations r
    LEFT JOIN Guest g ON r.GuestID = g.GuestID
    LEFT JOIN Rooms rm ON r.RoomID = rm.RoomID
    WHERE r.ReservationID = p_ReservationID;
END //

CREATE PROCEDURE prcGetAllReservations()
BEGIN
    SELECT r.*, g.FirstName, g.LastName, rm.RoomNumber
    FROM reservations r
    LEFT JOIN Guest g ON r.GuestID = g.GuestID
    LEFT JOIN Rooms rm ON r.RoomID = rm.RoomID
    ORDER BY r.Check_InDate DESC;
END //

CREATE PROCEDURE prcGetReservationGridData()
BEGIN
    SELECT r.ReservationID, r.GuestID, r.Check_InDate, r.Check_OutDate,
           r.NumAdult, r.NumChildren, r.SpecialRequest, r.ReservationStatus, r.RoomID,
           r.numberOfNights, r.BookingReferences, r.ReservationType,
           g.FirstName, g.LastName,
           GROUP_CONCAT(DISTINCT rm.RoomNumber ORDER BY rm.RoomNumber SEPARATOR ', ') AS RoomNumbers,
           rt.RoomType
    FROM reservations r
    LEFT JOIN Guest g ON r.GuestID = g.GuestID
    LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
    LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
    LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
    GROUP BY r.ReservationID, r.GuestID, r.Check_InDate, r.Check_OutDate,
             r.NumAdult, r.NumChildren, r.SpecialRequest, r.ReservationStatus, r.RoomID,
             r.numberOfNights, r.BookingReferences, r.ReservationType,
             g.FirstName, g.LastName, rt.RoomType
    ORDER BY r.Check_InDate DESC;
END //

CREATE PROCEDURE prcGetExpectedArrivalsGridData(
    IN p_Date DATE
)
BEGIN
    SELECT
        r.ReservationID,
        r.BookingReferences,
        CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
        GROUP_CONCAT(DISTINCT rm.RoomNumber ORDER BY rm.RoomNumber SEPARATOR ', ') AS RoomNumbers,
        rt.RoomType AS RoomType,
        r.Check_InDate,
        r.Check_OutDate,
        r.NumAdult,
        r.NumChildren,
        (COALESCE(r.NumAdult, 0) + COALESCE(r.NumChildren, 0)) AS Occupants,
        r.ReservationStatus
     FROM reservations r
     LEFT JOIN Guest g ON r.GuestID = g.GuestID
     LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
     LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
     LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
     WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
       AND DATE(r.Check_InDate) = p_Date
     GROUP BY r.ReservationID, r.BookingReferences, g.FirstName, g.LastName, rt.RoomType,
              r.Check_InDate, r.Check_OutDate, r.NumAdult, r.NumChildren, r.ReservationStatus
     ORDER BY r.Check_InDate, GuestName;
END //

CREATE PROCEDURE prcGetExpectedDeparturesGridData(
    IN p_Date DATE
)
BEGIN
    SELECT
        r.ReservationID,
        r.BookingReferences,
        CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
        GROUP_CONCAT(DISTINCT rm.RoomNumber ORDER BY rm.RoomNumber SEPARATOR ', ') AS RoomNumbers,
        rt.RoomType AS RoomType,
        r.Check_InDate,
        r.Check_OutDate,
        r.NumAdult,
        r.NumChildren,
        (COALESCE(r.NumAdult, 0) + COALESCE(r.NumChildren, 0)) AS Occupants,
        r.ReservationStatus
     FROM reservations r
     LEFT JOIN Guest g ON r.GuestID = g.GuestID
     LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
     LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
     LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
     WHERE r.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
       AND DATE(r.Check_OutDate) = p_Date
     GROUP BY r.ReservationID, r.BookingReferences, g.FirstName, g.LastName, rt.RoomType,
              r.Check_InDate, r.Check_OutDate, r.NumAdult, r.NumChildren, r.ReservationStatus
     ORDER BY r.Check_OutDate, GuestName;
END //

CREATE PROCEDURE prcGetReservationStatusCounts()
BEGIN
    SELECT
        COUNT(*) as Total,
        SUM(CASE WHEN ReservationStatus = 'Confirmed' THEN 1 ELSE 0 END) as Confirmed,
        SUM(CASE WHEN ReservationStatus = 'Checked-In' THEN 1 ELSE 0 END) as CheckedIn,
        SUM(CASE WHEN ReservationStatus = 'Checked-Out' THEN 1 ELSE 0 END) as CheckedOut,
        SUM(CASE WHEN ReservationStatus = 'Cancelled' THEN 1 ELSE 0 END) as Cancelled
    FROM reservations;
END //

CREATE PROCEDURE prcCheckRoomAvailability(
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

CREATE PROCEDURE prcGetAvailableRoomsByTypeIdDateRange(
    IN p_RoomTypeID INT,
    IN p_CheckInDate DATETIME,
    IN p_CheckOutDate DATETIME
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
    WHERE r.RoomTypeID = p_RoomTypeID
      AND rs.RoomStatus = 'Available'
      AND r.RoomID NOT IN (
          SELECT DISTINCT RoomID
          FROM reservations
          WHERE ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
            AND (Check_InDate <= p_CheckOutDate AND Check_OutDate >= p_CheckInDate)
          UNION
          SELECT DISTINCT rr.RoomID
          FROM ReservationRooms rr
          INNER JOIN reservations res ON rr.ReservationID = res.ReservationID
          WHERE res.ReservationStatus NOT IN ('Cancelled', 'Checked-Out')
            AND (res.Check_InDate <= p_CheckOutDate AND res.Check_OutDate >= p_CheckInDate)
      )
    ORDER BY r.RoomNumber;
END //

CREATE PROCEDURE prcGetAllRoomTypesForReservation()
BEGIN
    SELECT RoomTypeID, RoomType
    FROM RoomType
    ORDER BY RoomType;
END //

DELIMITER ;
