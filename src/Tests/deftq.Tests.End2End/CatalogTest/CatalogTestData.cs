using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Catalog.Domain.OperationCatalog;
using deftq.Catalog.Domain.SupplementCatalog;
using deftq.Catalog.Domain.Time;
using deftq.Catalog.Infrastructure;
using Marten;

namespace deftq.Tests.End2End.CatalogTest
{
    public static class CatalogTestData
    {
        public static Guid BoxHoleOperationId { get; } = Guid.Parse("FE967880-C3F9-4881-B470-9DC1DEC28FC1");
        public static Guid RemoveCoverOperationId { get; } = Guid.Parse("C76A9035-BCB0-4C63-8027-0E3218818220");
        public static string BoxHoleOperationName { get; } = "Hul for 2M Euro-dåse i gips <=15mm/1 lag, Ø o/60-80 mm";
        public static Guid MaterialWithMasterId { get; } = Guid.Parse("D9EFABAB-A5F7-4E35-B640-6A91F2662667");
        public static Guid MaterialWithReplacementId { get; } = Guid.Parse("96FBF0E9-9CA0-46EE-B0C0-663DC3DA4F61");
        public static string MaterialWithMasterName { get; } = "Generelt 3x1,5 kabel med røde markeringer";
        public static Guid TripleCableEmbeddedSupplementOperationId { get; }= Guid.Parse("DF554BDD-90E0-4883-A485-C117CA27BC87");
        public static Guid TripleCableMaterialId { get; } = Guid.Parse("4EAE982E-21BE-4596-8039-DA8A45BB7FD7");
        public static Guid Bah25SupplementId { get; } = Guid.Parse("C0C64FCC-D855-43ED-B808-9094C3D60646");
        public static Guid Bah75SupplementId { get; } = Guid.Parse("28AD88AD-C66B-49A8-88DF-CAFAB2A87290");
        
        /// <summary>
        /// Import some test materials (there is no API functionality for importing materials).
        /// Additional test materials may be imported in the search controller.
        /// </summary>
#pragma warning disable MA0051
        internal static async Task ImportMaterials(WebAppFixture fixture)
        {
            var sessionFactory = (ISessionFactory?)fixture.AppFactory.Services.GetService(typeof(ISessionFactory))!;
            var entityEventsPublisher = (IEntityEventsPublisher?)fixture.AppFactory.Services.GetService(typeof(IEntityEventsPublisher))!;
            var martenConfig = (IMartenConfig)fixture.AppFactory.Services.GetService(typeof(IMartenConfig))!;

            await using var documentSession = sessionFactory.OpenSession();
            var uow = new MartenUnitOfWork(documentSession);
            var materialRepository = new MaterialRepository(documentSession, entityEventsPublisher, martenConfig);
            var operationRepository = new OperationRepository(documentSession, entityEventsPublisher, martenConfig);

            var simpleEmbeddedSupplementOperation =
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("80F99085-A142-4ED5-91B3-A5D310B86DA3")),
                    OperationNumber.Create("321"), OperationText.Create("Master operation"), OperationTime.Create(29000), SupplementOperationType.UnitRelated());
            var simpleMaterial = Material.Create(MaterialId.Create(Guid.Parse("32C8E0F0-AE4B-4261-B40D-5FC7CC8CA3EB")),
                EanNumber.Create("1112221112211"),
                MaterialName.Create("Simpelt 3x1,5 kabel"), MaterialUnit.Meter(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(3), OperationTime.Create(25000),
                        new List<SupplementOperation> { simpleEmbeddedSupplementOperation })
                },
                MaterialReference.Empty(), MaterialReference.Empty(), MaterialPublished.Empty());
            
            var materialWithMaster = Material.Create(MaterialId.Create(MaterialWithMasterId),
                EanNumber.Create("2222222222222"),
                MaterialName.Create(MaterialWithMasterName), MaterialUnit.Meter(),
                new List<Mounting> { Mounting.Create(MountingCode.FromCode(25), OperationTime.Create(65000), new List<SupplementOperation>()) },
                MaterialReference.Create(simpleMaterial.MaterialId, simpleMaterial.EanNumber),
                MaterialReference.Empty(), MaterialPublished.Empty());

            var materialWithReplacement = Material.Create(MaterialId.Create(MaterialWithReplacementId),
                EanNumber.Create("3333333333333"),
                MaterialName.Create("Gammelt 3x1,5 kabel"), MaterialUnit.Meter(),
                new List<Mounting> { Mounting.Create(MountingCode.FromCode(80), OperationTime.Create(125000), new List<SupplementOperation>()) },
                MaterialReference.Empty(), MaterialReference.Create(simpleMaterial.MaterialId, simpleMaterial.EanNumber), MaterialPublished.Empty());

            // Material with embedded supplement operation and supplement operation from operation catalog
            var embeddedSupplementOperation =
                SupplementOperation.Create(SupplementOperationId.Create(TripleCableEmbeddedSupplementOperationId),
                    OperationNumber.Create("123"), OperationText.Create("Just do something"), OperationTime.Create(29000), SupplementOperationType.AmountRelated());
            var supplementOperation =
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("3F65DA66-C36E-4BB7-9CB8-D968D4783AFF")),
                    OperationId.Create(Guid.Parse("E4A8788E-97B3-426A-ADFC-AEFDFEB180D9")), SupplementOperationType.UnitRelated());
            var anotherSupplementOperation =
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("26C6F8BA-1788-4D4A-A908-47521F09F6F7")),
                    OperationId.Create(Guid.Parse("10FB85BE-8909-4202-BD59-81ED84F01963")), SupplementOperationType.UnitRelated());
            var operation1 = Operation.Create(supplementOperation.OperationId, OperationNumber.Create("123"), OperationText.Create("Dig a hole"),
                OperationTime.Create(31000m));
            var operation2 = Operation.Create(anotherSupplementOperation.OperationId, OperationNumber.Create("123"), OperationText.Create("Do something unexpected"),
                OperationTime.Create(48000m));
            var materialWithMultipleOperations = Material.Create(MaterialId.Create(TripleCableMaterialId),
                EanNumber.Create("4444444444444"), MaterialName.Create("Trippelt 3x1,5 kabel"), MaterialUnit.Meter(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(3), OperationTime.Create(25000),
                        new List<SupplementOperation> { embeddedSupplementOperation, supplementOperation, anotherSupplementOperation })
                },
                MaterialReference.Empty(), MaterialReference.Empty(), MaterialPublished.Empty());


            // Upsert material documents
            await materialRepository.DeleteById(simpleMaterial.Id);
            await materialRepository.Update(simpleMaterial);
            await materialRepository.DeleteById(materialWithMaster.Id);
            await materialRepository.Update(materialWithMaster);
            await materialRepository.DeleteById(materialWithReplacement.Id);
            await materialRepository.Update(materialWithReplacement);
            await materialRepository.DeleteById(materialWithMultipleOperations.Id);
            await materialRepository.Update(materialWithMultipleOperations);
            await operationRepository.Update(operation1);
            await operationRepository.DeleteById(operation1.Id);
            await operationRepository.Update(operation2);
            await operationRepository.DeleteById(operation2.Id);

            await materialRepository.SaveChanges();
            await uow.Commit(CancellationToken.None);
        }
#pragma warning restore MA0051
        
        internal static async Task ImportSupplements(WebAppFixture fixture)
        {
            var sessionFactory = (ISessionFactory?)fixture.AppFactory.Services.GetService(typeof(ISessionFactory))!;
            var entityEventsPublisher = (IEntityEventsPublisher?)fixture.AppFactory.Services.GetService(typeof(IEntityEventsPublisher))!;

            await using var documentSession = sessionFactory.OpenSession();
            var uow = new MartenUnitOfWork(documentSession);
            var supplementRepository = new SupplementRepository(documentSession, entityEventsPublisher);
            
            var bah25 = Supplement.Create(SupplementId.Create(Bah25SupplementId), SupplementNumber.Create("bah25"), SupplementText.Create("Begrænset arbejdshøjde fra 100 t.o.m. 150cm v/ fremspring over 50cm 25%"), SupplementValue.Create(25));
            var bah75 = Supplement.Create(SupplementId.Create(Bah75SupplementId), SupplementNumber.Create("bah75"), SupplementText.Create("Begrænset arbejdshøjde fra 50 t.o.m. 100cm v/ fremspring over 20cm 75%"), SupplementValue.Create(75));

            // Upsert supplement documents
            await supplementRepository.DeleteById(bah25.Id);
            await supplementRepository.Update(bah25);
            await supplementRepository.DeleteById(bah75.Id);
            await supplementRepository.Update(bah75);
            
            await supplementRepository.SaveChanges();
            await uow.Commit(CancellationToken.None);
        }

        public static async Task ImportOperations(WebAppFixture fixture)
        {
            var sessionFactory = (ISessionFactory?)fixture.AppFactory.Services.GetService(typeof(ISessionFactory))!;
            var entityEventsPublisher = (IEntityEventsPublisher?)fixture.AppFactory.Services.GetService(typeof(IEntityEventsPublisher))!;
            var martenConfig = (IMartenConfig)fixture.AppFactory.Services.GetService(typeof(IMartenConfig))!;

            await using var documentSession = sessionFactory.OpenSession();
            var uow = new MartenUnitOfWork(documentSession);
            var supplementRepository = new OperationRepository(documentSession, entityEventsPublisher, martenConfig);

            var op1 = Operation.Create(OperationId.Create(RemoveCoverOperationId), OperationNumber.Create("170141000001"), OperationText.Create("Fjerne dæksel"), OperationTime.Create(269));
            var op2 = Operation.Create(OperationId.Create(BoxHoleOperationId), OperationNumber.Create("170200000001"), OperationText.Create(BoxHoleOperationName), OperationTime.Create(2996));

            // Upsert operation documents
            await supplementRepository.DeleteById(op1.Id);
            await supplementRepository.Update(op1);
            await supplementRepository.DeleteById(op2.Id);
            await supplementRepository.Update(op2);
            
            await supplementRepository.SaveChanges();
            await uow.Commit(CancellationToken.None);
        }
    }
}
