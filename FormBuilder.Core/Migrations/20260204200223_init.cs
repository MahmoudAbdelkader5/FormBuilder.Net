using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Core.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APPROVAL_DELEGATIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ToUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ScopeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScopeId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVAL_DELEGATIONS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ATTACHMENT_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxSizeMB = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATTACHMENT_TYPES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BLOCKING_RULE_AUDIT_LOG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    SubmissionId = table.Column<int>(type: "int", nullable: true),
                    EvaluationPhase = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RuleId = table.Column<int>(type: "int", nullable: true),
                    RuleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    BlockMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ContextJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BLOCKING_RULE_AUDIT_LOG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FIELD_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    HasOptions = table.Column<bool>(type: "bit", nullable: false),
                    AllowMultiple = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIELD_TYPES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FORM_BUILDER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ForeignFormName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FormCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForeignDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_BUILDER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FORM_STORED_PROCEDURES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DatabaseName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SchemaName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProcedureName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ProcedureCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsageType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    ExecutionOrder = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_STORED_PROCEDURES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NOTIFICATIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReferenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NOTIFICATIONS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PROJECTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECTS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "REFRESH_TOKENS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REFRESH_TOKENS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SMTP_CONFIGS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Host = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    UseSsl = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordEncrypted = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FromEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FromDisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMTP_CONFIGS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TABLE_MENUS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ForeignName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MenuCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TABLE_MENUS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USER_QUERIES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DatabaseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_QUERIES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FORM_ATTACHMENT_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    AttachmentTypeId = table.Column<int>(type: "int", nullable: false),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_ATTACHMENT_TYPES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_ATTACHMENT_TYPES_ATTACHMENT_TYPES_AttachmentTypeId",
                        column: x => x.AttachmentTypeId,
                        principalTable: "ATTACHMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FORM_ATTACHMENT_TYPES_FORM_BUILDER_FormBuilderId",
                        column: x => x.FormBuilderId,
                        principalTable: "FORM_BUILDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORM_BUTTONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    ButtonName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ButtonCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ButtonOrder = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionConfigJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisibleDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_BUTTONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_BUTTONS_FORM_BUILDER_FormBuilderId",
                        column: x => x.FormBuilderId,
                        principalTable: "FORM_BUILDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORM_TABS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    TabName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ForeignTabName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TabCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TabOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_TABS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_TABS_FORM_BUILDER_FormBuilderId",
                        column: x => x.FormBuilderId,
                        principalTable: "FORM_BUILDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORM_RULES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RuleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConditionField = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConditionOperator = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConditionValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConditionValueType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StoredProcedureId = table.Column<int>(type: "int", nullable: true),
                    FORM_STORED_PROCEDURESId = table.Column<int>(type: "int", nullable: true),
                    StoredProcedureName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StoredProcedureDatabase = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ParameterMapping = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultMapping = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ExecutionOrder = table.Column<int>(type: "int", nullable: true),
                    EvaluationPhase = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ConditionSource = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ConditionKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BlockMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_RULES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_RULES_FORM_BUILDER_FormBuilderId",
                        column: x => x.FormBuilderId,
                        principalTable: "FORM_BUILDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FORM_RULES_FORM_STORED_PROCEDURES_FORM_STORED_PROCEDURESId",
                        column: x => x.FORM_STORED_PROCEDURESId,
                        principalTable: "FORM_STORED_PROCEDURES",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TABLE_MENU_PERMISSIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    PermissionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TABLE_MENU_PERMISSIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TABLE_MENU_PERMISSIONS_TABLE_MENUS_MenuId",
                        column: x => x.MenuId,
                        principalTable: "TABLE_MENUS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TABLE_SUB_MENUS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ForeignName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TABLE_SUB_MENUS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TABLE_SUB_MENUS_TABLE_MENUS_MenuId",
                        column: x => x.MenuId,
                        principalTable: "TABLE_MENUS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORM_GRIDS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    GridName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GridCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TabId = table.Column<int>(type: "int", nullable: true),
                    GridOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MinRows = table.Column<int>(type: "int", nullable: true),
                    MaxRows = table.Column<int>(type: "int", nullable: true),
                    GridRulesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_GRIDS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_GRIDS_FORM_BUILDER_FormBuilderId",
                        column: x => x.FormBuilderId,
                        principalTable: "FORM_BUILDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FORM_GRIDS_FORM_TABS_TabId",
                        column: x => x.TabId,
                        principalTable: "FORM_TABS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FORM_RULE_ACTIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FieldCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Expression = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsElseAction = table.Column<bool>(type: "bit", nullable: false),
                    ActionOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_RULE_ACTIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_RULE_ACTIONS_FORM_RULES_RuleId",
                        column: x => x.RuleId,
                        principalTable: "FORM_RULES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TABLE_SUB_MENU_PERMISSIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubMenuId = table.Column<int>(type: "int", nullable: false),
                    PermissionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TABLE_SUB_MENU_PERMISSIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TABLE_SUB_MENU_PERMISSIONS_TABLE_SUB_MENUS_SubMenuId",
                        column: x => x.SubMenuId,
                        principalTable: "TABLE_SUB_MENUS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORM_FIELDS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TabId = table.Column<int>(type: "int", nullable: false),
                    FieldTypeId = table.Column<int>(type: "int", nullable: true),
                    GridId = table.Column<int>(type: "int", nullable: true),
                    FieldName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ForeignFieldName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FieldCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FieldOrder = table.Column<int>(type: "int", nullable: false),
                    Placeholder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForeignPlaceholder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HintText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForeignHintText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: true),
                    IsEditable = table.Column<bool>(type: "bit", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    DefaultValueJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    MaxValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    RegexPattern = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidationMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForeignValidationMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpressionText = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                    CalculationMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecalculateOn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResultType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_FIELDS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_FIELDS_FIELD_TYPES_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalTable: "FIELD_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FORM_FIELDS_FORM_GRIDS_GridId",
                        column: x => x.GridId,
                        principalTable: "FORM_GRIDS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FORM_FIELDS_FORM_TABS_TabId",
                        column: x => x.TabId,
                        principalTable: "FORM_TABS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORM_GRID_COLUMNS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GridId = table.Column<int>(type: "int", nullable: false),
                    FieldTypeId = table.Column<int>(type: "int", nullable: true),
                    ColumnName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ColumnCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ColumnOrder = table.Column<int>(type: "int", nullable: false),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    DefaultValueJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidationRuleJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    VisibilityRuleJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_GRID_COLUMNS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_GRID_COLUMNS_FIELD_TYPES_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalTable: "FIELD_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FORM_GRID_COLUMNS_FORM_GRIDS_GridId",
                        column: x => x.GridId,
                        principalTable: "FORM_GRIDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIELD_DATA_SOURCES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldId = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApiUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApiPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HttpMethod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RequestBodyJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValuePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfigurationJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIELD_DATA_SOURCES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIELD_DATA_SOURCES_FORM_FIELDS_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FORM_FIELDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FIELD_OPTIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldId = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ForeignOptionText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OptionValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OptionOrder = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FIELD_OPTIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FIELD_OPTIONS_FORM_FIELDS_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FORM_FIELDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORM_VALIDATION_RULES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FieldId = table.Column<int>(type: "int", nullable: true),
                    ExpressionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_VALIDATION_RULES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_VALIDATION_RULES_FORM_BUILDER_FormBuilderId",
                        column: x => x.FormBuilderId,
                        principalTable: "FORM_BUILDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FORM_VALIDATION_RULES_FORM_FIELDS_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FORM_FIELDS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FORMULAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExpressionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultFieldId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORMULAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORMULAS_FORM_BUILDER_FormBuilderId",
                        column: x => x.FormBuilderId,
                        principalTable: "FORM_BUILDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FORMULAS_FORM_FIELDS_ResultFieldId",
                        column: x => x.ResultFieldId,
                        principalTable: "FORM_FIELDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAP_FIELD_MAPPINGS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormFieldId = table.Column<int>(type: "int", nullable: false),
                    SapFieldName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAP_FIELD_MAPPINGS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAP_FIELD_MAPPINGS_FORM_FIELDS_FormFieldId",
                        column: x => x.FormFieldId,
                        principalTable: "FORM_FIELDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GRID_COLUMN_DATA_SOURCES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColumnId = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApiUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApiPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HttpMethod = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RequestBodyJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValuePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TextPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ConfigurationJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArrayPropertyNames = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRID_COLUMN_DATA_SOURCES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRID_COLUMN_DATA_SOURCES_FORM_GRID_COLUMNS_ColumnId",
                        column: x => x.ColumnId,
                        principalTable: "FORM_GRID_COLUMNS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GRID_COLUMN_OPTIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColumnId = table.Column<int>(type: "int", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ForeignOptionText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OptionValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OptionOrder = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRID_COLUMN_OPTIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRID_COLUMN_OPTIONS_FORM_GRID_COLUMNS_ColumnId",
                        column: x => x.ColumnId,
                        principalTable: "FORM_GRID_COLUMNS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORMULA_VARIABLES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormulaId = table.Column<int>(type: "int", nullable: false),
                    VariableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SourceFieldId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORMULA_VARIABLES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORMULA_VARIABLES_FORMULAS_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "FORMULAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FORMULA_VARIABLES_FORM_FIELDS_SourceFieldId",
                        column: x => x.SourceFieldId,
                        principalTable: "FORM_FIELDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ALERT_RULES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TriggerType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConditionJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailTemplateId = table.Column<int>(type: "int", nullable: true),
                    NotificationType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TargetRoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TargetUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ALERT_RULES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APPROVAL_STAGE_ASSIGNEES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVAL_STAGE_ASSIGNEES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APPROVAL_STAGES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<int>(type: "int", nullable: false),
                    StageOrder = table.Column<int>(type: "int", nullable: false),
                    StageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MinAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    IsFinalStage = table.Column<bool>(type: "bit", nullable: false),
                    MinimumRequiredAssignees = table.Column<int>(type: "int", nullable: true),
                    AmountFieldCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequiresAdobeSign = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVAL_STAGES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "APPROVAL_WORKFLOWS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPROVAL_WORKFLOWS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DOCUMENT_TYPES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    MenuCaption = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MenuOrder = table.Column<int>(type: "int", nullable: false),
                    ParentMenuId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalWorkflowId = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENT_TYPES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_TYPES_APPROVAL_WORKFLOWS_ApprovalWorkflowId",
                        column: x => x.ApprovalWorkflowId,
                        principalTable: "APPROVAL_WORKFLOWS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_TYPES_DOCUMENT_TYPES_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalTable: "DOCUMENT_TYPES",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DOCUMENT_TYPES_FORM_BUILDER_FormBuilderId",
                        column: x => x.FormBuilderId,
                        principalTable: "FORM_BUILDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CRYSTAL_LAYOUTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    LayoutName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LayoutPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CRYSTAL_LAYOUTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CRYSTAL_LAYOUTS_DOCUMENT_TYPES_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DOCUMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DOCUMENT_SERIES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SeriesCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NextNumber = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENT_SERIES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_SERIES_DOCUMENT_TYPES_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DOCUMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_SERIES_PROJECTS_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "PROJECTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EMAIL_TEMPLATES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TemplateCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubjectTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyTemplateHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmtpConfigId = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMAIL_TEMPLATES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EMAIL_TEMPLATES_DOCUMENT_TYPES_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DOCUMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EMAIL_TEMPLATES_SMTP_CONFIGS_SmtpConfigId",
                        column: x => x.SmtpConfigId,
                        principalTable: "SMTP_CONFIGS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OUTLOOK_APPROVAL_CONFIG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Mailbox = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RulesJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUTLOOK_APPROVAL_CONFIG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OUTLOOK_APPROVAL_CONFIG_DOCUMENT_TYPES_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DOCUMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SAP_OBJECT_MAPPINGS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    SapObjectName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDraftOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAP_OBJECT_MAPPINGS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAP_OBJECT_MAPPINGS_DOCUMENT_TYPES_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DOCUMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TABLE_MENU_DOCUMENTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: true),
                    SubMenuId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TABLE_MENU_DOCUMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TABLE_MENU_DOCUMENTS_DOCUMENT_TYPES_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DOCUMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TABLE_MENU_DOCUMENTS_TABLE_MENUS_MenuId",
                        column: x => x.MenuId,
                        principalTable: "TABLE_MENUS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TABLE_MENU_DOCUMENTS_TABLE_SUB_MENUS_SubMenuId",
                        column: x => x.SubMenuId,
                        principalTable: "TABLE_SUB_MENUS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FORM_SUBMISSIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormBuilderId = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubmittedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_SUBMISSIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSIONS_APPROVAL_STAGES_StageId",
                        column: x => x.StageId,
                        principalTable: "APPROVAL_STAGES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSIONS_DOCUMENT_SERIES_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "DOCUMENT_SERIES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSIONS_DOCUMENT_TYPES_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DOCUMENT_TYPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSIONS_FORM_BUILDER_FormBuilderId",
                        column: x => x.FormBuilderId,
                        principalTable: "FORM_BUILDER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TABLE_MENU_DOCUMENT_PERMISSIONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuDocumentId = table.Column<int>(type: "int", nullable: false),
                    PermissionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TABLE_MENU_DOCUMENT_PERMISSIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TABLE_MENU_DOCUMENT_PERMISSIONS_TABLE_MENU_DOCUMENTS_MenuDocumentId",
                        column: x => x.MenuDocumentId,
                        principalTable: "TABLE_MENU_DOCUMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COPY_TO_DOCUMENT_AUDIT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceSubmissionId = table.Column<int>(type: "int", nullable: false),
                    TargetDocumentId = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<int>(type: "int", nullable: true),
                    RuleId = table.Column<int>(type: "int", nullable: true),
                    SourceFormId = table.Column<int>(type: "int", nullable: false),
                    TargetFormId = table.Column<int>(type: "int", nullable: false),
                    TargetDocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FieldsCopied = table.Column<int>(type: "int", nullable: false),
                    GridRowsCopied = table.Column<int>(type: "int", nullable: false),
                    TargetDocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExecutionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COPY_TO_DOCUMENT_AUDIT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_COPY_TO_DOCUMENT_AUDIT_FORM_RULES_RuleId",
                        column: x => x.RuleId,
                        principalTable: "FORM_RULES",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_COPY_TO_DOCUMENT_AUDIT_FORM_RULE_ACTIONS_ActionId",
                        column: x => x.ActionId,
                        principalTable: "FORM_RULE_ACTIONS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_COPY_TO_DOCUMENT_AUDIT_FORM_SUBMISSIONS_SourceSubmissionId",
                        column: x => x.SourceSubmissionId,
                        principalTable: "FORM_SUBMISSIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COPY_TO_DOCUMENT_AUDIT_FORM_SUBMISSIONS_TargetDocumentId",
                        column: x => x.TargetDocumentId,
                        principalTable: "FORM_SUBMISSIONS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DOCUMENT_APPROVAL_HISTORY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OriginalApproverUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DelegationId = table.Column<int>(type: "int", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENT_APPROVAL_HISTORY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_APPROVAL_HISTORY_APPROVAL_DELEGATIONS_DelegationId",
                        column: x => x.DelegationId,
                        principalTable: "APPROVAL_DELEGATIONS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DOCUMENT_APPROVAL_HISTORY_APPROVAL_STAGES_StageId",
                        column: x => x.StageId,
                        principalTable: "APPROVAL_STAGES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DOCUMENT_APPROVAL_HISTORY_FORM_SUBMISSIONS_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "FORM_SUBMISSIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FORM_SUBMISSION_ATTACHMENTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    FieldId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_SUBMISSION_ATTACHMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSION_ATTACHMENTS_FORM_FIELDS_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FORM_FIELDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSION_ATTACHMENTS_FORM_SUBMISSIONS_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "FORM_SUBMISSIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FORM_SUBMISSION_GRID_ROWS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    GridId = table.Column<int>(type: "int", nullable: false),
                    RowIndex = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_SUBMISSION_GRID_ROWS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSION_GRID_ROWS_FORM_GRIDS_GridId",
                        column: x => x.GridId,
                        principalTable: "FORM_GRIDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSION_GRID_ROWS_FORM_SUBMISSIONS_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "FORM_SUBMISSIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORM_SUBMISSION_VALUES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    FieldId = table.Column<int>(type: "int", nullable: false),
                    FieldCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ValueString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueNumber = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    ValueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValueBool = table.Column<bool>(type: "bit", nullable: true),
                    ValueJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_SUBMISSION_VALUES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSION_VALUES_FORM_FIELDS_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FORM_FIELDS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSION_VALUES_FORM_SUBMISSIONS_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "FORM_SUBMISSIONS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FORM_SUBMISSION_GRID_CELLS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowId = table.Column<int>(type: "int", nullable: false),
                    ColumnId = table.Column<int>(type: "int", nullable: false),
                    ValueString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueNumber = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    ValueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValueBool = table.Column<bool>(type: "bit", nullable: true),
                    ValueJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORM_SUBMISSION_GRID_CELLS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSION_GRID_CELLS_FORM_GRID_COLUMNS_ColumnId",
                        column: x => x.ColumnId,
                        principalTable: "FORM_GRID_COLUMNS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FORM_SUBMISSION_GRID_CELLS_FORM_SUBMISSION_GRID_ROWS_RowId",
                        column: x => x.RowId,
                        principalTable: "FORM_SUBMISSION_GRID_ROWS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ALERT_RULES_DocumentTypeId",
                table: "ALERT_RULES",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ALERT_RULES_EmailTemplateId",
                table: "ALERT_RULES",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_STAGE_ASSIGNEES_StageId",
                table: "APPROVAL_STAGE_ASSIGNEES",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_STAGES_WorkflowId",
                table: "APPROVAL_STAGES",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_APPROVAL_WORKFLOWS_DocumentTypeId",
                table: "APPROVAL_WORKFLOWS",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_COPY_TO_DOCUMENT_AUDIT_ActionId",
                table: "COPY_TO_DOCUMENT_AUDIT",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_COPY_TO_DOCUMENT_AUDIT_RuleId",
                table: "COPY_TO_DOCUMENT_AUDIT",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_COPY_TO_DOCUMENT_AUDIT_SourceSubmissionId",
                table: "COPY_TO_DOCUMENT_AUDIT",
                column: "SourceSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_COPY_TO_DOCUMENT_AUDIT_TargetDocumentId",
                table: "COPY_TO_DOCUMENT_AUDIT",
                column: "TargetDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_CRYSTAL_LAYOUTS_DocumentTypeId",
                table: "CRYSTAL_LAYOUTS",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_APPROVAL_HISTORY_DelegationId",
                table: "DOCUMENT_APPROVAL_HISTORY",
                column: "DelegationId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_APPROVAL_HISTORY_StageId",
                table: "DOCUMENT_APPROVAL_HISTORY",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_APPROVAL_HISTORY_SubmissionId",
                table: "DOCUMENT_APPROVAL_HISTORY",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_SERIES_DocumentTypeId",
                table: "DOCUMENT_SERIES",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_SERIES_ProjectId",
                table: "DOCUMENT_SERIES",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_SERIES_SeriesCode",
                table: "DOCUMENT_SERIES",
                column: "SeriesCode");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_TYPES_ApprovalWorkflowId",
                table: "DOCUMENT_TYPES",
                column: "ApprovalWorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_TYPES_Code",
                table: "DOCUMENT_TYPES",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_TYPES_FormBuilderId",
                table: "DOCUMENT_TYPES",
                column: "FormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENT_TYPES_ParentMenuId",
                table: "DOCUMENT_TYPES",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_EMAIL_TEMPLATES_DocumentTypeId",
                table: "EMAIL_TEMPLATES",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EMAIL_TEMPLATES_SmtpConfigId",
                table: "EMAIL_TEMPLATES",
                column: "SmtpConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_FIELD_DATA_SOURCES_FieldId",
                table: "FIELD_DATA_SOURCES",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FIELD_OPTIONS_FieldId",
                table: "FIELD_OPTIONS",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_ATTACHMENT_TYPES_AttachmentTypeId",
                table: "FORM_ATTACHMENT_TYPES",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_ATTACHMENT_TYPES_FormBuilderId",
                table: "FORM_ATTACHMENT_TYPES",
                column: "FormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_BUILDER_FormCode",
                table: "FORM_BUILDER",
                column: "FormCode");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_BUTTONS_FormBuilderId",
                table: "FORM_BUTTONS",
                column: "FormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_FIELDS_FieldTypeId",
                table: "FORM_FIELDS",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_FIELDS_GridId",
                table: "FORM_FIELDS",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_FIELDS_TabId",
                table: "FORM_FIELDS",
                column: "TabId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_GRID_COLUMNS_FieldTypeId",
                table: "FORM_GRID_COLUMNS",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_GRID_COLUMNS_GridId",
                table: "FORM_GRID_COLUMNS",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_GRIDS_FormBuilderId",
                table: "FORM_GRIDS",
                column: "FormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_GRIDS_TabId",
                table: "FORM_GRIDS",
                column: "TabId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_RULE_ACTIONS_RuleId",
                table: "FORM_RULE_ACTIONS",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_RULES_FORM_STORED_PROCEDURESId",
                table: "FORM_RULES",
                column: "FORM_STORED_PROCEDURESId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_RULES_FormBuilderId",
                table: "FORM_RULES",
                column: "FormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSION_ATTACHMENTS_FieldId",
                table: "FORM_SUBMISSION_ATTACHMENTS",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSION_ATTACHMENTS_SubmissionId",
                table: "FORM_SUBMISSION_ATTACHMENTS",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSION_GRID_CELLS_ColumnId",
                table: "FORM_SUBMISSION_GRID_CELLS",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSION_GRID_CELLS_RowId",
                table: "FORM_SUBMISSION_GRID_CELLS",
                column: "RowId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSION_GRID_ROWS_GridId",
                table: "FORM_SUBMISSION_GRID_ROWS",
                column: "GridId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSION_GRID_ROWS_SubmissionId",
                table: "FORM_SUBMISSION_GRID_ROWS",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSION_VALUES_FieldId",
                table: "FORM_SUBMISSION_VALUES",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSION_VALUES_SubmissionId",
                table: "FORM_SUBMISSION_VALUES",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSIONS_DocumentNumber",
                table: "FORM_SUBMISSIONS",
                column: "DocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSIONS_DocumentTypeId",
                table: "FORM_SUBMISSIONS",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSIONS_FormBuilderId",
                table: "FORM_SUBMISSIONS",
                column: "FormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSIONS_SeriesId",
                table: "FORM_SUBMISSIONS",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_SUBMISSIONS_StageId",
                table: "FORM_SUBMISSIONS",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_TABS_FormBuilderId",
                table: "FORM_TABS",
                column: "FormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_VALIDATION_RULES_FieldId",
                table: "FORM_VALIDATION_RULES",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FORM_VALIDATION_RULES_FormBuilderId",
                table: "FORM_VALIDATION_RULES",
                column: "FormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_FORMULA_VARIABLES_FormulaId",
                table: "FORMULA_VARIABLES",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_FORMULA_VARIABLES_SourceFieldId",
                table: "FORMULA_VARIABLES",
                column: "SourceFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FORMULAS_FormBuilderId",
                table: "FORMULAS",
                column: "FormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_FORMULAS_ResultFieldId",
                table: "FORMULAS",
                column: "ResultFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_GRID_COLUMN_DATA_SOURCES_ColumnId",
                table: "GRID_COLUMN_DATA_SOURCES",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_GRID_COLUMN_OPTIONS_ColumnId",
                table: "GRID_COLUMN_OPTIONS",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_OUTLOOK_APPROVAL_CONFIG_DocumentTypeId",
                table: "OUTLOOK_APPROVAL_CONFIG",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECTS_Code",
                table: "PROJECTS",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_SAP_FIELD_MAPPINGS_FormFieldId",
                table: "SAP_FIELD_MAPPINGS",
                column: "FormFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_SAP_OBJECT_MAPPINGS_DocumentTypeId",
                table: "SAP_OBJECT_MAPPINGS",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TABLE_MENU_DOCUMENT_PERMISSIONS_MenuDocumentId",
                table: "TABLE_MENU_DOCUMENT_PERMISSIONS",
                column: "MenuDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TABLE_MENU_DOCUMENTS_DocumentTypeId",
                table: "TABLE_MENU_DOCUMENTS",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TABLE_MENU_DOCUMENTS_MenuId",
                table: "TABLE_MENU_DOCUMENTS",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_TABLE_MENU_DOCUMENTS_SubMenuId",
                table: "TABLE_MENU_DOCUMENTS",
                column: "SubMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_TABLE_MENU_PERMISSIONS_MenuId",
                table: "TABLE_MENU_PERMISSIONS",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_TABLE_SUB_MENU_PERMISSIONS_SubMenuId",
                table: "TABLE_SUB_MENU_PERMISSIONS",
                column: "SubMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_TABLE_SUB_MENUS_MenuId",
                table: "TABLE_SUB_MENUS",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_ALERT_RULES_DOCUMENT_TYPES_DocumentTypeId",
                table: "ALERT_RULES",
                column: "DocumentTypeId",
                principalTable: "DOCUMENT_TYPES",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ALERT_RULES_EMAIL_TEMPLATES_EmailTemplateId",
                table: "ALERT_RULES",
                column: "EmailTemplateId",
                principalTable: "EMAIL_TEMPLATES",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_APPROVAL_STAGE_ASSIGNEES_APPROVAL_STAGES_StageId",
                table: "APPROVAL_STAGE_ASSIGNEES",
                column: "StageId",
                principalTable: "APPROVAL_STAGES",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_APPROVAL_STAGES_APPROVAL_WORKFLOWS_WorkflowId",
                table: "APPROVAL_STAGES",
                column: "WorkflowId",
                principalTable: "APPROVAL_WORKFLOWS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_APPROVAL_WORKFLOWS_DOCUMENT_TYPES_DocumentTypeId",
                table: "APPROVAL_WORKFLOWS",
                column: "DocumentTypeId",
                principalTable: "DOCUMENT_TYPES",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APPROVAL_WORKFLOWS_DOCUMENT_TYPES_DocumentTypeId",
                table: "APPROVAL_WORKFLOWS");

            migrationBuilder.DropTable(
                name: "ALERT_RULES");

            migrationBuilder.DropTable(
                name: "APPROVAL_STAGE_ASSIGNEES");

            migrationBuilder.DropTable(
                name: "BLOCKING_RULE_AUDIT_LOG");

            migrationBuilder.DropTable(
                name: "COPY_TO_DOCUMENT_AUDIT");

            migrationBuilder.DropTable(
                name: "CRYSTAL_LAYOUTS");

            migrationBuilder.DropTable(
                name: "DOCUMENT_APPROVAL_HISTORY");

            migrationBuilder.DropTable(
                name: "FIELD_DATA_SOURCES");

            migrationBuilder.DropTable(
                name: "FIELD_OPTIONS");

            migrationBuilder.DropTable(
                name: "FORM_ATTACHMENT_TYPES");

            migrationBuilder.DropTable(
                name: "FORM_BUTTONS");

            migrationBuilder.DropTable(
                name: "FORM_SUBMISSION_ATTACHMENTS");

            migrationBuilder.DropTable(
                name: "FORM_SUBMISSION_GRID_CELLS");

            migrationBuilder.DropTable(
                name: "FORM_SUBMISSION_VALUES");

            migrationBuilder.DropTable(
                name: "FORM_VALIDATION_RULES");

            migrationBuilder.DropTable(
                name: "FORMULA_VARIABLES");

            migrationBuilder.DropTable(
                name: "GRID_COLUMN_DATA_SOURCES");

            migrationBuilder.DropTable(
                name: "GRID_COLUMN_OPTIONS");

            migrationBuilder.DropTable(
                name: "NOTIFICATIONS");

            migrationBuilder.DropTable(
                name: "OUTLOOK_APPROVAL_CONFIG");

            migrationBuilder.DropTable(
                name: "REFRESH_TOKENS");

            migrationBuilder.DropTable(
                name: "SAP_FIELD_MAPPINGS");

            migrationBuilder.DropTable(
                name: "SAP_OBJECT_MAPPINGS");

            migrationBuilder.DropTable(
                name: "TABLE_MENU_DOCUMENT_PERMISSIONS");

            migrationBuilder.DropTable(
                name: "TABLE_MENU_PERMISSIONS");

            migrationBuilder.DropTable(
                name: "TABLE_SUB_MENU_PERMISSIONS");

            migrationBuilder.DropTable(
                name: "USER_QUERIES");

            migrationBuilder.DropTable(
                name: "EMAIL_TEMPLATES");

            migrationBuilder.DropTable(
                name: "FORM_RULE_ACTIONS");

            migrationBuilder.DropTable(
                name: "APPROVAL_DELEGATIONS");

            migrationBuilder.DropTable(
                name: "ATTACHMENT_TYPES");

            migrationBuilder.DropTable(
                name: "FORM_SUBMISSION_GRID_ROWS");

            migrationBuilder.DropTable(
                name: "FORMULAS");

            migrationBuilder.DropTable(
                name: "FORM_GRID_COLUMNS");

            migrationBuilder.DropTable(
                name: "TABLE_MENU_DOCUMENTS");

            migrationBuilder.DropTable(
                name: "SMTP_CONFIGS");

            migrationBuilder.DropTable(
                name: "FORM_RULES");

            migrationBuilder.DropTable(
                name: "FORM_SUBMISSIONS");

            migrationBuilder.DropTable(
                name: "FORM_FIELDS");

            migrationBuilder.DropTable(
                name: "TABLE_SUB_MENUS");

            migrationBuilder.DropTable(
                name: "FORM_STORED_PROCEDURES");

            migrationBuilder.DropTable(
                name: "APPROVAL_STAGES");

            migrationBuilder.DropTable(
                name: "DOCUMENT_SERIES");

            migrationBuilder.DropTable(
                name: "FIELD_TYPES");

            migrationBuilder.DropTable(
                name: "FORM_GRIDS");

            migrationBuilder.DropTable(
                name: "TABLE_MENUS");

            migrationBuilder.DropTable(
                name: "PROJECTS");

            migrationBuilder.DropTable(
                name: "FORM_TABS");

            migrationBuilder.DropTable(
                name: "DOCUMENT_TYPES");

            migrationBuilder.DropTable(
                name: "APPROVAL_WORKFLOWS");

            migrationBuilder.DropTable(
                name: "FORM_BUILDER");
        }
    }
}
