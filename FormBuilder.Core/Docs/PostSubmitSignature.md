# Post-Submit Signature (DocuSign Embedded)

## Environment variables
Set these variables for the API process (no hardcoded secrets):

- `DS_AUTH_SERVER` (example: `account-d.docusign.com`)
- `DS_INTEGRATION_KEY`
- `DS_USER_ID`
- `DS_PRIVATE_KEY_PATH` (absolute path to PEM private key)
- `DS_REDIRECT_URI` (optional fallback return URL base)
- `APP_BASE_URL` (preferred base URL for embedded signing return URL)

## Submit API contract
### Endpoint
`POST /api/submissions/{id}/submit`

### Success response (200)
```json
{
  "submitted": true,
  "signatureRequired": true,
  "signatureStatus": "pending",
  "signingUrl": "https://demo.docusign.net/Signing/..."
}
```

If signature is not required:
```json
{
  "submitted": true,
  "signatureRequired": false,
  "signatureStatus": "not_required",
  "signingUrl": null
}
```

## Webhook
### Endpoint
`POST /webhooks/docusign`

### Behavior
- Reads `envelopeId` and status from DocuSign payload.
- If status is `completed`:
  - finds submission by `docusign_envelope_id`
  - updates `signature_status = signed`
  - sets `signed_at = UtcNow`

## Local test on DocuSign demo
1. Configure DocuSign integration key and consent for JWT grant in demo account.
2. Set the env vars above in your local run profile / system environment.
3. Submit a form that reaches a stage with `RequiresAdobeSign = true`.
4. Verify API response contains:
   - `signatureRequired = true`
   - `signatureStatus = pending`
   - non-null `signingUrl`
5. Open `signingUrl`, complete signing.
6. Send/connect webhook payload to `/webhooks/docusign` with completed status and same envelope id.
7. Verify submission row has `signature_status = signed` and `signed_at` populated.

