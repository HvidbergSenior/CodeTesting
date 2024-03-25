using deftq.Pieceworks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace deftq.Pieceworks.Application.GetWorkItemsAsSpreadSheet
{
    public sealed class OpenXmlExporter : AbstractWorkItemsExporter, IDisposable
    {
        private readonly BaseRateAndSupplement _baseRateAndSupplement;
        private readonly Exporter _exporter;
        private readonly IList<Cell> _headers;
        private readonly List<OptionalHeaderInfo> _optionalHeaders;

        public OpenXmlExporter(BaseRateAndSupplement baseRateAndSupplement)
        {
            _baseRateAndSupplement = baseRateAndSupplement;
            _exporter = new Exporter();
            _headers = new List<Cell>
            {
                new() { CellValue = new CellValue("Folder"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Hierarki"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Ekstra arbejde"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Tekst"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Montage kode"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Montage tekst"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Antal"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Direkte operationstid (ms)"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Arbejdstid (ms)"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Betaling (Dkr)"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Samlet tillæg"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Betalingsfaktor"), DataType = CellValues.String },
                new() { CellValue = new CellValue("Betalingsfaktor regulering"), DataType = CellValues.String },
            };

            _optionalHeaders = new List<OptionalHeaderInfo>();
        }

        private class OptionalHeaderInfo
        {
            public enum OptionalHeaderType { Operation, Supplement }

            public OptionalHeaderType HeaderType { get; }
            public string Id { get; }
            public string Name { get; }

            public OptionalHeaderInfo(OptionalHeaderType headerType, string id, string name)
            {
                HeaderType = headerType;
                Id = id;
                Name = name;
            }
        }

        protected override void VisitProject(Project project)
        {
            _exporter.AddSheet(project.ProjectName.Value);
        }

        protected override void VisitFolder(ProjectFolder folder)
        {
        }

        protected override void VisitWorkItem(ProjectFolder folder, WorkItem workItem)
        {
            var nameCell = new Cell { CellValue = new CellValue(folder.Name.Value), DataType = CellValues.String };
            var hierarchyCell = new Cell { CellValue = new CellValue(GetFolderHierarchy(folder)), DataType = CellValues.String };
            var extraWorkCell = new Cell { CellValue = new CellValue(folder.IsExtraWork()), DataType = CellValues.Boolean };

            var textCell = new Cell { CellValue = new CellValue(workItem.Text.Value), DataType = CellValues.String };

            var mountingCodeCell = workItem.IsMaterial()
                ? new Cell { CellValue = new CellValue(workItem.WorkItemMaterial!.MountingCode.MountingCode), DataType = CellValues.String }
                : new Cell() { CellValue = new CellValue(), DataType = CellValues.String };
            var mountingTextCell = workItem.IsMaterial()
                ? new Cell { CellValue = new CellValue(workItem.WorkItemMaterial!.MountingCode.Text), DataType = CellValues.String }
                : new Cell() { CellValue = new CellValue(), DataType = CellValues.String };
            var amountCell = new Cell { CellValue = new CellValue(workItem.Amount.Value), DataType = CellValues.Number };
            var operationTimeCell = new Cell { CellValue = new CellValue(workItem.OperationTime.Value), DataType = CellValues.Number };
            var workTimeCell = new Cell { CellValue = new CellValue(workItem.TotalWorkTime.Value), DataType = CellValues.Number };
            var paymentCell = new Cell { CellValue = new CellValue(workItem.TotalPayment.Value), DataType = CellValues.Number };

            var workItemCalculator = new WorkItemCalculator(new BaseRateAndSupplementProxy(_baseRateAndSupplement, folder));
            var combinedSupplement = workItemCalculator.CalculateCombinedSupplementPercentage(workItem.Date.Value).Evaluate().Value;
            var combinedSupplementCell = new Cell { CellValue = new CellValue(combinedSupplement), DataType = CellValues.Number };

            var baseRateCell = new Cell
            {
                CellValue = new CellValue(_baseRateAndSupplement.GetBaseRateInterval(workItem.Date.Value).BaseRate.Value),
                DataType = CellValues.Number
            };
            var baseRateRegulationCell = new Cell
            {
                CellValue = new CellValue(folder.FolderRateAndSupplement.BaseRateRegulation.Value), DataType = CellValues.Number
            };

            var additionalCells = UpdateAdditionalCells(workItem);

            var cells = new List<Cell>
            {
                nameCell,
                hierarchyCell,
                extraWorkCell,
                textCell,
                mountingCodeCell,
                mountingTextCell,
                amountCell,
                operationTimeCell,
                workTimeCell,
                paymentCell,
                combinedSupplementCell,
                baseRateCell,
                baseRateRegulationCell
            };

            _exporter.AddRow(cells.Concat(additionalCells).ToArray());
        }

        private IList<Cell> UpdateAdditionalCells(WorkItem workItem)
        {
            foreach (var supplement in workItem.Supplements)
            {
                var existingHeader = _optionalHeaders.FirstOrDefault(header =>
                    header.HeaderType == OptionalHeaderInfo.OptionalHeaderType.Supplement &&
                    string.Equals(header.Id, supplement.CatalogSupplementId.Value.ToString(), StringComparison.OrdinalIgnoreCase));

                if (existingHeader is null)
                {
                    _optionalHeaders.Add(new OptionalHeaderInfo(OptionalHeaderInfo.OptionalHeaderType.Supplement,
                        supplement.CatalogSupplementId.Value.ToString(), supplement.SupplementText.Value));
                }
            }

            var result = new List<Cell>();
            foreach (var header in _optionalHeaders)
            {
                if (header.HeaderType == OptionalHeaderInfo.OptionalHeaderType.Supplement)
                {
                    var supplement = workItem.Supplements.FirstOrDefault(suppl =>
                        string.Equals(suppl.CatalogSupplementId.Value.ToString(), header.Id, StringComparison.OrdinalIgnoreCase));
                    if (supplement is not null)
                    {
                        result.Add(new Cell { CellValue = new CellValue(supplement.SupplementPercentage.Value), DataType = CellValues.Number });
                    }
                    else
                    {
                        result.Add(new Cell { CellValue = new CellValue(""), DataType = CellValues.String });
                    }
                }
                else
                {
                    result.Add(new Cell { CellValue = new CellValue(""), DataType = CellValues.String });
                }
            }

            return result;
        }

        private string GetFolderHierarchy(ProjectFolder folder)
        {
            if (folder.ParentFolder is null)
            {
                return $"/{folder.Name.Value.Replace('/', '#')}";
            }

            return $"{GetFolderHierarchy(folder.ParentFolder)}/{folder.Name.Value.Replace('/', '#')}";
        }

        public override WorkItemsExport GetExport()
        {
            var additionalSupplementHeaders = _optionalHeaders.SelectMany(header => new List<Cell>
            {
                new() { CellValue = new CellValue(header.Name), DataType = CellValues.String },
            });
            _exporter.AddHeaderRow(_headers.Concat(additionalSupplementHeaders).ToArray());
            return new WorkItemsExport("opmålinger.xlsx", _exporter.GetData());
        }

        public void Dispose()
        {
            _exporter.Dispose();
        }

        private class Exporter : IDisposable
        {
            private readonly MemoryStream _memoryStream;
            private readonly SpreadsheetDocument _spreadsheetDocument;
            private SheetData? _sheetData;
            private uint _rowIndex = 2;

            public Exporter()
            {
                _memoryStream = new MemoryStream();
                _spreadsheetDocument = SpreadsheetDocument.Create(_memoryStream, SpreadsheetDocumentType.Workbook);

                var workBookPart = _spreadsheetDocument.AddWorkbookPart();
                workBookPart.Workbook = new Workbook();
            }

            public void AddSheet(string sheetName)
            {
                if (_spreadsheetDocument.WorkbookPart is null)
                {
                    throw new InvalidOperationException("spreadsheetDocument.WorkbookPart is null");
                }

                var sheets = _spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

                var worksheetPart = _spreadsheetDocument.WorkbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var relationshipIdPart = _spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart);
                var sheet = new Sheet() { Id = relationshipIdPart, SheetId = 1, Name = sheetName };

                sheets.Append(sheet);

                _sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
            }

            public void AddRow(params Cell[] cells)
            {
                if (_sheetData is null)
                {
                    throw new InvalidOperationException("sheetData is null");
                }

                var sheetRow = new Row { RowIndex = new UInt32Value(_rowIndex) };
                _sheetData.Append(sheetRow);

                for (int i = 0; i < cells.Length; i++)
                {
                    sheetRow.InsertAt(cells[i], i);
                }

                _rowIndex++;
            }

            public byte[] GetData()
            {
                _spreadsheetDocument.Dispose();
                return _memoryStream.ToArray();
            }

            public void Dispose()
            {
                _spreadsheetDocument.Dispose();
                _memoryStream.Dispose();
            }

            public void AddHeaderRow(params Cell[] cells)
            {
                if (_sheetData is null)
                {
                    throw new InvalidOperationException("sheetData is null");
                }

                var sheetRow = new Row { RowIndex = new UInt32Value((uint)1) };
                for (int i = 0; i < cells.Length; i++)
                {
                    sheetRow.InsertAt(cells[i], i);
                }

                _sheetData.PrependChild(sheetRow);
            }
        }
    }
}
