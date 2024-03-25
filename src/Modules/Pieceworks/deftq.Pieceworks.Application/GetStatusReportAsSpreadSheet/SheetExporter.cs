using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet
{
    public class SheetExporter
    {
        private readonly SheetData _sheet;
        private uint _rowIndex;

        public SheetExporter(SheetData sheet, bool skipHeaderRow = false)
        {
            _sheet = sheet;
            _rowIndex = skipHeaderRow ? (uint)2 : 1;
        }
            
        public void AddRow(params Cell[] cells)
        {
            var sheetRow = new Row { RowIndex = new UInt32Value(_rowIndex) };
            _sheet.Append(sheetRow);

            for (int i = 0; i < cells.Length; i++)
            {
                sheetRow.InsertAt(cells[i], i);
            }

            _rowIndex++;
        }
    }
}
