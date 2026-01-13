DELIMITER //

CREATE PROCEDURE prcGetReceiptReservationDetails(
    IN p_ReservationID INT
)
BEGIN
    SELECT
        r.ReservationID,
        r.BookingReferences,
        r.Check_InDate,
        r.Check_OutDate,
        COALESCE(r.numberOfNights, DATEDIFF(r.Check_OutDate, r.Check_InDate)) AS numberOfNights,
        COALESCE(r.NumAdult, 0) AS NumAdult,
        COALESCE(r.NumChildren, 0) AS NumChildren,
        CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
        COALESCE(g.Address, '') AS Address,
        COALESCE(g.PhoneNumber, '') AS PhoneNumber,
        GROUP_CONCAT(DISTINCT rm.RoomNumber ORDER BY rm.RoomNumber SEPARATOR ', ') AS RoomNumbers,
        COALESCE(rt.RoomType, '') AS RoomType,
        COALESCE(SUM(DISTINCT rm.RoomRate), 0) AS NightlyRoomRateSum
    FROM reservations r
    LEFT JOIN Guest g ON r.GuestID = g.GuestID
    LEFT JOIN ReservationRooms rr ON r.ReservationID = rr.ReservationID
    LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r.RoomID)
    LEFT JOIN RoomType rt ON rm.RoomTypeID = rt.RoomTypeID
    WHERE r.ReservationID = p_ReservationID
    GROUP BY r.ReservationID, r.BookingReferences, r.Check_InDate, r.Check_OutDate, r.numberOfNights,
             r.NumAdult, r.NumChildren, g.FirstName, g.LastName, g.Address, g.PhoneNumber, rt.RoomType;
END //

CREATE PROCEDURE prcGetReceiptTotals(
    IN p_ReservationID INT
)
BEGIN
    SELECT
        ROUND(COALESCE(rtcalc.TotalDue, 0) + COALESCE(svc.ServiceCharges, 0), 2) AS TotalDue,
        ROUND(COALESCE(pd.TotalPaid, 0), 2) AS TotalPaid,
        ROUND((COALESCE(rtcalc.TotalDue, 0) + COALESCE(svc.ServiceCharges, 0)) - COALESCE(pd.TotalPaid, 0), 2) AS Balance,
        ROUND(COALESCE(svc.ServiceCharges, 0), 2) AS ServiceCharges
    FROM reservations r
    LEFT JOIN (
        SELECT
            r2.ReservationID,
            ((COALESCE(SUM(DISTINCT rm.RoomRate), 0) * COALESCE(r2.numberOfNights, 0)) * 1.05) AS TotalDue
        FROM reservations r2
        LEFT JOIN ReservationRooms rr ON r2.ReservationID = rr.ReservationID
        LEFT JOIN Rooms rm ON rm.RoomID = COALESCE(rr.RoomID, r2.RoomID)
        GROUP BY r2.ReservationID, r2.numberOfNights
    ) rtcalc ON rtcalc.ReservationID = r.ReservationID
    LEFT JOIN (
        SELECT
            ReservationID,
            COALESCE(SUM(amount), 0) AS TotalPaid
        FROM payment
        WHERE (Payment_Status IS NULL OR Payment_Status <> 'Voided')
          AND (Payment_method IS NULL OR Payment_method <> 'Additional Service')
          AND (Payment_Status IS NULL OR Payment_Status <> 'Charge')
        GROUP BY ReservationID
    ) pd ON pd.ReservationID = r.ReservationID
    LEFT JOIN (
        SELECT
            ReservationID,
            COALESCE(SUM(amount), 0) AS ServiceCharges
        FROM payment
        WHERE (Payment_Status IS NULL OR Payment_Status <> 'Voided')
          AND (Payment_method = 'Additional Service' OR Payment_Status = 'Charge')
        GROUP BY ReservationID
    ) svc ON svc.ReservationID = r.ReservationID
    WHERE r.ReservationID = p_ReservationID;
END //

CREATE PROCEDURE prcGetReceiptDate(
    IN p_ReservationID INT
)
BEGIN
    SELECT MAX(Payment_Date) AS ReceiptDate
    FROM payment
    WHERE ReservationID = p_ReservationID
      AND (Payment_Status IS NULL OR Payment_Status <> 'Voided');
END //

CREATE PROCEDURE prcGetLatestPaymentForReservation(
    IN p_ReservationID INT
)
BEGIN
    SELECT p.PaymentID, p.Payment_Date
    FROM payment p
    WHERE p.ReservationID = p_ReservationID
      AND (p.Payment_Status IS NULL OR p.Payment_Status <> 'Voided')
      AND (p.Payment_method IS NULL OR p.Payment_method <> 'Additional Service')
      AND (p.Payment_Status IS NULL OR p.Payment_Status <> 'Charge')
    ORDER BY p.Payment_Date DESC, p.PaymentID DESC
    LIMIT 1;
END //

DELIMITER ;
