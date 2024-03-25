using deftq.Pieceworks.Application.UploadDocument;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UploadDocument
{
    public class UploadDocumentValidatorTest
    {
        [Fact]
        public void WhenUploadingDocument_fileEndingIsAccepted()
        {
            using (var ms = new MemoryStream())
            {
                var validator = new UploadDocumentCommandValidator();
                
                Assert.True(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "aftaleseddel.pdf", ms)).IsValid);
                Assert.True(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "aftaleseddel.PDF", ms)).IsValid);
                Assert.True(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "aftaleseddel.PdF", ms)).IsValid);
                
                Assert.True(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "tegning.jpg", ms)).IsValid);
                Assert.True(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "tegning.jpeg", ms)).IsValid);
                Assert.True(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "tegning.png", ms)).IsValid);
                
                Assert.True(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "tegning", ms)).IsValid);
                
                Assert.False(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "tegning.exe", ms)).IsValid);
                Assert.False(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "tegning.apk", ms)).IsValid);
                Assert.False(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "tegning.ps1", ms)).IsValid);
                Assert.False(validator.Validate(UploadDocumentCommand.Create(Guid.NewGuid(), null, Guid.NewGuid(), "tegning.sh", ms)).IsValid);
            }
        }
    }
}
