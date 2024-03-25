using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Integration;

namespace deftq.Pieceworks.Infrastructure.projectDocument
{
    public sealed class BlobFileReference : ValueObject, IFileReference, IExternalFileReference
    {
        public string Container { get; private set; }
        public string Blob { get; private set; }

        public BlobFileReference()
        {
            Container = String.Empty;
            Blob = String.Empty;
        }

        private BlobFileReference(string container, string blob)
        {
            Container = container;
            Blob = blob;
        }

        public static BlobFileReference Create(string container, string blob)
        {
            if (string.IsNullOrWhiteSpace(container))
            {
                throw new ArgumentException("Invalid container", nameof(container));
            }
            
            if (string.IsNullOrWhiteSpace(blob))
            {
                throw new ArgumentException("Invalid blob", nameof(blob));
            }
            
            return new BlobFileReference(container, blob);
        }
    }
}
