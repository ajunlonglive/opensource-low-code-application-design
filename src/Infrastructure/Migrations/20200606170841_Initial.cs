using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DAppGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsInternal = table.Column<bool>(nullable: false),
                    ParentGroupId = table.Column<Guid>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppGroups_DAppGroups_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalTable: "DAppGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DCAppDataDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AllowNullValue = table.Column<bool>(nullable: false),
                    AllowOnlyUniqueValue = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true),
                    AllowMultipleSelection = table.Column<bool>(nullable: true),
                    Format = table.Column<string>(nullable: true, defaultValue: "DD/MMM/YY"),
                    Location = table.Column<string>(nullable: true),
                    HasDecimals = table.Column<bool>(nullable: true),
                    DecimalPlaces = table.Column<short>(nullable: true, defaultValue: (short)2),
                    RefDataModelId = table.Column<Guid>(nullable: true),
                    IsSingleRecord = table.Column<bool>(nullable: true),
                    Length = table.Column<short>(nullable: true, defaultValue: (short)100),
                    IsMultiLine = table.Column<bool>(nullable: true),
                    DCAppStringDataDefinition_Format = table.Column<string>(nullable: true),
                    IsRegularExpressionFormat = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCAppDataDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DCAppEntityRowReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCAppEntityRowReferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DAppDataModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsInternal = table.Column<bool>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: true),
                    IsSingleRecord = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true),
                    DCAppGroupId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppDataModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppDataModels_DAppGroups_DCAppGroupId",
                        column: x => x.DCAppGroupId,
                        principalTable: "DAppGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAppDataModels_DAppGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DAppGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsInternal = table.Column<bool>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppFeatures_DAppGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DAppGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsInternal = table.Column<bool>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: true),
                    IsSystemDefined = table.Column<bool>(nullable: false),
                    SystemRole_Value = table.Column<string>(nullable: true),
                    SystemRole_DisplayName = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppRoles_DAppGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DAppGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppStructures",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Validity = table.Column<DateTime>(nullable: false),
                    InternalGroupId = table.Column<Guid>(nullable: true),
                    ExternalGroupId = table.Column<Guid>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppStructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppStructures_DAppGroups_ExternalGroupId",
                        column: x => x.ExternalGroupId,
                        principalTable: "DAppGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAppStructures_DAppGroups_InternalGroupId",
                        column: x => x.InternalGroupId,
                        principalTable: "DAppGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppDataChoiceItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ChoiceParentId = table.Column<Guid>(nullable: false),
                    ChoiceGroup = table.Column<string>(nullable: true),
                    ChoiceDisplayText = table.Column<string>(nullable: true),
                    ChoiceValue = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppDataChoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppDataChoiceItems_DCAppDataDefinitions_ChoiceParentId",
                        column: x => x.ChoiceParentId,
                        principalTable: "DCAppDataDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DAppWorkFlows",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DCAppFeatureId = table.Column<Guid>(nullable: false),
                    IsSingleRecord = table.Column<bool>(nullable: false),
                    RowAccessType_Value = table.Column<string>(nullable: true),
                    RowAccessType_DisplayName = table.Column<string>(nullable: true),
                    DCAppDataModelId = table.Column<Guid>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppWorkFlows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppWorkFlows_DAppFeatures_DCAppFeatureId",
                        column: x => x.DCAppFeatureId,
                        principalTable: "DAppFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    IsInternal = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true),
                    DCAppStructureId = table.Column<Guid>(nullable: true),
                    DCAppInternalUser_DCAppStructureId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_DAppStructures_DCAppStructureId",
                        column: x => x.DCAppStructureId,
                        principalTable: "DAppStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_DAppStructures_DCAppInternalUser_DCAppStructureId",
                        column: x => x.DCAppInternalUser_DCAppStructureId,
                        principalTable: "DAppStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DCAppCanonicalURL",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DCAppStructureId = table.Column<Guid>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCAppCanonicalURL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DCAppCanonicalURL_DAppStructures_DCAppStructureId",
                        column: x => x.DCAppStructureId,
                        principalTable: "DAppStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DCAppWorkFlowId = table.Column<Guid>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppPages_DAppWorkFlows_DCAppWorkFlowId",
                        column: x => x.DCAppWorkFlowId,
                        principalTable: "DAppWorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DAppRolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AccessLevel_Value = table.Column<string>(nullable: true),
                    AccessLevel_DisplayName = table.Column<string>(nullable: true),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppRolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppRolePermissions_DAppWorkFlows_WorkFlowId",
                        column: x => x.WorkFlowId,
                        principalTable: "DAppWorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DAppRoleAccessGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ParentRoleId = table.Column<Guid>(nullable: true),
                    ParentUserId = table.Column<string>(nullable: true),
                    WorkFlowId = table.Column<Guid>(nullable: false),
                    IsSource = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppRoleAccessGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppRoleAccessGroups_DAppRoles_ParentRoleId",
                        column: x => x.ParentRoleId,
                        principalTable: "DAppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAppRoleAccessGroups_AspNetUsers_ParentUserId",
                        column: x => x.ParentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAppRoleAccessGroups_DAppWorkFlows_WorkFlowId",
                        column: x => x.WorkFlowId,
                        principalTable: "DAppWorkFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DAppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    UserId1 = table.Column<string>(nullable: true),
                    DCAppUserRoleRoleId = table.Column<Guid>(nullable: true),
                    DCAppUserRoleUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_DAppUserRoles_DAppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "DAppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DAppUserRoles_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAppUserRoles_DAppUserRoles_DCAppUserRoleUserId_DCAppUserRoleRoleId",
                        columns: x => new { x.DCAppUserRoleUserId, x.DCAppUserRoleRoleId },
                        principalTable: "DAppUserRoles",
                        principalColumns: new[] { "UserId", "RoleId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppDataFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    DataDefinitionId = table.Column<Guid>(nullable: true),
                    DCAppDataModelId = table.Column<Guid>(nullable: false),
                    DCAppRolePermissionId = table.Column<Guid>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppDataFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppDataFields_DAppDataModels_DCAppDataModelId",
                        column: x => x.DCAppDataModelId,
                        principalTable: "DAppDataModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DAppDataFields_DAppRolePermissions_DCAppRolePermissionId",
                        column: x => x.DCAppRolePermissionId,
                        principalTable: "DAppRolePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAppDataFields_DCAppDataDefinitions_DataDefinitionId",
                        column: x => x.DataDefinitionId,
                        principalTable: "DCAppDataDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppRoleRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RuleType_Value = table.Column<string>(nullable: true),
                    RuleType_DisplayName = table.Column<string>(nullable: true),
                    ParentRoleId = table.Column<Guid>(nullable: true),
                    ChildRoleId = table.Column<Guid>(nullable: true),
                    DCAppRolePermissionId = table.Column<Guid>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppRoleRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppRoleRules_DAppRoles_ChildRoleId",
                        column: x => x.ChildRoleId,
                        principalTable: "DAppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAppRoleRules_DAppRolePermissions_DCAppRolePermissionId",
                        column: x => x.DCAppRolePermissionId,
                        principalTable: "DAppRolePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DAppRoleRules_DAppRoles_ParentRoleId",
                        column: x => x.ParentRoleId,
                        principalTable: "DAppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppControls",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DCAppPageId = table.Column<Guid>(nullable: false),
                    DataFieldId = table.Column<Guid>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppControls_DAppPages_DCAppPageId",
                        column: x => x.DCAppPageId,
                        principalTable: "DAppPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DAppControls_DAppDataFields_DataFieldId",
                        column: x => x.DataFieldId,
                        principalTable: "DAppDataFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppDataValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReferenceRowIds = table.Column<string>(nullable: true),
                    RowId = table.Column<Guid>(nullable: false),
                    DataFieldId = table.Column<Guid>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppDataValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppDataValues_DAppDataFields_DataFieldId",
                        column: x => x.DataFieldId,
                        principalTable: "DAppDataFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppControlAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DCAppControlId = table.Column<Guid>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppControlAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppControlAction_DAppControls_DCAppControlId",
                        column: x => x.DCAppControlId,
                        principalTable: "DAppControls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DAppControlProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DCAppControlId = table.Column<Guid>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAppControlProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DAppControlProperties_DAppControls_DCAppControlId",
                        column: x => x.DCAppControlId,
                        principalTable: "DAppControls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DCAppCapability",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CapabilityType = table.Column<int>(nullable: false),
                    DataId = table.Column<Guid>(nullable: true),
                    DCAppControlActionId = table.Column<Guid>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCAppCapability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DCAppCapability_DAppControlAction_DCAppControlActionId",
                        column: x => x.DCAppControlActionId,
                        principalTable: "DAppControlAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DCAppCapability_DAppDataModels_DataId",
                        column: x => x.DataId,
                        principalTable: "DAppDataModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DCAppStructureId",
                table: "AspNetUsers",
                column: "DCAppStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DCAppInternalUser_DCAppStructureId",
                table: "AspNetUsers",
                column: "DCAppInternalUser_DCAppStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Name",
                table: "AspNetUsers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DAppControlAction_DCAppControlId",
                table: "DAppControlAction",
                column: "DCAppControlId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppControlProperties_DCAppControlId",
                table: "DAppControlProperties",
                column: "DCAppControlId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppControls_DCAppPageId",
                table: "DAppControls",
                column: "DCAppPageId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppControls_DataFieldId",
                table: "DAppControls",
                column: "DataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataChoiceItems_ChoiceParentId",
                table: "DAppDataChoiceItems",
                column: "ChoiceParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataFields_DCAppDataModelId",
                table: "DAppDataFields",
                column: "DCAppDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataFields_DCAppRolePermissionId",
                table: "DAppDataFields",
                column: "DCAppRolePermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataFields_DataDefinitionId",
                table: "DAppDataFields",
                column: "DataDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataModels_DCAppGroupId",
                table: "DAppDataModels",
                column: "DCAppGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataModels_GroupId",
                table: "DAppDataModels",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataValues_DataFieldId",
                table: "DAppDataValues",
                column: "DataFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppDataValues_Name",
                table: "DAppDataValues",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DAppFeatures_GroupId",
                table: "DAppFeatures",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppGroups_ParentGroupId",
                table: "DAppGroups",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppPages_DCAppWorkFlowId",
                table: "DAppPages",
                column: "DCAppWorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppPages_Name",
                table: "DAppPages",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRoleAccessGroups_ParentRoleId",
                table: "DAppRoleAccessGroups",
                column: "ParentRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRoleAccessGroups_ParentUserId",
                table: "DAppRoleAccessGroups",
                column: "ParentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRoleAccessGroups_WorkFlowId",
                table: "DAppRoleAccessGroups",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRolePermissions_Name",
                table: "DAppRolePermissions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRolePermissions_WorkFlowId",
                table: "DAppRolePermissions",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRoleRules_ChildRoleId",
                table: "DAppRoleRules",
                column: "ChildRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRoleRules_DCAppRolePermissionId",
                table: "DAppRoleRules",
                column: "DCAppRolePermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRoleRules_Name",
                table: "DAppRoleRules",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRoleRules_ParentRoleId",
                table: "DAppRoleRules",
                column: "ParentRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRoles_GroupId",
                table: "DAppRoles",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppRoles_Name",
                table: "DAppRoles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DAppStructures_ExternalGroupId",
                table: "DAppStructures",
                column: "ExternalGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppStructures_InternalGroupId",
                table: "DAppStructures",
                column: "InternalGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppStructures_Name",
                table: "DAppStructures",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DAppUserRoles_RoleId",
                table: "DAppUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppUserRoles_UserId1",
                table: "DAppUserRoles",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_DAppUserRoles_DCAppUserRoleUserId_DCAppUserRoleRoleId",
                table: "DAppUserRoles",
                columns: new[] { "DCAppUserRoleUserId", "DCAppUserRoleRoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_DAppWorkFlows_DCAppFeatureId",
                table: "DAppWorkFlows",
                column: "DCAppFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_DAppWorkFlows_Name",
                table: "DAppWorkFlows",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DCAppCanonicalURL_DCAppStructureId",
                table: "DCAppCanonicalURL",
                column: "DCAppStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_DCAppCapability_DCAppControlActionId",
                table: "DCAppCapability",
                column: "DCAppControlActionId");

            migrationBuilder.CreateIndex(
                name: "IX_DCAppCapability_DataId",
                table: "DCAppCapability",
                column: "DataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DAppControlProperties");

            migrationBuilder.DropTable(
                name: "DAppDataChoiceItems");

            migrationBuilder.DropTable(
                name: "DAppDataValues");

            migrationBuilder.DropTable(
                name: "DAppRoleAccessGroups");

            migrationBuilder.DropTable(
                name: "DAppRoleRules");

            migrationBuilder.DropTable(
                name: "DAppUserRoles");

            migrationBuilder.DropTable(
                name: "DCAppCanonicalURL");

            migrationBuilder.DropTable(
                name: "DCAppCapability");

            migrationBuilder.DropTable(
                name: "DCAppEntityRowReferences");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DAppRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DAppControlAction");

            migrationBuilder.DropTable(
                name: "DAppStructures");

            migrationBuilder.DropTable(
                name: "DAppControls");

            migrationBuilder.DropTable(
                name: "DAppPages");

            migrationBuilder.DropTable(
                name: "DAppDataFields");

            migrationBuilder.DropTable(
                name: "DAppDataModels");

            migrationBuilder.DropTable(
                name: "DAppRolePermissions");

            migrationBuilder.DropTable(
                name: "DCAppDataDefinitions");

            migrationBuilder.DropTable(
                name: "DAppWorkFlows");

            migrationBuilder.DropTable(
                name: "DAppFeatures");

            migrationBuilder.DropTable(
                name: "DAppGroups");
        }
    }
}
