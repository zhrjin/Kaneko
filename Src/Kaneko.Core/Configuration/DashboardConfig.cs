namespace Kaneko.Core.Configuration
{
    public class DashboardConfig
    {
        public string WriteInterval { get; set; } = "00:00:05"; // Recommended, not less than
        public bool HideTrace { get; set; } = true;
        public bool Enable { get; set; } = true;
        public int SiloDashboardPort { get; set; } = 7080;
        public string UserName { get; set; } = "Silo";
        public string Password { get; set; } = "Silo";
    }
}
