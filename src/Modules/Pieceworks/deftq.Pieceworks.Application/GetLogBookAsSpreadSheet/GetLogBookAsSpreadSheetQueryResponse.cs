namespace deftq.Pieceworks.Application.GetLogBookAsSpreadSheet
{
    public class GetLogBookAsSpreadSheetQueryResponse
    {
        public string FileName { get; }

        private readonly byte[] _data;
        
        public GetLogBookAsSpreadSheetQueryResponse(byte[] data, string fileName)
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
