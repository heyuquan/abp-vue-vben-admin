using System.ComponentModel.DataAnnotations;

namespace Leopard.AspNetCore.Serilog
{
    public class LoggerOptions
    {
        public const string SectionName = "Application:Logger";
        [Required]
        public bool? EnableToFile { get; set; } 
        public bool? EnableToElasticsearch { get; set; }
    }
}
