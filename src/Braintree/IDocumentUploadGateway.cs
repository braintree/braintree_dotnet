#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating documents
    /// </summary>
    public interface IDocumentUploadGateway
    {
        Result<DocumentUpload> Create(DocumentUploadRequest request);
        Task<Result<DocumentUpload>> CreateAsync(DocumentUploadRequest request);
    }
}

