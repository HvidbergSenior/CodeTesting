namespace deftq.Pieceworks.Domain.projectDocument
{
    public interface IFileReference
    {
        static IFileReference Empty()
        {
            return new EmptyFileReference();
        }
        
        private class EmptyFileReference : IFileReference
        {
            
        }
    }
}
