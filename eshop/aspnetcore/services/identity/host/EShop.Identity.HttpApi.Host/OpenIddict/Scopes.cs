using System.Collections.Generic;

namespace EShop.Identity.OpenIddict;

public class Scopes
{
    public string Name { get; set; }
    public IEnumerable<string> Resources { get; set; }
}