namespace Models
{
    public class DatabaseSettings 
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; } 
        public string UserName { get; set; } 
        public string Password { get; set; } 
        public string AuthMechanism { get; set; } 
    }
}