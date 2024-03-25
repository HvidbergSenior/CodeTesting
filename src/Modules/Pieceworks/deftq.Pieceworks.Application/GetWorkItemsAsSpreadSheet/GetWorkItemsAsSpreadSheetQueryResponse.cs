using System.Collections.ObjectModel;

namespace deftq.Pieceworks.Application.GetWorkItemsAsSpreadSheet
{
    public class GetWorkItemsAsSpreadSheetQueryResponse
    {
        public string Filename { get; }

        private readonly byte[] _data;
        
        public GetWorkItemsAsSpreadSheetQueryResponse(string filename, byte[] data)
        {
            Filename = filename;
            _data = data;
        }

        public byte[] GetBuffer()
        {
            return _data;
        }
    }
}
