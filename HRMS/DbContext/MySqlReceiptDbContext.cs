using HRMS.Helper;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace HRMS.DbContext
{
    public class MySqlReceiptDbContext
    {
        public (int PaymentId, DateTime PaymentDate) GetLatestPaymentForReservation(int reservationId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetLatestPaymentForReservation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ReservationID", reservationId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int paymentId = Convert.ToInt32(reader["PaymentID"]);
                            DateTime paymentDate = Convert.ToDateTime(reader["Payment_Date"]);
                            return (paymentId, paymentDate);
                        }
                    }
                }
            }

            return (0, DateTime.Now);
        }

        public bool TryGetReservationDetails(int reservationId, out string guestName, out string address, out string phone, out string roomType, out string roomNumbers, out string bookingRef, out DateTime checkIn, out DateTime checkOut, out int nights, out int adults, out int children, out decimal nightlyRoomRateSum)
        {
            guestName = "";
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

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetReceiptReservationDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ReservationID", reservationId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bookingRef = reader["BookingReferences"]?.ToString() ?? "";
                            guestName = reader["GuestName"]?.ToString() ?? "";
                            address = reader["Address"]?.ToString() ?? "";
                            phone = reader["PhoneNumber"]?.ToString() ?? "";
                            roomType = reader["RoomType"]?.ToString() ?? "";
                            roomNumbers = reader["RoomNumbers"]?.ToString() ?? "";

                            DateTime.TryParse(reader["Check_InDate"]?.ToString() ?? "", out checkIn);
                            DateTime.TryParse(reader["Check_OutDate"]?.ToString() ?? "", out checkOut);
                            int.TryParse(reader["numberOfNights"]?.ToString() ?? "0", out nights);
                            int.TryParse(reader["NumAdult"]?.ToString() ?? "0", out adults);
                            int.TryParse(reader["NumChildren"]?.ToString() ?? "0", out children);
                            decimal.TryParse(reader["NightlyRoomRateSum"]?.ToString() ?? "0", out nightlyRoomRateSum);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool TryGetTotals(int reservationId, out decimal totalDue, out decimal totalPaid, out decimal balance, out decimal serviceCharges)
        {
            totalDue = 0m;
            totalPaid = 0m;
            balance = 0m;
            serviceCharges = 0m;

            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetReceiptTotals", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ReservationID", reservationId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal.TryParse(reader["TotalDue"]?.ToString() ?? "0", out totalDue);
                            decimal.TryParse(reader["TotalPaid"]?.ToString() ?? "0", out totalPaid);
                            decimal.TryParse(reader["Balance"]?.ToString() ?? "0", out balance);
                            decimal.TryParse(reader["ServiceCharges"]?.ToString() ?? "0", out serviceCharges);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public DateTime GetReceiptDate(int reservationId)
        {
            using (var conn = DBHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand("prcGetReceiptDate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_ReservationID", reservationId);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        if (DateTime.TryParse(result.ToString(), out var receiptDate))
                        {
                            return receiptDate;
                        }
                    }
                }
            }

            return DateTime.Now;
        }
    }
}
