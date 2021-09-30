using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Leopard.Saas
{
    public class EditionDataSeeder : IEditionDataSeeder, ITransientDependency
	{
		protected IEditionRepository EditionRepository;

		protected IGuidGenerator GuidGenerator { get; }

		public EditionDataSeeder(IEditionRepository editionRepository, IGuidGenerator guidGenerator)
		{
			this.EditionRepository = editionRepository;
			this.GuidGenerator = guidGenerator;
		}

		public virtual async Task CreateStandardEditionsAsync()
		{
			await this.AddEditionIfNotExistsAsync("Standard");
		}

		protected virtual async Task AddEditionIfNotExistsAsync(string displayName)
		{
			var flag = await this.EditionRepository.CheckNameExistAsync(displayName);

			if (!flag)
			{
				await this.EditionRepository.InsertAsync(new Edition(this.GuidGenerator.Create(), displayName));
			}
		}
	}
}
