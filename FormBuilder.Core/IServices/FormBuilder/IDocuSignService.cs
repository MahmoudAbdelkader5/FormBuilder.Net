using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    /// <summary>
    /// Abstraction for DocuSign e-sign integration.
    /// </summary>
    public interface IDocuSignService
    {
        /// <summary>
        /// Create and send a signing envelope for a specific submission stage.
        /// </summary>
        Task<bool> CreateSigningEnvelopeAsync(
            int submissionId,
            int stageId,
            string signerEmail,
            string signerName,
            string requestedByUserId);
    }
}
