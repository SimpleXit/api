namespace SimpleX.Helpers.Settings
{
    public class DbSettings
    {
        public string ConnectionString { get; set; }
        public bool EnableLogging { get; set; } = true;
        public bool EnableSensitiveLogging { get; set; } = true;
    }
}
