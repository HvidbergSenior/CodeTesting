using System.Collections.ObjectModel;
using deftq.BuildingBlocks.Fakes;
using deftq.Catalog.Application.Import.GetMaterialImportStatus;
using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Catalog.Domain.MaterialImport;
using deftq.Catalog.Infrastructure;
using Xunit;

namespace deftq.Catalog.Test.Application.Import.GetMaterialImportStatus
{
    public class GetMaterialImportStatusQueryTest
    {
        [Fact]
        public async Task GivenNoMaterialsAndNoImport_WhenQueryingStatus_StatusIsBlank()
        {
            var materialImportPageRepository = new MaterialImportPageInMemoryRepository();
            var materialRepository = new MaterialInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();

            var query = GetMaterialImportStatusQuery.Create();
            var handler = new GetMaterialImportStatusQueryHandler(materialImportPageRepository, materialRepository, unitOfWork);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.False(response.IsComplete);
            Assert.Empty(response.Pages);
            Assert.Equal(DateTimeOffset.MinValue, response.CurrentCatalogPublished);
        }

        [Fact]
        public async Task GivenExistingMaterials_WhenQueryingStatus_CurrentCatalogPublishedIsIncluded()
        {
            var materialImportPageRepository = new MaterialImportPageInMemoryRepository();
            var materialRepository = new MaterialInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();

            var material1 = Material.Create(MaterialId.Create(Guid.NewGuid()), EanNumber.Empty(), MaterialName.Empty(), MaterialUnit.Empty(),
                new Collection<Mounting>(), MaterialReference.Empty(), MaterialReference.Empty(),
                MaterialPublished.Create(new DateTimeOffset(2023, 2, 1, 12, 22, 23, TimeSpan.Zero)));
            var material2 = Material.Create(MaterialId.Create(Guid.NewGuid()), EanNumber.Empty(), MaterialName.Empty(), MaterialUnit.Empty(),
                new Collection<Mounting>(), MaterialReference.Empty(), MaterialReference.Empty(),
                MaterialPublished.Create(new DateTimeOffset(2022, 1, 4, 2, 56, 45, TimeSpan.Zero)));

            await materialRepository.Add(material1);
            await materialRepository.Add(material2);

            var query = GetMaterialImportStatusQuery.Create();
            var handler = new GetMaterialImportStatusQueryHandler(materialImportPageRepository, materialRepository, unitOfWork);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(material1.Published.Value, response.CurrentCatalogPublished);
        }

        [Fact]
        public async Task GivenMissingImportPages_WhenQueryingStatus_ImportIsIncomplete()
        {
            var materialImportPageRepository = new MaterialImportPageInMemoryRepository();
            var materialRepository = new MaterialInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();

            await materialImportPageRepository.Add(MaterialImportPage.Create(MaterialImportPageId.Create(Guid.NewGuid()), DateTimeOffset.Now, 1, 100,
                false, new List<Material>()));

            var query = GetMaterialImportStatusQuery.Create();
            var handler = new GetMaterialImportStatusQueryHandler(materialImportPageRepository, materialRepository, unitOfWork);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.False(response.IsComplete);
            Assert.Single(response.Pages);
        }
        
        [Fact]
        public async Task GivenCompleteImportPages_WhenQueryingStatus_ImportIncomplete()
        {
            var materialImportPageRepository = new MaterialImportPageInMemoryRepository();
            var materialRepository = new MaterialInMemoryRepository();
            var unitOfWork = new FakeUnitOfWork();

            var published= DateTimeOffset.Now;

            var materials = Enumerable.Repeat(Any.Material(), 100).ToList();

            await materialImportPageRepository.Add(MaterialImportPage.Create(MaterialImportPageId.Create(Guid.NewGuid()), published, 1, 100,
                false, materials));

            await materialImportPageRepository.Add(MaterialImportPage.Create(MaterialImportPageId.Create(Guid.NewGuid()), published, 101, 100,
                true, materials));
            
            var query = GetMaterialImportStatusQuery.Create();
            var handler = new GetMaterialImportStatusQueryHandler(materialImportPageRepository, materialRepository, unitOfWork);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.True(response.IsComplete);
            Assert.Equal(2, response.Pages.Count);
        }
    }
}
