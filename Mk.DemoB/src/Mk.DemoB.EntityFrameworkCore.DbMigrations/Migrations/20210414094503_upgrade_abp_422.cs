using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoB.Migrations
{
    public partial class upgrade_abp_422 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_identity_server_api_claims_identity_server_api_resources_api~",
                table: "identity_server_api_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_identity_server_api_scope_claims_identity_server_api_scopes_~",
                table: "identity_server_api_scope_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_identity_server_api_scopes_identity_server_api_resources_api~",
                table: "identity_server_api_scopes");

            migrationBuilder.DropTable(
                name: "identity_server_api_secrets");

            migrationBuilder.DropTable(
                name: "identity_server_identity_claims");

            migrationBuilder.DropIndex(
                name: "IX_identity_server_device_flow_codes_user_code",
                table: "identity_server_device_flow_codes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_identity_server_client_properties",
                table: "identity_server_client_properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_identity_server_api_scopes",
                table: "identity_server_api_scopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_identity_server_api_scope_claims",
                table: "identity_server_api_scope_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_identity_server_api_claims",
                table: "identity_server_api_claims");

            migrationBuilder.DropColumn(
                name: "properties",
                table: "identity_server_identity_resources");

            migrationBuilder.DropColumn(
                name: "name",
                table: "identity_server_api_scope_claims");

            migrationBuilder.DropColumn(
                name: "properties",
                table: "identity_server_api_resources");

            migrationBuilder.RenameTable(
                name: "identity_server_api_claims",
                newName: "identity_server_api_resource_claims");

            migrationBuilder.RenameColumn(
                name: "api_resource_id",
                table: "identity_server_api_scopes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "api_resource_id",
                table: "identity_server_api_scope_claims",
                newName: "api_scope_id");

            migrationBuilder.AddColumn<DateTime>(
                name: "consumed_time",
                table: "identity_server_persisted_grants",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "identity_server_persisted_grants",
                type: "varchar(200) CHARACTER SET utf8mb4",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "session_id",
                table: "identity_server_persisted_grants",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "identity_server_device_flow_codes",
                type: "varchar(200) CHARACTER SET utf8mb4",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "session_id",
                table: "identity_server_device_flow_codes",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "allowed_identity_token_signing_algorithms",
                table: "identity_server_clients",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "require_request_object",
                table: "identity_server_clients",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "identity_server_client_properties",
                type: "varchar(300) CHARACTER SET utf8mb4",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000) CHARACTER SET utf8mb4",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<string>(
                name: "concurrency_stamp",
                table: "identity_server_api_scopes",
                type: "varchar(40) CHARACTER SET utf8mb4",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "creation_time",
                table: "identity_server_api_scopes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "creator_id",
                table: "identity_server_api_scopes",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "deleter_id",
                table: "identity_server_api_scopes",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deletion_time",
                table: "identity_server_api_scopes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "enabled",
                table: "identity_server_api_scopes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "extra_properties",
                table: "identity_server_api_scopes",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "identity_server_api_scopes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_modification_time",
                table: "identity_server_api_scopes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "last_modifier_id",
                table: "identity_server_api_scopes",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "allowed_access_token_signing_algorithms",
                table: "identity_server_api_resources",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "show_in_discovery_document",
                table: "identity_server_api_resources",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "customer_name",
                table: "demob_sale_order",
                type: "varchar(64) CHARACTER SET utf8mb4",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(64) CHARACTER SET utf8mb4",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_external",
                table: "abp_users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_identity_server_client_properties",
                table: "identity_server_client_properties",
                columns: new[] { "client_id", "key", "value" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_identity_server_api_scopes",
                table: "identity_server_api_scopes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_identity_server_api_scope_claims",
                table: "identity_server_api_scope_claims",
                columns: new[] { "api_scope_id", "type" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_identity_server_api_resource_claims",
                table: "identity_server_api_resource_claims",
                columns: new[] { "api_resource_id", "type" });

            migrationBuilder.CreateTable(
                name: "abp_link_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    source_user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    source_tenant_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    target_user_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    target_tenant_id = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_link_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abp_security_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false),
                    tenant_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    application_name = table.Column<string>(type: "varchar(96) CHARACTER SET utf8mb4", maxLength: 96, nullable: true),
                    identity = table.Column<string>(type: "varchar(96) CHARACTER SET utf8mb4", maxLength: 96, nullable: true),
                    action = table.Column<string>(type: "varchar(96) CHARACTER SET utf8mb4", maxLength: 96, nullable: true),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    user_name = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    tenant_name = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    client_id = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    correlation_id = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    client_ip_address = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    browser_info = table.Column<string>(type: "varchar(512) CHARACTER SET utf8mb4", maxLength: 512, nullable: true),
                    creation_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    extra_properties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_security_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_api_resource_properties",
                columns: table => new
                {
                    api_resource_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    key = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_api_resource_properties", x => new { x.api_resource_id, x.key, x.value });
                    table.ForeignKey(
                        name: "FK_identity_server_api_resource_properties_identity_server_api_~",
                        column: x => x.api_resource_id,
                        principalTable: "identity_server_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_api_resource_scopes",
                columns: table => new
                {
                    api_resource_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    scope = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_api_resource_scopes", x => new { x.api_resource_id, x.scope });
                    table.ForeignKey(
                        name: "FK_identity_server_api_resource_scopes_identity_server_api_reso~",
                        column: x => x.api_resource_id,
                        principalTable: "identity_server_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_api_resource_secrets",
                columns: table => new
                {
                    type = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false),
                    api_resource_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    description = table.Column<string>(type: "varchar(1000) CHARACTER SET utf8mb4", maxLength: 1000, nullable: true),
                    expiration = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_api_resource_secrets", x => new { x.api_resource_id, x.type, x.value });
                    table.ForeignKey(
                        name: "FK_identity_server_api_resource_secrets_identity_server_api_res~",
                        column: x => x.api_resource_id,
                        principalTable: "identity_server_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_api_scope_properties",
                columns: table => new
                {
                    api_scope_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    key = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_api_scope_properties", x => new { x.api_scope_id, x.key, x.value });
                    table.ForeignKey(
                        name: "FK_identity_server_api_scope_properties_identity_server_api_sco~",
                        column: x => x.api_scope_id,
                        principalTable: "identity_server_api_scopes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_identity_resource_claims",
                columns: table => new
                {
                    type = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: false),
                    identity_resource_id = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_identity_resource_claims", x => new { x.identity_resource_id, x.type });
                    table.ForeignKey(
                        name: "FK_identity_server_identity_resource_claims_identity_server_ide~",
                        column: x => x.identity_resource_id,
                        principalTable: "identity_server_identity_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_identity_resource_properties",
                columns: table => new
                {
                    identity_resource_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    key = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_identity_resource_properties", x => new { x.identity_resource_id, x.key, x.value });
                    table.ForeignKey(
                        name: "FK_identity_server_identity_resource_properties_identity_server~",
                        column: x => x.identity_resource_id,
                        principalTable: "identity_server_identity_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_identity_server_persisted_grants_subject_id_session_id_type",
                table: "identity_server_persisted_grants",
                columns: new[] { "subject_id", "session_id", "type" });

            migrationBuilder.CreateIndex(
                name: "IX_identity_server_device_flow_codes_user_code",
                table: "identity_server_device_flow_codes",
                column: "user_code");

            migrationBuilder.CreateIndex(
                name: "IX_abp_link_users_source_user_id_source_tenant_id_target_user_i~",
                table: "abp_link_users",
                columns: new[] { "source_user_id", "source_tenant_id", "target_user_id", "target_tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_abp_security_logs_tenant_id_action",
                table: "abp_security_logs",
                columns: new[] { "tenant_id", "action" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_security_logs_tenant_id_application_name",
                table: "abp_security_logs",
                columns: new[] { "tenant_id", "application_name" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_security_logs_tenant_id_identity",
                table: "abp_security_logs",
                columns: new[] { "tenant_id", "identity" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_security_logs_tenant_id_user_id",
                table: "abp_security_logs",
                columns: new[] { "tenant_id", "user_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_identity_server_api_resource_claims_identity_server_api_reso~",
                table: "identity_server_api_resource_claims",
                column: "api_resource_id",
                principalTable: "identity_server_api_resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_identity_server_api_scope_claims_identity_server_api_scopes_~",
                table: "identity_server_api_scope_claims",
                column: "api_scope_id",
                principalTable: "identity_server_api_scopes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_identity_server_api_resource_claims_identity_server_api_reso~",
                table: "identity_server_api_resource_claims");

            migrationBuilder.DropForeignKey(
                name: "FK_identity_server_api_scope_claims_identity_server_api_scopes_~",
                table: "identity_server_api_scope_claims");

            migrationBuilder.DropTable(
                name: "abp_link_users");

            migrationBuilder.DropTable(
                name: "abp_security_logs");

            migrationBuilder.DropTable(
                name: "identity_server_api_resource_properties");

            migrationBuilder.DropTable(
                name: "identity_server_api_resource_scopes");

            migrationBuilder.DropTable(
                name: "identity_server_api_resource_secrets");

            migrationBuilder.DropTable(
                name: "identity_server_api_scope_properties");

            migrationBuilder.DropTable(
                name: "identity_server_identity_resource_claims");

            migrationBuilder.DropTable(
                name: "identity_server_identity_resource_properties");

            migrationBuilder.DropIndex(
                name: "IX_identity_server_persisted_grants_subject_id_session_id_type",
                table: "identity_server_persisted_grants");

            migrationBuilder.DropIndex(
                name: "IX_identity_server_device_flow_codes_user_code",
                table: "identity_server_device_flow_codes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_identity_server_client_properties",
                table: "identity_server_client_properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_identity_server_api_scopes",
                table: "identity_server_api_scopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_identity_server_api_scope_claims",
                table: "identity_server_api_scope_claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_identity_server_api_resource_claims",
                table: "identity_server_api_resource_claims");

            migrationBuilder.DropColumn(
                name: "consumed_time",
                table: "identity_server_persisted_grants");

            migrationBuilder.DropColumn(
                name: "description",
                table: "identity_server_persisted_grants");

            migrationBuilder.DropColumn(
                name: "session_id",
                table: "identity_server_persisted_grants");

            migrationBuilder.DropColumn(
                name: "description",
                table: "identity_server_device_flow_codes");

            migrationBuilder.DropColumn(
                name: "session_id",
                table: "identity_server_device_flow_codes");

            migrationBuilder.DropColumn(
                name: "allowed_identity_token_signing_algorithms",
                table: "identity_server_clients");

            migrationBuilder.DropColumn(
                name: "require_request_object",
                table: "identity_server_clients");

            migrationBuilder.DropColumn(
                name: "concurrency_stamp",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "creation_time",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "deleter_id",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "deletion_time",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "enabled",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "extra_properties",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "last_modification_time",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "last_modifier_id",
                table: "identity_server_api_scopes");

            migrationBuilder.DropColumn(
                name: "allowed_access_token_signing_algorithms",
                table: "identity_server_api_resources");

            migrationBuilder.DropColumn(
                name: "show_in_discovery_document",
                table: "identity_server_api_resources");

            migrationBuilder.DropColumn(
                name: "is_external",
                table: "abp_users");

            migrationBuilder.RenameTable(
                name: "identity_server_api_resource_claims",
                newName: "identity_server_api_claims");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "identity_server_api_scopes",
                newName: "api_resource_id");

            migrationBuilder.RenameColumn(
                name: "api_scope_id",
                table: "identity_server_api_scope_claims",
                newName: "api_resource_id");

            migrationBuilder.AddColumn<string>(
                name: "properties",
                table: "identity_server_identity_resources",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "identity_server_client_properties",
                type: "varchar(2000) CHARACTER SET utf8mb4",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300) CHARACTER SET utf8mb4",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "identity_server_api_scope_claims",
                type: "varchar(200) CHARACTER SET utf8mb4",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "properties",
                table: "identity_server_api_resources",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "customer_name",
                table: "demob_sale_order",
                type: "varchar(64) CHARACTER SET utf8mb4",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(64) CHARACTER SET utf8mb4",
                oldMaxLength: 64);

            migrationBuilder.AddPrimaryKey(
                name: "PK_identity_server_client_properties",
                table: "identity_server_client_properties",
                columns: new[] { "client_id", "key" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_identity_server_api_scopes",
                table: "identity_server_api_scopes",
                columns: new[] { "api_resource_id", "name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_identity_server_api_scope_claims",
                table: "identity_server_api_scope_claims",
                columns: new[] { "api_resource_id", "name", "type" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_identity_server_api_claims",
                table: "identity_server_api_claims",
                columns: new[] { "api_resource_id", "type" });

            migrationBuilder.CreateTable(
                name: "identity_server_api_secrets",
                columns: table => new
                {
                    api_resource_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    type = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "varchar(2000) CHARACTER SET utf8mb4", maxLength: 2000, nullable: true),
                    expiration = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_api_secrets", x => new { x.api_resource_id, x.type, x.value });
                    table.ForeignKey(
                        name: "FK_identity_server_api_secrets_identity_server_api_resources_ap~",
                        column: x => x.api_resource_id,
                        principalTable: "identity_server_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_identity_claims",
                columns: table => new
                {
                    identity_resource_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    type = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_identity_claims", x => new { x.identity_resource_id, x.type });
                    table.ForeignKey(
                        name: "FK_identity_server_identity_claims_identity_server_identity_res~",
                        column: x => x.identity_resource_id,
                        principalTable: "identity_server_identity_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_identity_server_device_flow_codes_user_code",
                table: "identity_server_device_flow_codes",
                column: "user_code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_identity_server_api_claims_identity_server_api_resources_api~",
                table: "identity_server_api_claims",
                column: "api_resource_id",
                principalTable: "identity_server_api_resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_identity_server_api_scope_claims_identity_server_api_scopes_~",
                table: "identity_server_api_scope_claims",
                columns: new[] { "api_resource_id", "name" },
                principalTable: "identity_server_api_scopes",
                principalColumns: new[] { "api_resource_id", "name" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_identity_server_api_scopes_identity_server_api_resources_api~",
                table: "identity_server_api_scopes",
                column: "api_resource_id",
                principalTable: "identity_server_api_resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
