#pragma warning disable 1591

using System.Threading.Tasks;

namespace Braintree
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting disputes
    /// </summary>
    public interface IDisputeGateway
    {
        Result<Dispute> Accept(string disputeId);
        Task<Result<Dispute>> AcceptAsync(string disputeId);
        Result<DisputeEvidence> AddFileEvidence(string disputeId, string documentUploadId);
        Result<DisputeEvidence> AddFileEvidence(string disputeId, FileEvidenceRequest request);
        Task<Result<DisputeEvidence>> AddFileEvidenceAsync(string disputeId, string documentUploadId);
        Task<Result<DisputeEvidence>> AddFileEvidenceAsync(string disputeId, FileEvidenceRequest request);
        Result<DisputeEvidence> AddTextEvidence(string disputeId, string content);
        Result<DisputeEvidence> AddTextEvidence(string disputeId, TextEvidenceRequest request);
        Task<Result<DisputeEvidence>> AddTextEvidenceAsync(string disputeId, string content);
        Task<Result<DisputeEvidence>> AddTextEvidenceAsync(string disputeId, TextEvidenceRequest request);
        Result<Dispute> Finalize(string disputeId);
        Task<Result<Dispute>> FinalizeAsync(string disputeId);
        Result<Dispute> Find(string disputeId);
        Task<Result<Dispute>> FindAsync(string disputeId);
        Result<Dispute> RemoveEvidence(string disputeId, string evidenceId);
        Task<Result<Dispute>> RemoveEvidenceAsync(string disputeId, string evidenceId);
        PaginatedCollection<Dispute> Search(DisputeSearchRequest request);
    }
}
