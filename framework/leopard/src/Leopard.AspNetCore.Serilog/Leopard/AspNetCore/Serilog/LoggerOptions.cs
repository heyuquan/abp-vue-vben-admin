using System.ComponentModel.DataAnnotations;

namespace Leopard.AspNetCore.Serilog
{
    public class LoggerOptions
    {
        public const string SectionName = "Application:Logger";
        public bool EnableToFile { get; set; } = true;
        public bool EnableToElasticsearch { get; set; }
    }
}
