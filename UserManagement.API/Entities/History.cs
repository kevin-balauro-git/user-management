namespace UserManagement.API.Entities
{
    public class History
    {
        public int Id { get; set; }
        public string HttpVerb { get; set; }
        public DateTime LogDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; }
        
    }
}
