namespace Models
{
    public class BookingDates
    {
        public required string checkin { get; set; }
        public required string checkout { get; set; }
    }

    public class BookingRequest
    {
        public required string firstname { get; set; }
        public required string lastname { get; set; }
        public required int totalprice { get; set; }
        public required bool depositpaid { get; set; }
        public required BookingDates bookingdates { get; set; }
        public required string additionalneeds { get; set; }
    }

    public class BookingResponse
    {
        public int bookingid { get; set; }
        public BookingRequest? booking { get; set; }
    }

    public class AuthRequest { public string? username { get; set; } public string? password { get; set; } }
    public class AuthResponse { public string? token { get; set; } }
}