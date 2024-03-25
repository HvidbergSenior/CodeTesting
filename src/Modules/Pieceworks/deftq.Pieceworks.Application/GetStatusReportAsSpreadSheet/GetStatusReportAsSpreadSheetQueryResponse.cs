namespace deftq.Pieceworks.Application.GetStatusReportAsSpreadSheet
{
    public class GetStatusReportAsSpreadSheetQueryResponse
    {
        public string FileName { get; }

        private readonly byte[] _data;
        
        public GetStatusReportAsSpreadSheetQueryResponse(byte[] data, string fileName)
        {
            _data = data;
            FileName = fileName;
        }

        public byte[] GetBuffer()
        {
            return _data;
        }
    }
}
