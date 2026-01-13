-- Sample Data for Testing Receipts
-- Assumes tables: Guest, reservations, payment
-- Adjust IDs/RoomIDs to match your existing data

-- Guests (20 records)
INSERT INTO Guest (FirstName, LastName, Email, PhoneNumber, Address, IDNumber, Nationality, DateOfBirth, IDType, Classification) VALUES
('John', 'Doe', 'john.doe@example.com', '09123456789', '123 Main St, Manila', '1234567890', 'Filipino', '1990-05-15', 'Passport', 'Regular'),
('Jane', 'Smith', 'jane.smith@example.com', '09234567890', '456 Oak Ave, Quezon City', '2345678901', 'American', '1985-08-22', 'Driver''s License', 'VIP'),
('Carlos', 'Reyes', 'carlos.reyes@example.com', '09345678901', '789 Pine Rd, Cebu', '3456789012', 'Filipino', '1992-12-03', 'Passport', 'Regular'),
('Emily', 'Chen', 'emily.chen@example.com', '09456789012', '321 Maple Dr, Makati', '4567890123', 'Chinese', '1988-04-10', 'Passport', 'Regular'),
('Michael', 'Santos', 'michael.santos@example.com', '09567890123', '654 Birch Blvd, Davao', '5678901234', 'Filipino', '1995-09-30', 'Driver''s License', 'Regular'),
('Sarah', 'Lim', 'sarah.lim@example.com', '09678901234', '987 Cedar Ln, Pasay', '6789012345', 'Filipino', '1991-07-18', 'Passport', 'VIP'),
('David', 'Garcia', 'david.garcia@example.com', '09789012345', '147 Spruce St, Taguig', '7890123456', 'Filipino', '1987-11-25', 'Driver''s License', 'Regular'),
('Lisa', 'Wong', 'lisa.wong@example.com', '09890123456', '258 Elm Ct, Mandaluyong', '8901234567', 'Filipino', '1993-02-14', 'Passport', 'Regular'),
('Robert', 'Tan', 'robert.tan@example.com', '09901234567', '369 Willow Way, Paranaque', '9012345678', 'Filipino', '1986-06-07', 'Driver''s License', 'VIP'),
('Anna', 'Fernandez', 'anna.fernandez@example.com', '09012345678', '741 Ash Rd, Las Pinas', '0123456789', 'Filipino', '1994-10-20', 'Passport', 'Regular'),
('James', 'Lee', 'james.lee@example.com', '09123456789', '852 Fir St, Muntinlupa', '1234567890', 'Korean', '1989-03-28', 'Passport', 'Regular'),
('Maria', 'Cruz', 'maria.cruz@example.com', '09234567890', '963 Grove Ave, Caloocan', '2345678901', 'Filipino', '1992-08-12', 'Driver''s License', 'VIP'),
('Kevin', 'Park', 'kevin.park@example.com', '09345678901', '159 Pine Blvd, Pasig', '3456789012', 'Filipino', '1990-12-05', 'Passport', 'Regular'),
('Laura', 'Ng', 'laura.ng@example.com', '09456789012', '753 Birch Rd, Marikina', '4567890123', 'Filipino', '1988-07-22', 'Driver''s License', 'Regular'),
('Steven', 'Cooper', 'steven.cooper@example.com', '09567890123', '456 Maple Dr, San Juan', '5678901234', 'American', '1985-04-18', 'Passport', 'VIP'),
('Rachel', 'Kim', 'rachel.kim@example.com', '09678901234', '654 Cedar Ln, Valenzuela', '6789012345', 'Filipino', '1993-11-09', 'Passport', 'Regular'),
('Brian', 'Mendoza', 'brian.mendoza@example.com', '09789012345', '321 Willow Ct, Malabon', '7890123456', 'Filipino', '1991-01-30', 'Driver''s License', 'Regular'),
('Sophia', 'Gomez', 'sophia.gomez@example.com', '09890123456', '987 Elm St, Navotas', '8901234567', 'Filipino', '1994-09-15', 'Passport', 'VIP'),
('Daniel', 'Hernandez', 'daniel.hernandez@example.com', '09901234567', '147 Spruce Ave, Pateros', '9012345678', 'Filipino', '1987-05-23', 'Driver''s License', 'Regular'),
('Olivia', 'Reyes', 'olivia.reyes@example.com', '09012345678', '258 Fir Rd, Cainta', '0123456789', 'Filipino', '1992-02-28', 'Passport', 'Regular');

-- Reservations (20 records)
-- Note: Adjust RoomID and numberOfNights as needed
INSERT INTO reservations (GuestID, Check_InDate, Check_OutDate, NumAdult, NumChildren, SpecialRequest, ReservationStatus, ReservationType, RoomID, numberOfNights, BookingReferences) VALUES
(1, '2025-01-10', '2025-01-12', 2, 0, 'Late check-in preferred', 'Confirmed', 'Walk-in', 101, 2, 'BKG-2025-0001'),
(2, '2025-01-11', '2025-01-14', 2, 1, 'Extra bed needed', 'Confirmed', 'Online', 102, 3, 'BKG-2025-0002'),
(3, '2025-01-12', '2025-01-13', 1, 0, '', 'Checked-In', 'Walk-in', 103, 1, 'BKG-2025-0003'),
(4, '2025-01-13', '2025-01-16', 2, 2, 'Connecting rooms', 'Confirmed', 'Online', 104, 3, 'BKG-2025-0004'),
(5, '2025-01-14', '2025-01-15', 1, 1, 'Near elevator', 'Confirmed', 'Walk-in', 105, 1, 'BKG-2025-0005'),
(6, '2025-01-15', '2025-01-18', 2, 0, 'Late checkout', 'Confirmed', 'Online', 106, 3, 'BKG-2025-0006'),
(7, '2025-01-16', '2025-01-17', 1, 0, 'Quiet room', 'Checked-In', 'Walk-in', 107, 1, 'BKG-2025-0007'),
(8, '2025-01-17', '2025-01-20', 2, 1, 'Balcony preferred', 'Confirmed', 'Online', 108, 3, 'BKG-2025-0008'),
(9, '2025-01-18', '2025-01-19', 1, 0, '', 'Confirmed', 'Walk-in', 109, 1, 'BKG-2025-0009'),
(10, '2025-01-19', '2025-01-22', 2, 2, 'Extra pillows', 'Confirmed', 'Online', 110, 3, 'BKG-2025-0010'),
(11, '2025-01-20', '2025-01-21', 1, 1, 'Early check-in', 'Confirmed', 'Walk-in', 111, 1, 'BKG-2025-0011'),
(12, '2025-01-21', '2025-01-24', 2, 0, 'High floor', 'Confirmed', 'Online', 112, 3, 'BKG-2025-0012'),
(13, '2025-01-22', '2025-01-23', 1, 0, 'No smoking', 'Checked-In', 'Walk-in', 113, 1, 'BKG-2025-0013'),
(14, '2025-01-23', '2025-01-26', 2, 1, 'Airport transfer', 'Confirmed', 'Online', 114, 3, 'BKG-2025-0014'),
(15, '2025-01-24', '2025-01-25', 1, 0, '', 'Confirmed', 'Walk-in', 115, 1, 'BKG-2025-0015'),
(16, '2025-01-25', '2025-01-28', 2, 2, 'Late check-in', 'Confirmed', 'Online', 116, 3, 'BKG-2025-0016'),
(17, '2025-01-26', '2025-01-27', 1, 1, 'Quiet room', 'Confirmed', 'Walk-in', 117, 1, 'BKG-2025-0017'),
(18, '2025-01-27', '2025-01-30', 2, 0, 'Early checkout', 'Confirmed', 'Online', 118, 3, 'BKG-2025-0018'),
(19, '2025-01-28', '2025-01-29', 1, 0, '', 'Checked-In', 'Walk-in', 119, 1, 'BKG-2025-0019'),
(20, '2025-01-29', '2025-01-31', 2, 1, 'Extra bed', 'Confirmed', 'Online', 120, 2, 'BKG-2025-0020');

-- Payments (20 records)
-- Mix of full payments, partial, and additional services
INSERT INTO payment (ReservationID, amount, Payment_method, Payment_Status, Payment_Date, UserID) VALUES
(1, 4500.00, 'Cash', 'Paid', '2025-01-12 14:30:00', 1),
(2, 6800.00, 'Credit Card', 'Paid', '2025-01-14 10:15:00', 2),
(3, 2200.00, 'Cash', 'Paid', '2025-01-13 11:00:00', 1),
(4, 9500.00, 'Bank Transfer', 'Paid', '2025-01-16 09:45:00', 3),
(5, 2300.00, 'Cash', 'Paid', '2025-01-15 16:20:00', 1),
(6, 6900.00, 'Credit Card', 'Paid', '2025-01-18 12:00:00', 2),
(7, 2100.00, 'Cash', 'Paid', '2025-01-17 13:10:00', 1),
(8, 7200.00, 'Bank Transfer', 'Paid', '2025-01-20 10:30:00', 3),
(9, 2400.00, 'Cash', 'Paid', '2025-01-19 15:45:00', 1),
(10, 9800.00, 'Credit Card', 'Paid', '2025-01-22 11:20:00', 2),
(11, 2500.00, 'Cash', 'Paid', '2025-01-21 14:00:00', 1),
(12, 7100.00, 'Bank Transfer', 'Paid', '2025-01-24 09:00:00', 3),
(13, 2200.00, 'Cash', 'Paid', '2025-01-23 12:30:00', 1),
(14, 9400.00, 'Credit Card', 'Paid', '2025-01-26 13:50:00', 2),
(15, 2600.00, 'Cash', 'Paid', '2025-01-25 16:10:00', 1),
(16, 10000.00, 'Bank Transfer', 'Paid', '2025-01-28 10:15:00', 3),
(17, 2700.00, 'Cash', 'Paid', '2025-01-27 11:40:00', 1),
(18, 7300.00, 'Credit Card', 'Paid', '2025-01-30 14:25:00', 2),
(19, 2800.00, 'Cash', 'Paid', '2025-01-29 15:00:00', 1),
(20, 7600.00, 'Bank Transfer', 'Paid', '2025-01-31 09:30:00', 3);

-- Optional: Additional Service payments (a few examples)
INSERT INTO payment (ReservationID, amount, Payment_method, Payment_Status, Payment_Date, UserID) VALUES
(2, 500.00, 'Additional Service', 'Charge', '2025-01-14 18:00:00', 2),
(4, 800.00, 'Additional Service', 'Charge', '2025-01-16 20:00:00', 3),
(8, 300.00, 'Additional Service', 'Charge', '2025-01-20 19:00:00', 3),
(10, 600.00, 'Additional Service', 'Charge', '2025-01-22 21:00:00', 2),
(16, 1000.00, 'Additional Service', 'Charge', '2025-01-28 22:00:00', 3);
