/**
 * CopyToDocument Component Example
 * يوضح كيفية استخدام CopyToDocument Service في Angular
 */

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { CopyToDocumentService } from '../services/copy-to-document.service';
import { CopyToDocumentActionDto, CopyToDocumentResultDto } from '../models/copy-to-document.models';

@Component({
  selector: 'app-copy-document',
  templateUrl: './copy-document.component.html',
  styleUrls: ['./copy-document.component.css']
})
export class CopyDocumentComponent implements OnInit {
  copyForm!: FormGroup;
  loading = false;
  result: CopyToDocumentResultDto | null = null;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private copyToDocumentService: CopyToDocumentService
  ) {}

  ngOnInit() {
    this.initializeForm();
  }

  /**
   * تهيئة الـ Form مع الحقول المطلوبة الجديدة
   */
  initializeForm() {
    this.copyForm = this.fb.group({
      // الحقول المطلوبة الجديدة
      sourceDocumentTypeId: [null, [Validators.required, Validators.min(1)]],
      sourceFormId: [null, [Validators.required, Validators.min(1)]],
      
      targetDocumentTypeId: [null, [Validators.required, Validators.min(1)]],
      targetFormId: [null, [Validators.required, Validators.min(1)]],
      sourceSubmissionId: [null, [Validators.required, Validators.min(1)]],
      
      createNewDocument: [true],
      targetDocumentId: [null],
      
      // الحقل الجديد
      initialStatus: ['Draft', Validators.required],
      
      // Field Mapping
      fieldMappings: this.fb.array([]),
      
      // Grid Mapping
      gridMappings: this.fb.array([]),
      
      // Options
      copyCalculatedFields: [true],
      copyGridRows: [true],
      startWorkflow: [false],
      linkDocuments: [true],
      copyAttachments: [false],
      copyMetadata: [false],
      overrideTargetDefaults: [false],
      
      // Metadata Fields
      metadataFields: this.fb.array([])
    });
  }

  /**
   * إضافة field mapping جديد
   */
  addFieldMapping() {
    const fieldMappings = this.copyForm.get('fieldMappings') as FormArray;
    fieldMappings.push(this.fb.group({
      sourceFieldCode: ['', Validators.required],
      targetFieldCode: ['', Validators.required]
    }));
  }

  /**
   * حذف field mapping
   */
  removeFieldMapping(index: number) {
    const fieldMappings = this.copyForm.get('fieldMappings') as FormArray;
    fieldMappings.removeAt(index);
  }

  /**
   * إضافة grid mapping جديد
   */
  addGridMapping() {
    const gridMappings = this.copyForm.get('gridMappings') as FormArray;
    gridMappings.push(this.fb.group({
      sourceGridCode: ['', Validators.required],
      targetGridCode: ['', Validators.required]
    }));
  }

  /**
   * حذف grid mapping
   */
  removeGridMapping(index: number) {
    const gridMappings = this.copyForm.get('gridMappings') as FormArray;
    gridMappings.removeAt(index);
  }

  /**
   * إضافة metadata field
   */
  addMetadataField() {
    const metadataFields = this.copyForm.get('metadataFields') as FormArray;
    metadataFields.push(this.fb.control('', Validators.required));
  }

  /**
   * حذف metadata field
   */
  removeMetadataField(index: number) {
    const metadataFields = this.copyForm.get('metadataFields') as FormArray;
    metadataFields.removeAt(index);
  }

  /**
   * تنفيذ CopyToDocument
   */
  onSubmit() {
    if (this.copyForm.valid) {
      this.loading = true;
      this.result = null;
      this.errorMessage = null;

      const formValue = this.copyForm.value;

      // بناء Field Mapping
      const fieldMapping: { [key: string]: string } = {};
      formValue.fieldMappings.forEach((mapping: any) => {
        if (mapping.sourceFieldCode && mapping.targetFieldCode) {
          fieldMapping[mapping.sourceFieldCode] = mapping.targetFieldCode;
        }
      });

      // بناء Grid Mapping
      const gridMapping: { [key: string]: string } = {};
      formValue.gridMappings.forEach((mapping: any) => {
        if (mapping.sourceGridCode && mapping.targetGridCode) {
          gridMapping[mapping.sourceGridCode] = mapping.targetGridCode;
        }
      });

      // بناء Config
      const config: CopyToDocumentActionDto = {
        sourceDocumentTypeId: formValue.sourceDocumentTypeId,
        sourceFormId: formValue.sourceFormId,
        targetDocumentTypeId: formValue.targetDocumentTypeId,
        targetFormId: formValue.targetFormId,
        sourceSubmissionId: formValue.sourceSubmissionId,
        createNewDocument: formValue.createNewDocument,
        targetDocumentId: formValue.targetDocumentId,
        initialStatus: formValue.initialStatus,
        fieldMapping: fieldMapping,
        gridMapping: gridMapping,
        copyCalculatedFields: formValue.copyCalculatedFields,
        copyGridRows: formValue.copyGridRows,
        startWorkflow: formValue.startWorkflow,
        linkDocuments: formValue.linkDocuments,
        copyAttachments: formValue.copyAttachments,
        copyMetadata: formValue.copyMetadata,
        overrideTargetDefaults: formValue.overrideTargetDefaults,
        metadataFields: formValue.metadataFields.filter((f: string) => f && f.trim())
      };

      const request = {
        config: config,
        sourceSubmissionId: formValue.sourceSubmissionId
      };

      // استدعاء الـ Service
      this.copyToDocumentService.executeCopyToDocument(request).subscribe({
        next: (response) => {
          this.loading = false;
          if (response.statusCode === 200 && response.data) {
            this.result = response.data;
            if (this.result.success) {
              console.log('تم النسخ بنجاح!', this.result);
            } else {
              this.errorMessage = this.result.errorMessage || 'فشل النسخ';
            }
          } else {
            this.errorMessage = response.message || 'حدث خطأ غير متوقع';
          }
        },
        error: (error) => {
          this.loading = false;
          console.error('Error:', error);
          
          if (error.error?.errors) {
            // Validation errors
            const errors = error.error.errors;
            const errorMessages = Object.keys(errors).map(key => 
              `${key}: ${errors[key].join(', ')}`
            ).join('\n');
            this.errorMessage = `أخطاء في التحقق:\n${errorMessages}`;
          } else {
            this.errorMessage = error.error?.message || error.message || 'حدث خطأ في الاتصال';
          }
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.copyForm.controls).forEach(key => {
        this.copyForm.get(key)?.markAsTouched();
      });
    }
  }

  /**
   * استخدام Helper Method من Service
   */
  executeWithHelper() {
    const config = this.copyToDocumentService.createConfig({
      sourceDocumentTypeId: 1,
      sourceFormId: 10,
      targetDocumentTypeId: 2,
      targetFormId: 20,
      sourceSubmissionId: 115,
      initialStatus: 'Draft',
      fieldMapping: {
        'TOTAL_AMOUNT': 'CONTRACT_VALUE',
        'REQUEST_DATE': 'ORDER_DATE'
      },
      copyCalculatedFields: true,
      copyGridRows: true
    });

    const request = {
      config: config,
      sourceSubmissionId: 115
    };

    this.copyToDocumentService.executeCopyToDocument(request).subscribe({
      next: (response) => {
        if (response.data?.success) {
          console.log('Success:', response.data);
        }
      }
    });
  }

  /**
   * عرض سجلات Audit
   */
  viewAuditRecords() {
    this.copyToDocumentService.getAuditRecords({
      sourceSubmissionId: this.copyForm.value.sourceSubmissionId,
      page: 1,
      pageSize: 10
    }).subscribe({
      next: (response) => {
        if (response.data) {
          console.log('Audit Records:', response.data.data);
          console.log('Total:', response.data.totalCount);
        }
      }
    });
  }

  /**
   * Getter methods for FormArrays
   */
  get fieldMappings() {
    return this.copyForm.get('fieldMappings') as FormArray;
  }

  get gridMappings() {
    return this.copyForm.get('gridMappings') as FormArray;
  }

  get metadataFields() {
    return this.copyForm.get('metadataFields') as FormArray;
  }
}


