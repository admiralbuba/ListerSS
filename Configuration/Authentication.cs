namespace ListerSS.Configuration
{
    public class Authentication
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string SecretKey { get; set; }
    }
}