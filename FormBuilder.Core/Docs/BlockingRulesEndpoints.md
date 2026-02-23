# Blocking Rules API Endpoints - Test Guide

## Overview
This document describes the API endpoints for testing Blocking Rules functionality.

## Endpoints

### 1. Evaluate Blocking Rules
**POST** `/api/FormRules/evaluate-blocking`

Evaluates blocking rules for a form at a specific evaluation phase.

#### Request Body
```json
{
  "formBuilderId": 1,
  "evaluationPhase": "PreOpen",  // or "PreSubmit"
  "submissionId": 123,  // Optional, required for PreSubmit
  "fieldValues": {  // Optional, required for PreSubmit with Submission-based rules
    "TOTAL_AMOUNT": 15000,
    "COUNTRY": "US"
  }
}
```

#### Response (200 OK)
```json
{
  "isBlocked": false,
  "blockMessage": null,
  "matchedRuleId": null,
  "matchedRuleName": null
}
```

#### Response (Blocked - 200 OK)
```json
{
  "isBlocked": true,
  "blockMessage": "Form access is blocked by rule 'Accounting Period Closed'",
  "matchedRuleId": 5,
  "matchedRuleName": "Accounting Period Closed"
}
```

#### Example: Pre-Open Test
```bash
POST /api/FormRules/evaluate-blocking
Content-Type: application/json

{
  "formBuilderId": 1,
  "evaluationPhase": "PreOpen"
}
```

#### Example: Pre-Submit Test
```bash
POST /api/FormRules/evaluate-blocking
Content-Type: application/json

{
  "formBuilderId": 1,
  "evaluationPhase": "PreSubmit",
  "submissionId": 123,
  "fieldValues": {
    "TOTAL_AMOUNT": 15000,
    "COUNTRY": "US"
  }
}
```

---

### 2. Get Blocking Rules Audit Logs
**GET** `/api/FormRules/blocking-audit-logs`

Retrieves audit logs for blocking rule evaluations.

#### Query Parameters
- `formBuilderId` (optional): Filter by form builder ID
- `submissionId` (optional): Filter by submission ID
- `evaluationPhase` (optional): Filter by phase ("PreOpen" or "PreSubmit")
- `ruleId` (optional): Filter by rule ID
- `isBlocked` (optional): Filter by blocked status (true/false)
- `page` (optional, default: 1): Page number
- `pageSize` (optional, default: 50): Items per page

#### Response (200 OK)
```json
{
  "data": [
    {
      "id": 1,
      "formBuilderId": 1,
      "submissionId": 123,
      "evaluationPhase": "PreSubmit",
      "ruleId": 5,
      "ruleName": "Accounting Period Closed",
      "isBlocked": true,
      "blockMessage": "Form access is blocked by rule 'Accounting Period Closed'",
      "userId": "user123",
      "contextJson": null,
      "createdDate": "2025-02-15T10:30:00Z"
    }
  ],
  "totalCount": 1,
  "page": 1,
  "pageSize": 50,
  "totalPages": 1
}
```

#### Example Requests
```bash
# Get all audit logs
GET /api/FormRules/blocking-audit-logs

# Get logs for specific form
GET /api/FormRules/blocking-audit-logs?formBuilderId=1

# Get blocked logs only
GET /api/FormRules/blocking-audit-logs?isBlocked=true

# Get Pre-Open logs
GET /api/FormRules/blocking-audit-logs?evaluationPhase=PreOpen

# Paginated results
GET /api/FormRules/blocking-audit-logs?page=1&pageSize=20
```

---

## Testing Scenarios

### Scenario 1: Pre-Open Blocking Rule
1. Create a blocking rule with:
   - `EvaluationPhase`: "PreOpen"
   - `ConditionSource`: "Database"
   - `RuleType`: "StoredProcedure"
   - `BlockMessage`: "Accounting period is closed"

2. Test endpoint:
```bash
POST /api/FormRules/evaluate-blocking
{
  "formBuilderId": 1,
  "evaluationPhase": "PreOpen"
}
```

3. Expected: If condition met, `isBlocked` = true

### Scenario 2: Pre-Submit Blocking Rule
1. Create a blocking rule with:
   - `EvaluationPhase`: "PreSubmit"
   - `ConditionSource`: "Submission"
   - `ConditionKey`: "TOTAL_AMOUNT"
   - `ConditionOperator`: ">"
   - `ConditionValue`: "10000"
   - `BlockMessage`: "Total amount exceeds allowed limit"

2. Test endpoint:
```bash
POST /api/FormRules/evaluate-blocking
{
  "formBuilderId": 1,
  "evaluationPhase": "PreSubmit",
  "submissionId": 123,
  "fieldValues": {
    "TOTAL_AMOUNT": 15000
  }
}
```

3. Expected: If TOTAL_AMOUNT > 10000, `isBlocked` = true

### Scenario 3: Check Audit Logs
After running evaluations, check audit logs:
```bash
GET /api/FormRules/blocking-audit-logs?formBuilderId=1&isBlocked=true
```

---

## Integration with Form Submission

### Pre-Open Check
When creating a draft submission:
- Endpoint: `POST /api/FormSubmissions/create-draft`
- Automatically evaluates Pre-Open blocking rules
- Returns 403 if blocked

### Pre-Submit Check
When submitting a form:
- Endpoint: `POST /api/FormSubmissions/submit`
- Automatically evaluates Pre-Submit blocking rules
- Returns 403 if blocked

---

## Notes
- All blocking rule evaluations are automatically logged to `BLOCKING_RULE_AUDIT_LOG` table
- Rules are evaluated in priority order (highest priority first)
- Only active rules are evaluated
- First matching rule stops evaluation (highest priority wins)

