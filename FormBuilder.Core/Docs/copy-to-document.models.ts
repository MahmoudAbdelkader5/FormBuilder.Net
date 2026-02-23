/**
 * CopyToDocument Models for Angular/TypeScript
 * Updated with new required fields: sourceDocumentTypeId, sourceFormId, initialStatus
 */

/**
 * Configuration DTO for CopyToDocument action
 */
export interface CopyToDocumentActionDto {
  /** Source Document Type ID (required - NEW) */
  sourceDocumentTypeId: number;

  /** Source Form Builder ID (required - was optional) */
  sourceFormId: number;

  /** Source Submission ID (optional - defaults to current submission) */
  sourceSubmissionId?: number;

  /** Target Document Type ID (required) */
  targetDocumentTypeId: number;

  /** Target Form Builder ID (required) */
  targetFormId: number;

  /** Create new document if true, update existing if false */
  createNewDocument: boolean;

  /** Target document ID to update when CreateNewDocument is false */
  targetDocumentId?: number;

  /** Initial status for new target document (Draft / Submitted) - NEW */
  initialStatus?: 'Draft' | 'Submitted';

  /** Field mapping: SourceFieldCode -> TargetFieldCode */
  fieldMapping: { [sourceFieldCode: string]: string };

  /** Grid mapping: SourceGridCode -> TargetGridCode */
  gridMapping?: { [sourceGridCode: string]: string };

  /** Copy calculated fields (Yes/No) */
  copyCalculatedFields: boolean;

  /** Copy grid rows (Yes/No) */
  copyGridRows: boolean;

  /** Start workflow for target document (Yes/No) */
  startWorkflow: boolean;

  /** Link source and target documents (set ParentDocumentId) */
  linkDocuments: boolean;

  /** Copy attachments (Yes/No) */
  copyAttachments: boolean;

  /** Copy metadata (submission date, document number, etc.) */
  copyMetadata: boolean;

  /** Override target default values with source values (Yes/No) */
  overrideTargetDefaults: boolean;

  /** Metadata fields to copy (if CopyMetadata = true) */
  metadataFields?: string[];
}

/**
 * Result DTO for CopyToDocument action execution
 */
export interface CopyToDocumentResultDto {
  /** Whether the copy operation was successful */
  success: boolean;

  /** Target document ID (if created/updated successfully) */
  targetDocumentId?: number;

  /** Target document number (if created successfully) */
  targetDocumentNumber?: string;

  /** Error message (if failed) */
  errorMessage?: string;

  /** Number of fields copied */
  fieldsCopied: number;

  /** Number of grid rows copied */
  gridRowsCopied: number;

  /** Action ID that triggered this copy */
  actionId?: number;

  /** Source submission ID */
  sourceSubmissionId: number;
}

/**
 * Request DTO for executing CopyToDocument action (using IDs)
 */
export interface ExecuteCopyToDocumentRequestDto {
  /** CopyToDocument configuration */
  config: CopyToDocumentActionDto;

  /** Source submission ID */
  sourceSubmissionId: number;

  /** Action ID (optional - for audit purposes) */
  actionId?: number;

  /** Rule ID (optional - for audit purposes) */
  ruleId?: number;
}

/**
 * Configuration DTO for CopyToDocument action using codes instead of IDs
 */
export interface CopyToDocumentActionByCodesDto {
  /** Source Document Type Code (required - NEW) */
  sourceDocumentTypeCode: string;

  /** Source Form Code (required - NEW) */
  sourceFormCode: string;

  /** Target Document Type Code (required) */
  targetDocumentTypeCode: string;

  /** Target Form Code (required) */
  targetFormCode: string;

  /** Create new document if true, update existing if false */
  createNewDocument: boolean;

  /** Target document ID to update when CreateNewDocument is false */
  targetDocumentId?: number;

  /** Initial status for new target document (Draft / Submitted) - NEW */
  initialStatus?: 'Draft' | 'Submitted';

  /** Field mapping: SourceFieldCode -> TargetFieldCode */
  fieldMapping?: { [key: string]: string };

  /** Grid mapping: SourceGridCode -> TargetGridCode */
  gridMapping?: { [key: string]: string };

  /** Copy calculated fields (Yes/No) */
  copyCalculatedFields: boolean;

  /** Copy grid rows (Yes/No) */
  copyGridRows: boolean;

  /** Start workflow for target document (Yes/No) */
  startWorkflow: boolean;

  /** Link source and target documents (set ParentDocumentId) */
  linkDocuments: boolean;

  /** Copy attachments (Yes/No) */
  copyAttachments: boolean;

  /** Copy metadata (submission date, document number, etc.) */
  copyMetadata: boolean;

  /** Override target default values with source values (Yes/No) */
  overrideTargetDefaults: boolean;

  /** Metadata fields to copy (if CopyMetadata = true) */
  metadataFields?: string[];
}

/**
 * Request DTO for executing CopyToDocument action (using Codes)
 */
export interface ExecuteCopyToDocumentByCodesRequestDto {
  /** CopyToDocument configuration with codes */
  config: CopyToDocumentActionByCodesDto;

  /** Source submission ID */
  sourceSubmissionId: number;

  /** Action ID (optional - for audit purposes) */
  actionId?: number;

  /** Rule ID (optional - for audit purposes) */
  ruleId?: number;
}

/**
 * DTO for CopyToDocument audit record
 */
export interface CopyToDocumentAuditDto {
  id: number;
  sourceSubmissionId: number;
  targetDocumentId?: number;
  actionId?: number;
  ruleId?: number;
  sourceFormId: number;
  targetFormId: number;
  targetDocumentTypeId: number;
  success: boolean;
  errorMessage?: string;
  fieldsCopied: number;
  gridRowsCopied: number;
  targetDocumentNumber?: string;
  executionDate: string; // ISO date string
  createdDate: string; // ISO date string
  createdByUserId?: string;
}

/**
 * API Response wrapper
 */
export interface ApiResponse<T = any> {
  statusCode: number;
  message: string;
  data?: T;
  errors?: { [key: string]: string[] };
}

/**
 * Paginated Audit Records Response
 */
export interface PaginatedAuditResponse {
  data: CopyToDocumentAuditDto[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}


