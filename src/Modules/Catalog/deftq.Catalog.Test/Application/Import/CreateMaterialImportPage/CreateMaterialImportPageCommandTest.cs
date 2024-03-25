using deftq.BuildingBlocks.Fakes;
using deftq.Catalog.Application.Import.CreateMaterialImportPage;
using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Catalog.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace deftq.Catalog.Test.Application.Import.CreateMaterialImportPage
{
    public class CreateMaterialImportPageCommandTest
    {
        private readonly ITestOutputHelper _output;

        public CreateMaterialImportPageCommandTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task GivenExistingMaterial_WhenImportingPage_MaterialIdsAreReused()
        {
            var materialImportRepo = new MaterialImportPageInMemoryRepository();
            var materialRepo = new MaterialInMemoryRepository();
            var existingMaterial = Any.Material().WithEanNumber(EanNumber.Create("1111111111111"));
            await materialRepo.Add(existingMaterial);
            var unitOfWork = new FakeUnitOfWork();
            var logger = new TestLogger<CreateMaterialImportPageCommand>(_output);
            
            var importMaterials = new List<ImportMaterial>();
            importMaterials.Add(new ImportMaterial("1111111111111", "abc", "meter", new List<ImportMounting>()));
            importMaterials.Add(new ImportMaterial("2222222222222", "abc", "meter", new List<ImportMounting>()));
            var cmd = CreateMaterialImportPageCommand.Create(DateTimeOffset.Now, 1, 2, true, importMaterials);
            
            var handler = new CreateMaterialImportPageCommandHandler(materialImportRepo, materialRepo, unitOfWork, logger);

            await handler.Handle(cmd, CancellationToken.None);
            
            var importPages = materialImportRepo.Query().ToList();
            
            Assert.Single(importPages);
            Assert.Equal(2, importPages[0].Content.Count);
            
            var material1 = importPages[0].Content.Single(m => string.Equals(m.EanNumber.Value, "1111111111111", StringComparison.OrdinalIgnoreCase));
            var material2 = importPages[0].Content.Single(m => string.Equals(m.EanNumber.Value, "2222222222222", StringComparison.OrdinalIgnoreCase));
            
            Assert.Equal(existingMaterial.MaterialId.Value, material1.MaterialId.Value);
            Assert.NotEqual(existingMaterial.MaterialId.Value, material2.MaterialId.Value);
        }
    }
}
