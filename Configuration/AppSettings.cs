namespace Lister.WebApi.Configuration
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public Authentication Authentication { get; set; }
        public string? AllowedHosts { get; set; }
        public string? RedisCacheUrl { get; set; }
    }
}
