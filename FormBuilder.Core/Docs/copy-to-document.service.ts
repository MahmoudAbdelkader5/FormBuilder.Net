/**
 * CopyToDocument Service for Angular
 * Updated to support new required fields: sourceDocumentTypeId, sourceFormId, initialStatus
 */

import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  CopyToDocumentActionDto, 
  CopyToDocumentResultDto,
  ExecuteCopyToDocumentRequestDto,
  ExecuteCopyToDocumentByCodesRequestDto,
  CopyToDocumentAuditDto,
  ApiResponse,
  PaginatedAuditResponse
} from './copy-to-document.models';

@Injectable({
  providedIn: 'root'
})
export class CopyToDocumentService {
  private readonly apiUrl = '/api/CopyToDocument';

  constructor(private http: HttpClient) { }

  /**
   * تنفيذ CopyToDocument باستخدام IDs
   * @param request Request DTO containing config and sourceSubmissionId
   * @returns Observable of CopyToDocumentResultDto
   */
  executeCopyToDocument(request: ExecuteCopyToDocumentRequestDto): Observable<ApiResponse<CopyToDocumentResultDto>> {
    return this.http.post<ApiResponse<CopyToDocumentResultDto>>(
      `${this.apiUrl}/execute`, 
      request
    );
  }

  /**
   * تنفيذ CopyToDocument باستخدام Codes
   * @param request Request DTO with codes instead of IDs
   * @returns Observable of CopyToDocumentResultDto
   */
  executeCopyToDocumentByCodes(request: ExecuteCopyToDocumentByCodesRequestDto): Observable<ApiResponse<CopyToDocumentResultDto>> {
    return this.http.post<ApiResponse<CopyToDocumentResultDto>>(
      `${this.apiUrl}/execute-by-codes`, 
      request
    );
  }

  /**
   * الحصول على سجلات Audit مع pagination و filters
   * @param params Query parameters for filtering and pagination
   * @returns Observable of paginated audit records
   */
  getAuditRecords(params?: {
    sourceSubmissionId?: number;
    targetDocumentId?: number;
    ruleId?: number;
    success?: boolean;
    page?: number;
    pageSize?: number;
  }): Observable<ApiResponse<PaginatedAuditResponse>> {
    let httpParams = new HttpParams();
    
    if (params) {
      if (params.sourceSubmissionId) {
        httpParams = httpParams.set('sourceSubmissionId', params.sourceSubmissionId.toString());
      }
      if (params.targetDocumentId) {
        httpParams = httpParams.set('targetDocumentId', params.targetDocumentId.toString());
      }
      if (params.ruleId) {
        httpParams = httpParams.set('ruleId', params.ruleId.toString());
      }
      if (params.success !== undefined) {
        httpParams = httpParams.set('success', params.success.toString());
      }
      if (params.page) {
        httpParams = httpParams.set('page', params.page.toString());
      }
      if (params.pageSize) {
        httpParams = httpParams.set('pageSize', params.pageSize.toString());
      }
    }

    return this.http.get<ApiResponse<PaginatedAuditResponse>>(
      `${this.apiUrl}/audit`, 
      { params: httpParams }
    );
  }

  /**
   * الحصول على سجل Audit محدد بالـ ID
   * @param id Audit record ID
   * @returns Observable of audit record
   */
  getAuditRecordById(id: number): Observable<ApiResponse<CopyToDocumentAuditDto>> {
    return this.http.get<ApiResponse<CopyToDocumentAuditDto>>(
      `${this.apiUrl}/audit/${id}`
    );
  }

  /**
   * الحصول على سجلات Audit لمستند مصدر محدد
   * @param submissionId Source submission ID
   * @returns Observable of audit records array
   */
  getAuditRecordsBySubmissionId(submissionId: number): Observable<ApiResponse<CopyToDocumentAuditDto[]>> {
    return this.http.get<ApiResponse<CopyToDocumentAuditDto[]>>(
      `${this.apiUrl}/audit/submission/${submissionId}`
    );
  }

  /**
   * الحصول على سجلات Audit لمستند هدف محدد
   * @param targetDocumentId Target document ID
   * @returns Observable of audit records array
   */
  getAuditRecordsByTargetDocumentId(targetDocumentId: number): Observable<ApiResponse<CopyToDocumentAuditDto[]>> {
    return this.http.get<ApiResponse<CopyToDocumentAuditDto[]>>(
      `${this.apiUrl}/audit/target/${targetDocumentId}`
    );
  }

  /**
   * Helper method: إنشاء CopyToDocument config بسهولة
   * @param options Configuration options
   * @returns CopyToDocumentActionDto
   */
  createConfig(options: {
    sourceDocumentTypeId: number;
    sourceFormId: number;
    targetDocumentTypeId: number;
    targetFormId: number;
    sourceSubmissionId?: number;
    createNewDocument?: boolean;
    targetDocumentId?: number;
    initialStatus?: 'Draft' | 'Submitted';
    fieldMapping?: { [key: string]: string };
    gridMapping?: { [key: string]: string };
    copyCalculatedFields?: boolean;
    copyGridRows?: boolean;
    startWorkflow?: boolean;
    linkDocuments?: boolean;
    copyAttachments?: boolean;
    copyMetadata?: boolean;
    overrideTargetDefaults?: boolean;
    metadataFields?: string[];
  }): CopyToDocumentActionDto {
    return {
      sourceDocumentTypeId: options.sourceDocumentTypeId,
      sourceFormId: options.sourceFormId,
      targetDocumentTypeId: options.targetDocumentTypeId,
      targetFormId: options.targetFormId,
      sourceSubmissionId: options.sourceSubmissionId,
      createNewDocument: options.createNewDocument ?? true,
      targetDocumentId: options.targetDocumentId,
      initialStatus: options.initialStatus ?? 'Draft',
      fieldMapping: options.fieldMapping ?? {},
      gridMapping: options.gridMapping ?? {},
      copyCalculatedFields: options.copyCalculatedFields ?? true,
      copyGridRows: options.copyGridRows ?? true,
      startWorkflow: options.startWorkflow ?? false,
      linkDocuments: options.linkDocuments ?? true,
      copyAttachments: options.copyAttachments ?? false,
      copyMetadata: options.copyMetadata ?? false,
      overrideTargetDefaults: options.overrideTargetDefaults ?? false,
      metadataFields: options.metadataFields ?? []
    };
  }
}


