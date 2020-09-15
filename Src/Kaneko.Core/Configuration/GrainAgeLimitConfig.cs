using System;
using System.ComponentModel.DataAnnotations;
namespace Kaneko.Core.Configuration
{
    public class GrainAgeLimitConfig
    {
        /// <remarks>
        /// The CollectionAgeLimit must be greater than CollectionQuantum, which is set to 00:01:00 (by default).
        /// https://dotnet.github.io/orleans/Documentation/clusters_and_clients/configuration_guide/activation_garbage_collection.html
        /// See CollectionAgeLimitValidator.cs details.
        /// </remarks>
        [Range(1.001d, double.MaxValue, ErrorMessage = "The GrainAgeLimitInMins " +
                                                       "(CollectionAgeLimit) must be greater than CollectionQuantum, " +
                                                       "which is set to 1 min (by default). The type is double.")]
        public double GrainAgeLimitInMins { get; set; }

        /// <summary>
        /// The full qualified type name to apply grain age limit.
        /// </summary>
        public string GrainType { get; set; }
    }
}
