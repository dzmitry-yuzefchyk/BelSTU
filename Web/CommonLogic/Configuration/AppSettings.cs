namespace CommonLogic.Configuration
{
    public class AppSettings
    {
        public Host Host { get; set; }
        public Jwt Jwt { get; set; }
    }

    public class Host
    {
        public string Url { get; set; }
    }

    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}
