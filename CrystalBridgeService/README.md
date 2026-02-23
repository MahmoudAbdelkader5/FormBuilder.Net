# CrystalBridgeService (.NET Framework 4.8)

This project is the Crystal Reports bridge used by the .NET 9 API proxy.

## Endpoints

- `GET /api/reports/health`
- `GET /api/reports/GenerateLayout?idLayout={idLayout}&idObject={idObject}&fileName={fileName}&printedByUserId={userId}`

## Required setup

1. Install **SAP Crystal Reports for Visual Studio**.
2. Install **SAP Crystal Reports Runtime** on the machine running this service.
3. Copy `.rpt` files to `CrystalBridgeService/Attachments/SystemLayout` (or set another folder).
4. Set `ReportsRootPath` in `Web.config` to the folder that contains report files.
5. Ensure report parameters include `DocKey@`, `ObjectId@`, `PrintedByUserID@`, `ApplicationPath@`.
6. Set `FormBuilderDb` connection string in `Web.config`.
7. Run this service on `http://localhost:5005/`.

## Core API integration

In `FrombuilderApiProject/appsettings.Development.json`:

```json
"CrystalBridge": {
  "BaseUrl": "http://localhost:5005/",
  "GenerateLayoutPath": "api/reports/GenerateLayout",
  "Username": "",
  "Password": "",
  "TimeoutSeconds": 120
}
```
