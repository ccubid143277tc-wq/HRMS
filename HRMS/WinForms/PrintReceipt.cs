using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using HRMS.Helper;
using HRMS.DbContext;
using MySql.Data.MySqlClient;

namespace HRMS.WinForms
{
    public partial class PrintReceipt : Form
    {
        private readonly int _reservationId;
        private readonly int _paymentId;
        private readonly MySqlReceiptDbContext _receiptDb;

        public PrintReceipt(int reservationId, int paymentId)
        {
            InitializeComponent();

            _reservationId = reservationId;
            _paymentId = paymentId;
            _receiptDb = new MySqlReceiptDbContext();

            Load += PrintReceipt_Load;
        }

        private void PrintReceipt_Load(object? sender, EventArgs e)
        {
            LoadReceiptData();
        }

        private void LoadReceiptData()
        {
            // Latest payment info
            var (latestPaymentId, latestPaymentDate) = _receiptDb.GetLatestPaymentForReservation(_reservationId);

            // Reservation + guest details
            if (!_receiptDb.TryGetReservationDetails(_reservationId, out string guestName, out string address, out string phone, out string roomType, out string roomNumbers, out string bookingRef, out DateTime checkIn, out DateTime checkOut, out int nights, out int adults, out int children, out decimal nightlyRoomRateSum))
            {
                // Could not fetch reservation details; keep defaults
                guestName = "Guest";
                address = "";
                phone = "";
                roomType = "";
                roomNumbers = "";
                bookingRef = "";
                checkIn = DateTime.MinValue;
                checkOut = DateTime.MinValue;
                nights = 0;
                adults = 0;
                children = 0;
                nightlyRoomRateSum = 0m;
            }

            // Totals
            _receiptDb.TryGetTotals(_reservationId, out decimal totalDue, out decimal totalPaid, out decimal balance, out decimal serviceCharges);

            // Receipt date
            DateTime receiptDate = _receiptDb.GetReceiptDate(_reservationId);

            // Prefer the latest payment details for receipt meta.
            int effectivePaymentId = latestPaymentId > 0 ? latestPaymentId : _paymentId;
            if (latestPaymentId > 0)
            {
                receiptDate = latestPaymentDate;
            }

            // Header + billed to
            label6.Text = string.IsNullOrWhiteSpace(guestName) ? "Guest" : guestName;
            label7.Text = string.IsNullOrWhiteSpace(address) ? "" : address;
            label8.Text = string.IsNullOrWhiteSpace(phone) ? "" : phone;

            // Receipt meta
            string receiptNo = effectivePaymentId > 0 ? $"TMH - {effectivePaymentId:000}" : (string.IsNullOrWhiteSpace(bookingRef) ? "TMH - 000" : bookingRef);
            label11.Text = receiptNo;
            label12.Text = receiptDate.ToString("MMM-dd-yyyy");

            // Reservation details
            label26.Text = string.IsNullOrWhiteSpace(roomType) ? "" : roomType;
            label27.Text = string.IsNullOrWhiteSpace(roomNumbers) ? "" : roomNumbers;
            label23.Text = checkIn == DateTime.MinValue ? "" : checkIn.ToString("MMM-dd-yyyy");
            label24.Text = checkOut == DateTime.MinValue ? "" : checkOut.ToString("MMM-dd-yyyy");
            label25.Text = nights.ToString();

            DateTime displayCheckIn = checkIn;
            if (displayCheckIn != DateTime.MinValue && displayCheckIn.TimeOfDay == TimeSpan.Zero)
            {
                displayCheckIn = displayCheckIn.Date.AddHours(12);
            }

            DateTime displayCheckOut = checkOut;
            if (displayCheckOut != DateTime.MinValue && displayCheckOut.TimeOfDay == TimeSpan.Zero)
            {
                displayCheckOut = displayCheckOut.Date.AddHours(12);
            }

            if (label41 != null)
            {
                label41.Text = $"• Check-in Time: {(displayCheckIn == DateTime.MinValue ? "" : displayCheckIn.ToString("hh:mm tt"))}";
            }

            if (label42 != null)
            {
                label42.Text = $"• Check-out Time: {(displayCheckOut == DateTime.MinValue ? "" : displayCheckOut.ToString("hh:mm tt"))}";
            }

            string guestsText;
            if (children > 0 && adults > 0)
            {
                guestsText = $"{children} Child, {adults} Adults";
            }
            else
            {
                guestsText = (adults + children).ToString();
            }
            label28.Text = guestsText;

            // Charges breakdown
            decimal roomSubtotal = nightlyRoomRateSum * Math.Max(0, nights);

            // Tax breakdown:
            // - Room total due (rtcalc.TotalDue) is roomSubtotal * 1.05
            // - Additional services are stored as already-taxed charges (amount includes the 5%)
            decimal roomTax = Math.Round(roomSubtotal * 0.05m, 2);
            decimal serviceChargesBase = 0m;
            if (serviceCharges > 0m)
            {
                serviceChargesBase = Math.Round(serviceCharges / 1.05m, 2);
            }
            decimal serviceTax = Math.Round(serviceCharges - serviceChargesBase, 2);
            decimal taxTotal = Math.Round(roomTax + serviceTax, 2);

            label29.Text = $"Room Rate ({Math.Max(0, nights)} nights)";
            label30.Text = MoneyHelper.Format(roomSubtotal);
            label31.Text = "Additional Services";
            label33.Text = MoneyHelper.Format(serviceCharges);

            label51.Text = MoneyHelper.Format(taxTotal);

            // Discount is not stored in DB (based on current code), so default to 0
            label36.Text = MoneyHelper.Format(0m);
            label34.Text = MoneyHelper.Format(totalDue);
            label39.Text = MoneyHelper.Format(totalPaid);
        }
        
    }
}
