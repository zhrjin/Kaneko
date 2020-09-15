namespace Kaneko.Core.Configuration
{
    public class OrleansClientConfig
    {
        public string ServiceName { get; set; }

        public string ServiceId
        {
            get
            {
                return ServiceName;
            }
        }

        public string ClusterId
        {
            get
            {
                return ServiceName;
            }
        }

        public ConsulConfig Consul { get; set; } = new ConsulConfig();

        public string[] ServiceAssembly { get; set; }
    }
}
