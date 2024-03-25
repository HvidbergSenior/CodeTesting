using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Integration;

namespace deftq.Pieceworks.Infrastructure.projectDocument
{
    public sealed class FilesystemFileReference : ValueObject, IFileReference, IExternalFileReference
    {
        public string Filename { get; private set; }

        private FilesystemFileReference()
        {
            Filename = String.Empty;
        }

        private FilesystemFileReference(string filename)
        {
            Filename = filename;
        }

        public static FilesystemFileReference Create(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Invalid filename", nameof(filename));
            }
            
            return new FilesystemFileReference(filename);
        }
    }
}
