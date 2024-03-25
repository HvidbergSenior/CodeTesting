using deftq.BuildingBlocks.DataAccess.InitialData;
using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Catalog.Domain.OperationCatalog;
using deftq.Catalog.Domain.Time;
using Marten;
using Marten.Services;

namespace deftq.Catalog.Infrastructure.DemoData
{
    public class DemoMaterials : IDemoDataProvider
    {
        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            await ImportDemoMaterials(documentStore, cancellation);
        }

#pragma warning disable MA0051
#pragma warning disable CA1031
        private static async Task ImportDemoMaterials(IDocumentStore documentStore, CancellationToken cancellation)
        {
            await using var session = await documentStore.OpenSessionAsync(new SessionOptions(), cancellation);

            var emptySupplementOperations = new List<SupplementOperation>();

            await UpsertMaterial(session, "8D0A963B-C80E-41F9-A732-CF79D812641F", "5703302157230", "Fuga ramme Soft 1M HV 560D6010",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(3), OperationTime.FromOneThousandthMinutes(231), emptySupplementOperations)
                });

            await UpsertOperation(session, Guid.Parse("74e023b3-4ceb-4982-a2a0-288d0b1e1553"), String.Empty,
                "Tilledning tom 3 x tom 1,5# for belysn./brugsgenst.", OperationTime.FromOneThousandthMinutes(3057));

            await UpsertOperation(session, Guid.Parse("bb702969-8467-40a6-87af-e6f3e43c2715"), String.Empty,
                "Tilledning 4 x tom 1,5# for belysn./brugsgenst.", OperationTime.FromOneThousandthMinutes(3100));

            await UpsertOperation(session, Guid.Parse("93f1fad5-09ce-484b-944a-f59ef680a724"), String.Empty,
                "Tilledning 5 x tom 1,5# for belysn./brugsgenst.", OperationTime.FromOneThousandthMinutes(3713));

            await UpsertMaterial(session, "C6941EDA-001A-4740-A16B-BA739E576969", "5703302153492", "Fuga lampeudtag 4-l+j HV 509D6015",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(3), OperationTime.FromOneThousandthMinutes(4821),
                        new List<SupplementOperation>
                        {
                            SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("74e023b3-4ceb-4982-a2a0-288d0b1e1552")),
                                OperationId.Create(Guid.Parse("74e023b3-4ceb-4982-a2a0-288d0b1e1553")), SupplementOperationType.UnitRelated()),
                            SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("bb702969-8467-40a6-87af-e6f3e43c2714")),
                                OperationId.Create(Guid.Parse("bb702969-8467-40a6-87af-e6f3e43c2715")), SupplementOperationType.UnitRelated()),
                            SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("93f1fad5-09ce-484b-944a-f59ef680a723")),
                                OperationId.Create(Guid.Parse("93f1fad5-09ce-484b-944a-f59ef680a724")), SupplementOperationType.UnitRelated())
                        })
                });

            await UpsertMaterial(session, "3227EDDC-88A2-47E0-AC7D-DF62E8D33FC3", "5703302157803", "Fuga teknisk montageramme 1M 560D0010",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(3), OperationTime.FromOneThousandthMinutes(276), emptySupplementOperations)
                });

            await UpsertOperation(session, Guid.Parse("68f32c9e-acf5-4ce7-b7f4-80c9b22c1c05"), String.Empty,
                "Udskiftning til afbr.skruer, hvor dette er aftalt. Pr. afbr/stikk m.v.", OperationTime.FromOneThousandthMinutes(552));

            await UpsertMaterial(session, "8D448110-C45A-4522-8C7F-AF68FC53FB5A", "5703302159401", "Fuga afbryder 1-pol U.tg.542D0001",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(3), OperationTime.FromOneThousandthMinutes(3494),
                        new List<SupplementOperation>
                        {
                            SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("413A7826-920A-48EC-99FF-241ED9F4CEF0")),
                                OperationId.Create(Guid.Parse("68f32c9e-acf5-4ce7-b7f4-80c9b22c1c05")), SupplementOperationType.AmountRelated())
                        })
                });

            await UpsertMaterial(session, "B3806AD5-B312-4EC7-A9E1-55B295751D30", "9010459172754", "Plastrør PR 16 mm", MaterialUnit.Meter(),
                new List<Mounting> { });

            await UpsertOperation(session, Guid.Parse("51bcfa70-de5c-457d-b644-4a3f45a0f9d5"), String.Empty,
                "I/på føringsvej type 2, strips", OperationTime.FromOneThousandthMinutes(739));

            await UpsertOperation(session, Guid.Parse("d4e346f8-9f7a-46a3-81e7-3440d33f6a55"), String.Empty,
                "I/på føringsvej type 3, polbøjler/X-strips", OperationTime.FromOneThousandthMinutes(1025));

            await UpsertMaterial(session, "E0E4F6C4-98CF-473E-AD77-C1391ED1E0B3", "5702950197261", "NOIKLX 90° 5G1,5mm2", MaterialUnit.Meter(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(10), OperationTime.FromOneThousandthMinutes(2486), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(11), OperationTime.FromOneThousandthMinutes(894), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(12), OperationTime.FromOneThousandthMinutes(207), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(14), OperationTime.FromOneThousandthMinutes(475), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(15), OperationTime.FromOneThousandthMinutes(475), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(16), OperationTime.FromOneThousandthMinutes(3432), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(17), OperationTime.FromOneThousandthMinutes(4573), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(18), OperationTime.FromOneThousandthMinutes(475), new List<SupplementOperation>
                    {
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("51bcfa70-de5c-457d-b644-4a3f45a0f9d4")),
                            OperationId.Create(Guid.Parse("51bcfa70-de5c-457d-b644-4a3f45a0f9d5")), SupplementOperationType.UnitRelated()),
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("d4e346f8-9f7a-46a3-81e7-3440d33f6a54")),
                            OperationId.Create(Guid.Parse("d4e346f8-9f7a-46a3-81e7-3440d33f6a55")), SupplementOperationType.UnitRelated())
                    })
                });

            var masterMaterialId = Guid.Parse("E497686B-06DB-4E30-8226-EC7473BE25EA");
            var masterMaterialEan = EanNumber.Create("7330268020636");
            await UpsertMaterial(session, "73f45dd5-035e-4da6-94ca-7d76e17e4013", "3606481320155", "Actassi 500 kabel kat 6A F/UTP 4par 500m grøn",
                MaterialUnit.Meter(),
                new List<Mounting>(), MaterialReference.Create(MaterialId.Create(masterMaterialId), masterMaterialEan));

            await UpsertOperation(session, Guid.Parse("b097cc54-a44d-4df0-a282-672da7c93881"), String.Empty,
                "Fastgjort  med strips", OperationTime.FromOneThousandthMinutes(739));
            await UpsertOperation(session, Guid.Parse("dc708be5-ae4f-4add-820c-13ae27c8e5f1"), String.Empty,
                "Fastgjort på træ med clips", OperationTime.FromOneThousandthMinutes(187));
            await UpsertOperation(session, Guid.Parse("58e6b8ca-bf42-420f-9d48-8fd13d51419b"), String.Empty,
                "Fastgjort på træ med bøjler", OperationTime.FromOneThousandthMinutes(538));
            await UpsertOperation(session, Guid.Parse("f879b707-7f7d-40dd-83ac-9cae7783f83a"), String.Empty,
                "Fastgjort på tegl med clips", OperationTime.FromOneThousandthMinutes(407));
            await UpsertOperation(session, Guid.Parse("4ac61e73-b011-4e54-aa97-af1325a6c6fd"), String.Empty,
                "Fastgjort på tegl med bøjler", OperationTime.FromOneThousandthMinutes(1201));
            await UpsertOperation(session, Guid.Parse("9b4071f8-4d13-45bd-b3b4-d1c4ce113d97"), String.Empty,
                "Fastgjort på jern t.o.m. 2mm,  med bøjler", OperationTime.FromOneThousandthMinutes(0));
            await UpsertOperation(session, Guid.Parse("471c7022-0741-45ae-915b-d14605a28c67"), String.Empty,
                "Fastgjort på jern over 2mm,  med bøjler, excl. evt. gevind", OperationTime.FromOneThousandthMinutes(538));
            await UpsertOperation(session, Guid.Parse("de1d283f-647a-4a30-b19d-e4951f233c67"), String.Empty,
                "Fastgjort på beton med clips", OperationTime.FromOneThousandthMinutes(400));
            await UpsertOperation(session, Guid.Parse("79bc7b37-864b-47e2-a25b-4e3eb138dc7b"), String.Empty,
                "Fastgjort på jern t.o.m. 2mm,  med bøjler", OperationTime.FromOneThousandthMinutes(0));

            await UpsertMaterial(session, masterMaterialId.ToString(), masterMaterialEan.Value, "KABEL KAT5E F/UTP 4P 305M DCA", MaterialUnit.Meter(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(11), OperationTime.FromOneThousandthMinutes(283), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(14), OperationTime.FromOneThousandthMinutes(196), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(15), OperationTime.FromOneThousandthMinutes(196), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(16), OperationTime.FromOneThousandthMinutes(3154), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(17), OperationTime.FromOneThousandthMinutes(4295), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(18), OperationTime.FromOneThousandthMinutes(196),
                        new List<SupplementOperation>
                        {
                            SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("b097cc54-a44d-4df0-a282-672da7c93880")),
                                OperationId.Create(Guid.Parse("b097cc54-a44d-4df0-a282-672da7c93881")), SupplementOperationType.UnitRelated())
                        }),
                    Mounting.Create(MountingCode.FromCode(19), OperationTime.FromOneThousandthMinutes(239), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(30), OperationTime.FromOneThousandthMinutes(2762), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(32), OperationTime.FromOneThousandthMinutes(1359), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(33), OperationTime.FromOneThousandthMinutes(196), new List<SupplementOperation>
                    {
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("dc708be5-ae4f-4add-820c-13ae27c8e5f0")),
                            OperationId.Create(Guid.Parse("dc708be5-ae4f-4add-820c-13ae27c8e5f1")), SupplementOperationType.UnitRelated()),
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("58e6b8ca-bf42-420f-9d48-8fd13d51419a")),
                            OperationId.Create(Guid.Parse("58e6b8ca-bf42-420f-9d48-8fd13d51419b")), SupplementOperationType.UnitRelated())
                    }),
                    Mounting.Create(MountingCode.FromCode(60), OperationTime.FromOneThousandthMinutes(5416), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(62), OperationTime.FromOneThousandthMinutes(2236), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(63), OperationTime.FromOneThousandthMinutes(196), new List<SupplementOperation>
                    {
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("f879b707-7f7d-40dd-83ac-9cae7783f839")),
                            OperationId.Create(Guid.Parse("f879b707-7f7d-40dd-83ac-9cae7783f83a")), SupplementOperationType.UnitRelated()),
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("4ac61e73-b011-4e54-aa97-af1325a6c6fc")),
                            OperationId.Create(Guid.Parse("4ac61e73-b011-4e54-aa97-af1325a6c6fd")), SupplementOperationType.UnitRelated())
                    }),
                    Mounting.Create(MountingCode.FromCode(70), OperationTime.FromOneThousandthMinutes(9094), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(71), OperationTime.FromOneThousandthMinutes(11382), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(73), OperationTime.FromOneThousandthMinutes(196), new List<SupplementOperation>
                    {
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("9b4071f8-4d13-45bd-b3b4-d1c4ce113d96")),
                            OperationId.Create(Guid.Parse("9b4071f8-4d13-45bd-b3b4-d1c4ce113d97")), SupplementOperationType.UnitRelated()),
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("471c7022-0741-45ae-915b-d14605a28c66")),
                            OperationId.Create(Guid.Parse("471c7022-0741-45ae-915b-d14605a28c67")), SupplementOperationType.UnitRelated())
                    }),
                    Mounting.Create(MountingCode.FromCode(80), OperationTime.FromOneThousandthMinutes(5361), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(82), OperationTime.FromOneThousandthMinutes(2212), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(83), OperationTime.FromOneThousandthMinutes(196), new List<SupplementOperation>
                    {
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("de1d283f-647a-4a30-b19d-e4951f233c66")),
                            OperationId.Create(Guid.Parse("de1d283f-647a-4a30-b19d-e4951f233c67")), SupplementOperationType.UnitRelated()),
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("79bc7b37-864b-47e2-a25b-4e3eb138dc7a")),
                            OperationId.Create(Guid.Parse("79bc7b37-864b-47e2-a25b-4e3eb138dc7b")), SupplementOperationType.UnitRelated())
                    })
                });

            await UpsertMaterial(session, "4b2881b1-b60f-4d04-8916-ef4487a5b3e1", "3606485122960", "Actassi S1 støvlåg m.farvekode f.patchpanel rød",
                MaterialUnit.Piece(),
                new List<Mounting> { });

            await UpsertMaterial(session, "a215b37b-40e3-4daa-9606-d6389b7798cd", "3606485122953", "Actassi S1 støvlåg m.farvekode f.patchpanel grøn",
                MaterialUnit.Piece(),
                new List<Mounting> { });

            masterMaterialId = Guid.Parse("827538A3-D6BB-4943-BE8E-CD6F0708D346");
            masterMaterialEan = EanNumber.Create("4012591114376");
            await UpsertMaterial(session, "175e56ae-7063-498d-a0b0-9d040fe3492a", "4012591603320", "Dåse KVIK 2,5 hvid m. klemmer",
                MaterialUnit.Piece(),
                new List<Mounting>(), MaterialReference.Create(MaterialId.Create(masterMaterialId), masterMaterialEan));

            await UpsertOperation(session, Guid.Parse("1312c55c-2e40-41c2-9b41-0806f3643484"), String.Empty,
                "2 kabler i membr.-/forskr.dåse, t.o.m 5x2,5#", OperationTime.FromOneThousandthMinutes(5303));
            await UpsertOperation(session, Guid.Parse("a4c7def6-f98f-423a-8e05-39039c304da5"), String.Empty,
                "3 kabler i membr.-/forskr.dåse, t.o.m 5x2,5#", OperationTime.FromOneThousandthMinutes(7783));
            await UpsertOperation(session, Guid.Parse("c65de825-3ca3-49e5-87e2-75be1d95da6f"), String.Empty,
                "4 kabler i membr.-/forskr.dåse, t.o.m 5x2,5#", OperationTime.FromOneThousandthMinutes(11311));
            await UpsertOperation(session, Guid.Parse("26bed913-5058-44b9-9dcc-3247c9add7fb"), String.Empty,
                "5 kabler i membr.-/forskr.dåse, t.o.m 5x2,5#", OperationTime.FromOneThousandthMinutes(18204));
            await UpsertOperation(session, Guid.Parse("6331e655-057b-424a-866c-8d9b954bb376"), String.Empty,
                "1 kabel t.o.m. 5x2,5# afsluttet i membr.-/forskr.dåse", OperationTime.FromOneThousandthMinutes(2202));
            await UpsertOperation(session, Guid.Parse("4c5227e5-4a92-4627-8e4f-8e35354c22ec"), String.Empty,
                "Ledn. forb ud over 5 pr. kabel: Pr. leder", OperationTime.FromOneThousandthMinutes(1292));
            await UpsertOperation(session, Guid.Parse("6f7b9ad1-e8d0-4ad5-8e5c-bb5fbb36b600"), String.Empty,
                "Tilledning tom 3x tom 1,5# for belysn./brugsgenst.", OperationTime.FromOneThousandthMinutes(3057));
            await UpsertOperation(session, Guid.Parse("60d7ee38-f166-40d5-9633-12e8bfd42c02"), String.Empty,
                "Tilledning 4 x tom 1,5# for belysn./brugsgenst.", OperationTime.FromOneThousandthMinutes(3100));
            await UpsertOperation(session, Guid.Parse("3ec351a7-8a42-4422-9263-74ff28ce3e40"), String.Empty,
                "Tilledning 5 x tom 1,5# for belysn./brugsgenst.", OperationTime.FromOneThousandthMinutes(3713));

            var supplementOperations = new List<SupplementOperation>
            {
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("1312c55c-2e40-41c2-9b41-0806f3643483")),
                    OperationId.Create(Guid.Parse("1312c55c-2e40-41c2-9b41-0806f3643484")), SupplementOperationType.AmountRelated()),
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("a4c7def6-f98f-423a-8e05-39039c304da4")),
                    OperationId.Create(Guid.Parse("a4c7def6-f98f-423a-8e05-39039c304da5")), SupplementOperationType.AmountRelated()),
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("c65de825-3ca3-49e5-87e2-75be1d95da6e")),
                    OperationId.Create(Guid.Parse("c65de825-3ca3-49e5-87e2-75be1d95da6f")), SupplementOperationType.AmountRelated()),
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("26bed913-5058-44b9-9dcc-3247c9add7fa")),
                    OperationId.Create(Guid.Parse("26bed913-5058-44b9-9dcc-3247c9add7fb")), SupplementOperationType.AmountRelated()),
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("6331e655-057b-424a-866c-8d9b954bb375")),
                    OperationId.Create(Guid.Parse("6331e655-057b-424a-866c-8d9b954bb376")), SupplementOperationType.AmountRelated()),
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("4c5227e5-4a92-4627-8e4f-8e35354c22eb")),
                    OperationId.Create(Guid.Parse("4c5227e5-4a92-4627-8e4f-8e35354c22ec")), SupplementOperationType.AmountRelated()),
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("6f7b9ad1-e8d0-4ad5-8e5c-bb5fbb36b599")),
                    OperationId.Create(Guid.Parse("6f7b9ad1-e8d0-4ad5-8e5c-bb5fbb36b600")), SupplementOperationType.AmountRelated()),
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("60d7ee38-f166-40d5-9633-12e8bfd42c01")),
                    OperationId.Create(Guid.Parse("60d7ee38-f166-40d5-9633-12e8bfd42c02")), SupplementOperationType.AmountRelated()),
                SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("3ec351a7-8a42-4422-9263-74ff28ce3e39")),
                    OperationId.Create(Guid.Parse("3ec351a7-8a42-4422-9263-74ff28ce3e40")), SupplementOperationType.AmountRelated())
            };

            await UpsertMaterial(session, masterMaterialId.ToString(), masterMaterialEan.Value, "*MEMBRANDÅSE 2,5mm2 DE9330 GRÅ DE",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(25), OperationTime.FromOneThousandthMinutes(2698), supplementOperations),
                    Mounting.Create(MountingCode.FromCode(30), OperationTime.FromOneThousandthMinutes(2357), supplementOperations),
                    Mounting.Create(MountingCode.FromCode(60), OperationTime.FromOneThousandthMinutes(3020), supplementOperations),
                    Mounting.Create(MountingCode.FromCode(70), OperationTime.FromOneThousandthMinutes(3679), supplementOperations),
                    Mounting.Create(MountingCode.FromCode(71), OperationTime.FromOneThousandthMinutes(4512), supplementOperations),
                    Mounting.Create(MountingCode.FromCode(80), OperationTime.FromOneThousandthMinutes(3008), supplementOperations)
                });

            await UpsertMaterial(session, "e0c6722b-e6dd-430f-87f2-2d9474c2814b", "5702950197339", "NOIKLX 90° 3G2,5mm2", MaterialUnit.Meter(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(10), OperationTime.FromOneThousandthMinutes(2486), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(11), OperationTime.FromOneThousandthMinutes(894), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(12), OperationTime.FromOneThousandthMinutes(207), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(14), OperationTime.FromOneThousandthMinutes(475), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(15), OperationTime.FromOneThousandthMinutes(475), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(16), OperationTime.FromOneThousandthMinutes(3432), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(17), OperationTime.FromOneThousandthMinutes(4573), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(18), OperationTime.FromOneThousandthMinutes(475), new List<SupplementOperation>
                    {
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("51bcfa70-de5c-457d-b644-4a3f45a0f9d3")),
                            OperationId.Create(Guid.Parse("51bcfa70-de5c-457d-b644-4a3f45a0f9d5")), SupplementOperationType.AmountRelated()),
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("d4e346f8-9f7a-46a3-81e7-3440d33f6a53")),
                            OperationId.Create(Guid.Parse("d4e346f8-9f7a-46a3-81e7-3440d33f6a55")), SupplementOperationType.AmountRelated())
                    })
                });

            await UpsertMaterial(session, "1deffed4-0550-4ebb-b8fc-783389ac83e1", "9010459173034", "Plastrør HFIR 16 mm PVC-FRI",
                MaterialUnit.Meter(),
                new List<Mounting> { });

            masterMaterialId = Guid.Parse("ADDE7402-B530-4757-8505-13DB17E62D77");
            masterMaterialEan = EanNumber.Create("4024092025792");
            await UpsertMaterial(session, "5d00d67a-da06-4a30-a3f9-3a70cf38b16f", "4024092025778", "Forskruning perf. M25x1,5 11-17 LG",
                MaterialUnit.Piece(),
                new List<Mounting>(), MaterialReference.Create(MaterialId.Create(masterMaterialId), masterMaterialEan));

            await UpsertMaterial(session, masterMaterialId.ToString(), masterMaterialEan.Value, "Forskruning perf. M25x1,5 11-17 Grå",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(18), OperationTime.FromOneThousandthMinutes(1750), emptySupplementOperations)
                });

            await UpsertMaterial(session, "34e19c8d-4e04-4100-8ee3-33f8d065e3c2", "5999099005162", "Gummikabel 5G1,5 H07RN-F", MaterialUnit.Meter(),
                new List<Mounting> { });

            masterMaterialId = Guid.Parse("54CE3249-0D8C-480D-B595-1AC7F5E772C9");
            masterMaterialEan = EanNumber.Create("5703302099714");
            await UpsertMaterial(session, "00435972-7e16-4970-98c1-8f0ae2b91adc", "5703302099707", "Fuga tangent f.stikkontakt u.a. 1,5M LG 530D5912",
                MaterialUnit.Piece(),
                new List<Mounting>(), MaterialReference.Create(MaterialId.Create(masterMaterialId), masterMaterialEan));

            await UpsertMaterial(session, masterMaterialId.ToString(), masterMaterialEan.Value, "Fuga tangent f. stikkontakt u.a 1,5M HV 530D6912",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(18), OperationTime.FromOneThousandthMinutes(3354), emptySupplementOperations)
                });

            await UpsertOperation(session, Guid.Parse("29af94a1-d427-4284-a0f7-c75403c7ee62"), String.Empty,
                "Hul for 1½M  i 2 lag gips", OperationTime.FromOneThousandthMinutes(1490));
            await UpsertOperation(session, Guid.Parse("089fb02d-feaa-4165-86da-c2833cde14fd"), String.Empty,
                "Hul for 1½M  i 3 lag gips", OperationTime.FromOneThousandthMinutes(2233));

            await UpsertMaterial(session, "c184b312-f0ce-4046-8aac-a3b971f930ae", "5703302156097", "Fuga Air forfradåse blå 1,5M 504D3015",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(03), OperationTime.FromOneThousandthMinutes(3113),
                        new List<SupplementOperation>
                        {
                            SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("29af94a1-d427-4284-a0f7-c75403c7ee61")),
                                OperationId.Create(Guid.Parse("29af94a1-d427-4284-a0f7-c75403c7ee62")), SupplementOperationType.AmountRelated()),
                            SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("089fb02d-feaa-4165-86da-c2833cde14fc")),
                                OperationId.Create(Guid.Parse("089fb02d-feaa-4165-86da-c2833cde14fd")), SupplementOperationType.AmountRelated())
                        })
                });

            await UpsertMaterial(session, "11da40e9-3cb5-44c7-9324-68b363b2a886", "6417019130255", "Sikkerhedsafbr 3P 36A/400V OTP m/hj. 4xM32+2xM16",
                MaterialUnit.Piece(),
                new List<Mounting> { });

            masterMaterialId = Guid.Parse("1E630E64-9B5F-4581-902B-D0F8FA1CAF63");
            masterMaterialEan = EanNumber.Create("4024092002106");
            await UpsertMaterial(session, "30956732-0fc7-4b6b-9af4-ea0c50897d69", "4024092001093", "Møtrik M32x1,5  Messing", MaterialUnit.Piece(),
                new List<Mounting>(), MaterialReference.Create(MaterialId.Create(masterMaterialId), masterMaterialEan));

            await UpsertMaterial(session, masterMaterialId.ToString(), masterMaterialEan.Value, "MØTRIK M32X11,5 PA/7001", MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(18), OperationTime.FromOneThousandthMinutes(3191), emptySupplementOperations)
                });

            await UpsertMaterial(session, "8e56e012-d0a7-465a-aa08-403505bd1645", "5999099003779", "Gummikabel 4G6,0 H07RN-F", MaterialUnit.Meter(),
                new List<Mounting> { });

            await UpsertMaterial(session, "87f8652f-43fd-488c-89cc-4331c95c3fec", "5702950198121", "NOIKLX 90° 5G6mm2", MaterialUnit.Meter(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(11), OperationTime.FromOneThousandthMinutes(1127), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(12), OperationTime.FromOneThousandthMinutes(509), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(14), OperationTime.FromOneThousandthMinutes(678), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(15), OperationTime.FromOneThousandthMinutes(678), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(16), OperationTime.FromOneThousandthMinutes(3636), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(17), OperationTime.FromOneThousandthMinutes(4776), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(18), OperationTime.FromOneThousandthMinutes(678), new List<SupplementOperation>
                    {
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("51bcfa70-de5c-457d-b644-4a3f45a0f9d2")),
                            OperationId.Create(Guid.Parse("51bcfa70-de5c-457d-b644-4a3f45a0f9d5")), SupplementOperationType.AmountRelated()),
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("d4e346f8-9f7a-46a3-81e7-3440d33f6a52")),
                            OperationId.Create(Guid.Parse("d4e346f8-9f7a-46a3-81e7-3440d33f6a55")), SupplementOperationType.AmountRelated())
                    })
                });
            
            await UpsertMaterial(session, "a6ae35a2-7fe0-4623-9d70-3b7e870216ef", "5703302099059", "Fuga underlag 1,5M LG 503D5615",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(25), OperationTime.FromOneThousandthMinutes(2293), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(30), OperationTime.FromOneThousandthMinutes(1953), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(60), OperationTime.FromOneThousandthMinutes(2616), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(70), OperationTime.FromOneThousandthMinutes(3275), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(71), OperationTime.FromOneThousandthMinutes(4108), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(80), OperationTime.FromOneThousandthMinutes(2604), emptySupplementOperations)
                });

            await UpsertMaterial(session, "9cc402fe-1a9c-4b9c-8cd1-ece8723a9859", "5703472007632", "Lågudsnit 1E/BR 170 PH", MaterialUnit.Piece(),
                new List<Mounting> { });

            await UpsertMaterial(session, "ea61ba92-5173-49fc-8d8a-445a5c259e4d", "5703302154147", "Fuga afbryder Jalousi HV 542D6004",
                MaterialUnit.Piece(),
                new List<Mounting> { });

            masterMaterialId = Guid.Parse("7C9CCADC-DAC7-4EFB-A958-FDD65126512B");
            masterMaterialEan = EanNumber.Create("8697606300942");
            await UpsertMaterial(session, "347fbcc4-2051-4ea0-b752-16b1809085b8", "4024092116131",
                "Forskruning m. afl. krallen M20x1,5 10-12 polyamid", MaterialUnit.Piece(),
                new List<Mounting>(), MaterialReference.Create(MaterialId.Create(masterMaterialId), masterMaterialEan));

            await UpsertMaterial(session, masterMaterialId.ToString(), masterMaterialEan.Value, "FORSKRUNING M20 BIMED BM-12 13878",
                MaterialUnit.Piece(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(18), OperationTime.FromOneThousandthMinutes(1567), emptySupplementOperations)
                });

            await UpsertMaterial(session, "47800796-29fe-4c12-9069-fd6cb5ca4856", "5705150119002", "PEH flexrør korr-glat 12-040/031 40mm",
                MaterialUnit.Meter(),
                new List<Mounting>
                {
                    Mounting.Create(MountingCode.FromCode(14), OperationTime.FromOneThousandthMinutes(532), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(15), OperationTime.FromOneThousandthMinutes(532), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(16), OperationTime.FromOneThousandthMinutes(2011), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(17), OperationTime.FromOneThousandthMinutes(2581), emptySupplementOperations),
                    Mounting.Create(MountingCode.FromCode(18), OperationTime.FromOneThousandthMinutes(532), new List<SupplementOperation>
                    {
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("51bcfa70-de5c-457d-b644-4a3f45a0f9d1")),
                            OperationId.Create(Guid.Parse("51bcfa70-de5c-457d-b644-4a3f45a0f9d5")), SupplementOperationType.AmountRelated()),
                        SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("d4e346f8-9f7a-46a3-81e7-3440d33f6a51")),
                            OperationId.Create(Guid.Parse("d4e346f8-9f7a-46a3-81e7-3440d33f6a55")), SupplementOperationType.AmountRelated())
                    })
                });

            await session.SaveChangesAsync(cancellation);
        }
#pragma warning restore MA0051
#pragma warning restore CA1031
        private static async Task UpsertMaterial(IDocumentSession session, string guid, string ean, string name, MaterialUnit unit,
            List<Mounting> mountings)
        {
            await UpsertMaterial(session, guid, ean, name, unit, mountings, MaterialReference.Empty());
        }

        private static Task UpsertMaterial(IDocumentSession session, string guid, string ean, string name, MaterialUnit unit,
            List<Mounting> mountings, MaterialReference masterMaterial)
        {
            var material = Material.Create(MaterialId.Create(Guid.Parse(guid)), EanNumber.Create(ean), MaterialName.Create("TEST: " + name), unit,
                mountings, masterMaterial, MaterialReference.Empty(), MaterialPublished.Empty());

            session.Delete(material);
            session.Store(material);

            return Task.CompletedTask;
        }

        private static Task UpsertOperation(IDocumentSession session, Guid operationId, string operationNumber, string operationText,
            OperationTime operationTime)
        {
            var operation = Operation.Create(OperationId.Create(operationId), OperationNumber.Create(operationNumber),
                OperationText.Create(operationText), operationTime);

            session.Delete(operation);
            session.Store(operation);

            return Task.CompletedTask;
        }
    }
}
