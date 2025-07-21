namespace Domain.Entities
{
    public class EmailVerification
    {
        public int Id { get; set; }
        public string Email { get; set; } // unique
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
        public bool Verified { get; set; }
    }
}
