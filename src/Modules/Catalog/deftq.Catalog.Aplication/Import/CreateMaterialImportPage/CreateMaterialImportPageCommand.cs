using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.DataAccess;
using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Catalog.Domain.MaterialImport;
using deftq.Catalog.Domain.OperationCatalog;
using deftq.Catalog.Domain.Time;
using Microsoft.Extensions.Logging;

namespace deftq.Catalog.Application.Import.CreateMaterialImportPage
{
    public sealed class CreateMaterialImportPageCommand : ICommand<ICommandResponse>
    {
        public DateTimeOffset Published { get; }
        public int StartRow { get; }
        public int PageSize { get; }
        public bool IsLastPage { get; }
        public IList<ImportMaterial> Content { get; }

        private CreateMaterialImportPageCommand(DateTimeOffset published, int startRow, int pageSize, bool isLastPage, IList<ImportMaterial> content)
        {
            Published = published;
            StartRow = startRow;
            PageSize = pageSize;
            IsLastPage = isLastPage;
            Content = content;
        }

        public static CreateMaterialImportPageCommand Create(DateTimeOffset published, int startRow, int pageSize, bool lastPage,
            IList<ImportMaterial> content)
        {
            return new CreateMaterialImportPageCommand(published, startRow, pageSize, lastPage, content);
        }
    }

    public class ImportMaterial
    {
        public string Ean { get; }
        public string Name { get; }
        public string Unit { get; }
        public IList<ImportMounting> Mountings { get; }

        public ImportMaterial(string ean, string name, string unit, IList<ImportMounting> mountings)
        {
            Ean = ean;
            Name = name;
            Unit = unit;
            Mountings = mountings;
        }
    }

    public class ImportMounting
    {
        public int MountingCode { get; }
        public int OperationTimeMs { get; }
        public IList<ImportSupplementOperation> SupplementOperations { get; }

        public ImportMounting(int mountingCode, int operationTimeMs, IList<ImportSupplementOperation> supplementOperations)
        {
            MountingCode = mountingCode;
            OperationTimeMs = operationTimeMs;
            SupplementOperations = supplementOperations;
        }
    }

    public class ImportSupplementOperation
    {
        public string OperationNumber { get; }
        public string Text { get; }
        public int OperationTimeMs { get; }
        public string Type { get; }

        public ImportSupplementOperation(string operationNumber, string text, int operationTimeMs, string type)
        {
            OperationNumber = operationNumber;
            Text = text;
            OperationTimeMs = operationTimeMs;
            Type = type;
        }
    }

    internal class CreateMaterialImportPageCommandHandler : ICommandHandler<CreateMaterialImportPageCommand, ICommandResponse>
    {
        private readonly IMaterialImportPageRepository _importPageRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateMaterialImportPageCommand> _logger;

        public CreateMaterialImportPageCommandHandler(IMaterialImportPageRepository importPageRepository, IMaterialRepository materialRepository,
            IUnitOfWork unitOfWork, ILogger<CreateMaterialImportPageCommand> logger)
        {
            _importPageRepository = importPageRepository;
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ICommandResponse> Handle(CreateMaterialImportPageCommand notification, CancellationToken cancellationToken)
        {
            var materials = await Task.WhenAll(notification.Content.Select(m => MapMaterial(notification.Published, m, cancellationToken)));
            var materialImportPage = MaterialImportPage.Create(MaterialImportPageId.Create(Guid.NewGuid()), notification.Published,
                notification.StartRow, notification.PageSize, notification.IsLastPage, materials);

            await _importPageRepository.Add(materialImportPage, cancellationToken);
            await _importPageRepository.SaveChanges(cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        private async Task<Material> MapMaterial(DateTimeOffset published, ImportMaterial importImportMaterial, CancellationToken cancellationToken)
        {
            var existingMaterial = await _materialRepository.FindByEanNumber(importImportMaterial.Ean, cancellationToken);

            var materialId = existingMaterial?.MaterialId ?? MaterialId.Create(Guid.NewGuid());
            var eanNumber = EanNumber.Create(importImportMaterial.Ean);
            var materialName = MaterialName.Create(importImportMaterial.Name);
            var materialUnit = MaterialUnit.FromString(importImportMaterial.Unit);
            var mountings = MapMountings(importImportMaterial, existingMaterial);
            var refNone = MaterialReference.Empty();
            var catalogPublished = MaterialPublished.Create(published);

            return Material.Create(materialId, eanNumber, materialName, materialUnit, mountings, refNone, refNone, catalogPublished);
        }

        private static IList<Mounting> MapMountings(ImportMaterial importImportMaterial, Material? existingMaterial)
        {
            return importImportMaterial.Mountings.Select(m =>
            {
                var existingMounting = existingMaterial?.Mountings.FirstOrDefault(x => x.MountingCode.Code == m.MountingCode);
                var mountingCode = MountingCode.FromCode(m.MountingCode);
                var operationTime = OperationTime.Create(m.OperationTimeMs);
                var supplementOperations = m.SupplementOperations.Select(op =>
                {
                    var existingOperation = existingMounting?.SupplementOperations.FirstOrDefault(x =>
                        string.Equals(x.OperationNumber.Value, op.OperationNumber, StringComparison.OrdinalIgnoreCase));

                    var supplementOperationId = existingOperation?.SupplementOperationId ?? SupplementOperationId.Create(Guid.NewGuid());
                    var operationNumber = OperationNumber.Create(op.OperationNumber);
                    var operationText = OperationText.Create(op.Text);
                    var operationType = SupplementOperationType.FromString(op.Type);
                    var opOperationTime = OperationTime.Create(op.OperationTimeMs);

                    return SupplementOperation.Create(supplementOperationId, operationNumber, operationText, opOperationTime, operationType);
                }).ToList();
                return Mounting.Create(mountingCode, operationTime, supplementOperations);
            }).ToList();
        }
    }
}
