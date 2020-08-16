using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mk.DemoB.Dto.DtoValid
{
    public class CreateBookDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, 999.99)]
        public decimal Price { get; set; }
    }
}
