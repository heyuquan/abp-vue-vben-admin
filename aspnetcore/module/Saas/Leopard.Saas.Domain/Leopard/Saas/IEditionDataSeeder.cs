using System.Threading.Tasks;

namespace Leopard.Saas
{
    public interface IEditionDataSeeder
	{
		Task CreateStandardEditionsAsync();
	}
}
