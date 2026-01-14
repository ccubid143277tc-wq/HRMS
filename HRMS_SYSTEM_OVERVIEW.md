# HRMS System Overview: Structure and Flow

## Core Entities
- Guest
  - ID, Name, Contact, ID Type/Number, Classification
- Room
  - ID, Number, Type, Rate, Max Occupancy, RoomStatusID
  - RoomStatusID: 1=Available, 2=Occupied, 3=Maintenance, 4=Reserved
- Reservation
  - ID, GuestID, Check_InDate, Check_OutDate, Adults/Children, SpecialRequest
  - ReservationStatus: Pending, Confirmed, Checked-In, Checked-Out, Cancelled
  - BookingReferences (generated), RoomID/RoomTypeID
- Payment
  - ID, ReservationID, Amount, Payment_method, Payment_Status, UserID, Payment_Date
  - Payment_method examples: Cash, Card, Online, "Additional Service"
  - Payment_Status examples: Paid, Unpaid, Voided, Charge

---

## Key User Controls / Forms
### 1) UCreservation (Reservation Management)
- Create/Update reservations
- Auto-generates BookingReferences
- Status rule:
  - If Check_InDate > Today → ReservationStatus = "Pending"
  - Else → use dropdown (Pending, Confirmed, Checked-In, Checked-Out, Cancelled)
- Room status on save:
  - Checked-In → Occupied
  - Otherwise → Reserved
- Summary panel:
  - Room Rate per night × Nights = Subtotal
  - Tax = Subtotal × 5%
  - Total Amount = Subtotal + Tax

### 2) Payment (Billing & Payments)
- Grid shows active reservations (not Checked-Out)
- Columns: TotalDue, TotalPaid, Balance
- Billing summary:
  - Room Charges, Additional Services (with 5% service charge), Tax
  - Remaining Balance = Balance + Additional Services Charge
- Payment flow:
  - Amount Received → Amount to Pay = Min(Received, Effective Balance)
  - Change = Received - Amount to Pay
  - Insert payments:
    - Additional Service → method="Additional Service", status="Charge"
    - Regular payment → method=user-selected, status=Paid/Unpaid
- Check-Out:
  - Allowed only if Balance ≈ 0
  - Updates ReservationStatus = "Checked-Out"
  - Sets room status = "Available"

### 3) AdminDashboard
- Metrics:
  - Revenue Today (sum of payments, excludes voided/charges)
  - Pending Arrivals/Departures (via RoomManager)
  - Room Status Counts (Available/Occupied/Maintenance/Reserved)
  - Pending Reservations (WHERE ReservationStatus='Pending')
  - Pending Payments (count reservations with Balance > 0)
- Charts: Room Status Pie, Bookings by Room Type

### 4) ReceptionDashboard
- Occupancy Rate, Arrivals/Departures Today
- Expected Arrivals/Departures grids
- Weekly Occupancy Trend chart

---

## End-to-End Flow

### 1) Create Reservation
- User selects rooms and dates
- If Check_InDate > Today → Status = Pending, Room = Reserved
- Else → Status = Confirmed (or dropdown), Room = Reserved
- Saves Reservation + ReservationRooms

### 2) Check-In Day
- Reservation may be:
  - Pending (future date) → becomes Confirmed/Checked-In manually
  - Confirmed → ready for check-in
- When Checked-In:
  - Room status → Occupied
  - Optional: advance payments via UCreservation summary

### 3) Payments (Payment UC)
- User selects a reservation row
- System loads:
  - Room Charges, Additional Services, Current Balance
- User can:
  - Add payment (amount ≤ effective balance)
  - Add additional services (adds to balance)
- After payment:
  - Grid refreshes
  - Remaining Balance updates in billing summary

### 4) Check-Out
- Only allowed if Balance ≈ 0
- On Check-Out:
  - ReservationStatus = "Checked-Out"
  - RoomStatus = "Available"
  - Reservation disappears from active payment grid

---

## Business Rules Implemented
- Future check-ins → Pending status
- Room status transitions:
  - New reservation → Reserved
  - Checked-In → Occupied
  - Checked-Out → Available
- Tax:
  - Rooms: 5%
  - Additional Services: 5% (included in charge)
- Balance formula:
  - (Room Total Due + Service Charges) - Total Paid
- Outstanding payments metric counts rows where Balance > 0

---

## Database Notes
- RoomStatus IDs: 1=Available, 2=Occupied, 3=Maintenance, 4=Reserved
- Payment filtering:
  - Exclude Voided
  - Exclude "Additional Service" rows from Total Paid
  - Include "Additional Service" rows with Status="Charge" in Service Charges
- Stored procedures used for:
  - Expected arrivals/departures counts
  - Room availability
  - Occupied room counts by date

---

## Common UI Patterns
- Grids use manual column binding (AutoGenerateColumns = false)
- Money values formatted with ₱ and 2 decimal places
- Errors: MessageBox with try/catch; dashboard loads silently fail
- Current user session stored in UserSession (UserID, UserName, UserRole)

---

## Tips for Extending
- To add a new reservation status: add to dropdown, dictionary in GetReservationStatusCounts, and any status-based filters
- To add a new room status: add ID in GetRoomStatusCounts and update RoomStatus table
- To change tax/service charge rates: update RecalculateBillingSummary and UpdateReservationSummary
