using System.ComponentModel.DataAnnotations;

namespace Leopard.AspNetCore.Serilog
{
    public class LeopardLogOptions
    {
        public const string SectionName = "Leopard:Log";
        [Required]
        public bool EnableToFile { get; set; } = true;
        public bool EnableToElasticsearch { get; set; } = false;

    }
}
