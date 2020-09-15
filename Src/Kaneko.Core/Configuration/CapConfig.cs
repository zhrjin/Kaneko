using System;
using System.Collections.Generic;
using System.Text;

namespace Kaneko.Core.Configuration
{
    public class CapConfig
    {
        public bool Enable { get; set; } = true;

        public ServiceDiscoveryConfig ServiceDiscovery { get; set; } = new ServiceDiscoveryConfig();
    }
}
