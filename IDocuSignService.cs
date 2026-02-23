csharp FormBuilder.Domain.Interfaces.Services\IDocuSignService.cs
using System.Threading.Tasks;

namespace FormBuilder.Domain.Interfaces.Services
{
    /// <summary>
    /// Abstraction for DocuSign (or any e-sign provider) integration.
    /// Implement this to call DocuSign APIs and create envelopes/agreements.
    /// Return true when envelope was created/sent successfully.
    /// </summary>
    public interface IDocuSignService
    {
        /// <summary>
        /// Create and send a signing envelope for the given submission and stage.
        /// </summary>
        /// <param name="submissionId">Submission id</param>
        /// <param name="stageId">Approval stage id</param>
        /// <param name="signerEmail">Signer email</param>
        /// <param name="signerName">Signer display name</param>
        /// <param name="requestedByUserId">User who requested the signing</param>
        /// <returns>True when envelope was created and sent; false otherwise</returns>
        Task<bool> CreateSigningEnvelopeAsync(int submissionId, int stageId, string signerEmail, string signerName, string requestedByUserId);
    }