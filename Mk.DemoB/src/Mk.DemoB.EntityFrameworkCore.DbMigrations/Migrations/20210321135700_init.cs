using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mk.DemoB.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "abp_audit_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    application_name = table.Column<string>(maxLength: 96, nullable: true),
                    user_id = table.Column<Guid>(nullable: true),
                    user_name = table.Column<string>(maxLength: 256, nullable: true),
                    tenant_id = table.Column<Guid>(nullable: true),
                    tenant_name = table.Column<string>(nullable: true),
                    impersonator_user_id = table.Column<Guid>(nullable: true),
                    impersonator_tenant_id = table.Column<Guid>(nullable: true),
                    execution_time = table.Column<DateTime>(nullable: false),
                    execution_duration = table.Column<int>(nullable: false),
                    client_ip_address = table.Column<string>(maxLength: 64, nullable: true),
                    client_name = table.Column<string>(maxLength: 128, nullable: true),
                    client_id = table.Column<string>(maxLength: 64, nullable: true),
                    correlation_id = table.Column<string>(maxLength: 64, nullable: true),
                    browser_info = table.Column<string>(maxLength: 512, nullable: true),
                    http_method = table.Column<string>(maxLength: 16, nullable: true),
                    url = table.Column<string>(maxLength: 256, nullable: true),
                    exceptions = table.Column<string>(maxLength: 4000, nullable: true),
                    comments = table.Column<string>(maxLength: 256, nullable: true),
                    http_status_code = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_audit_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abp_background_jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    job_name = table.Column<string>(maxLength: 128, nullable: false),
                    job_args = table.Column<string>(maxLength: 1048576, nullable: false),
                    try_count = table.Column<short>(nullable: false, defaultValue: (short)0),
                    creation_time = table.Column<DateTime>(nullable: false),
                    next_try_time = table.Column<DateTime>(nullable: false),
                    last_try_time = table.Column<DateTime>(nullable: true),
                    is_abandoned = table.Column<bool>(nullable: false, defaultValue: false),
                    priority = table.Column<byte>(nullable: false, defaultValue: (byte)15)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_background_jobs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abp_claim_types",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    name = table.Column<string>(maxLength: 256, nullable: false),
                    required = table.Column<bool>(nullable: false),
                    is_static = table.Column<bool>(nullable: false),
                    regex = table.Column<string>(maxLength: 512, nullable: true),
                    regex_description = table.Column<string>(maxLength: 128, nullable: true),
                    description = table.Column<string>(maxLength: 256, nullable: true),
                    value_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_claim_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abp_feature_values",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(maxLength: 128, nullable: false),
                    value = table.Column<string>(maxLength: 128, nullable: false),
                    provider_name = table.Column<string>(maxLength: 64, nullable: true),
                    provider_key = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_feature_values", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abp_organization_units",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    tenant_id = table.Column<Guid>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    code = table.Column<string>(maxLength: 95, nullable: false),
                    display_name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_organization_units", x => x.id);
                    table.ForeignKey(
                        name: "FK_abp_organization_units_abp_organization_units_parent_id",
                        column: x => x.parent_id,
                        principalTable: "abp_organization_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "abp_permission_grants",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    tenant_id = table.Column<Guid>(nullable: true),
                    name = table.Column<string>(maxLength: 128, nullable: false),
                    provider_name = table.Column<string>(maxLength: 64, nullable: false),
                    provider_key = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_permission_grants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abp_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    tenant_id = table.Column<Guid>(nullable: true),
                    name = table.Column<string>(maxLength: 256, nullable: false),
                    normalized_name = table.Column<string>(maxLength: 256, nullable: false),
                    is_default = table.Column<bool>(nullable: false),
                    is_static = table.Column<bool>(nullable: false),
                    is_public = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abp_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(maxLength: 128, nullable: false),
                    value = table.Column<string>(maxLength: 2048, nullable: false),
                    provider_name = table.Column<string>(maxLength: 64, nullable: true),
                    provider_key = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abp_tenants",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_tenants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abp_users",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    tenant_id = table.Column<Guid>(nullable: true),
                    user_name = table.Column<string>(maxLength: 256, nullable: false),
                    normalized_user_name = table.Column<string>(maxLength: 256, nullable: false),
                    name = table.Column<string>(maxLength: 64, nullable: true),
                    surname = table.Column<string>(maxLength: 64, nullable: true),
                    email = table.Column<string>(maxLength: 256, nullable: false),
                    normalized_email = table.Column<string>(maxLength: 256, nullable: false),
                    email_confirmed = table.Column<bool>(nullable: false, defaultValue: false),
                    password_hash = table.Column<string>(maxLength: 256, nullable: true),
                    security_stamp = table.Column<string>(maxLength: 256, nullable: false),
                    phone_number = table.Column<string>(maxLength: 16, nullable: true),
                    phone_number_confirmed = table.Column<bool>(nullable: false, defaultValue: false),
                    two_factor_enabled = table.Column<bool>(nullable: false, defaultValue: false),
                    lockout_end = table.Column<DateTimeOffset>(nullable: true),
                    lockout_enabled = table.Column<bool>(nullable: false, defaultValue: false),
                    access_failed_count = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "demob_capture_currency",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    currency_code_from = table.Column<string>(maxLength: 8, nullable: false),
                    currency_code_to = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_capture_currency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "demob_exchange_rate",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    currency_code_from = table.Column<string>(maxLength: 8, nullable: false),
                    currency_code_to = table.Column<string>(maxLength: 8, nullable: false),
                    buy_price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    data_from_url = table.Column<string>(maxLength: 256, nullable: false),
                    capture_batch_number = table.Column<string>(maxLength: 64, nullable: false),
                    capture_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_exchange_rate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "demob_exchange_rate_capture_batch",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    capture_batch_number = table.Column<string>(maxLength: 64, nullable: false),
                    capture_time = table.Column<DateTime>(nullable: false),
                    is_success = table.Column<bool>(nullable: false),
                    remark = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_exchange_rate_capture_batch", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "demob_sale_order",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    tenant_id = table.Column<Guid>(nullable: true),
                    order_no = table.Column<string>(maxLength: 64, nullable: false),
                    order_time = table.Column<DateTime>(nullable: false),
                    currency = table.Column<string>(maxLength: 8, nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    order_status = table.Column<int>(nullable: false),
                    customer_name = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_sale_order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_api_resources",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    display_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    enabled = table.Column<bool>(nullable: false),
                    properties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_api_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_clients",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    client_id = table.Column<string>(maxLength: 200, nullable: false),
                    client_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    client_uri = table.Column<string>(maxLength: 2000, nullable: true),
                    logo_uri = table.Column<string>(maxLength: 2000, nullable: true),
                    enabled = table.Column<bool>(nullable: false),
                    protocol_type = table.Column<string>(maxLength: 200, nullable: false),
                    require_client_secret = table.Column<bool>(nullable: false),
                    require_consent = table.Column<bool>(nullable: false),
                    allow_remember_consent = table.Column<bool>(nullable: false),
                    always_include_user_claims_in_id_token = table.Column<bool>(nullable: false),
                    require_pkce = table.Column<bool>(nullable: false),
                    allow_plain_text_pkce = table.Column<bool>(nullable: false),
                    allow_access_tokens_via_browser = table.Column<bool>(nullable: false),
                    front_channel_logout_uri = table.Column<string>(maxLength: 2000, nullable: true),
                    front_channel_logout_session_required = table.Column<bool>(nullable: false),
                    back_channel_logout_uri = table.Column<string>(maxLength: 2000, nullable: true),
                    back_channel_logout_session_required = table.Column<bool>(nullable: false),
                    allow_offline_access = table.Column<bool>(nullable: false),
                    identity_token_lifetime = table.Column<int>(nullable: false),
                    access_token_lifetime = table.Column<int>(nullable: false),
                    authorization_code_lifetime = table.Column<int>(nullable: false),
                    consent_lifetime = table.Column<int>(nullable: true),
                    absolute_refresh_token_lifetime = table.Column<int>(nullable: false),
                    sliding_refresh_token_lifetime = table.Column<int>(nullable: false),
                    refresh_token_usage = table.Column<int>(nullable: false),
                    update_access_token_claims_on_refresh = table.Column<bool>(nullable: false),
                    refresh_token_expiration = table.Column<int>(nullable: false),
                    access_token_type = table.Column<int>(nullable: false),
                    enable_local_login = table.Column<bool>(nullable: false),
                    include_jwt_id = table.Column<bool>(nullable: false),
                    always_send_client_claims = table.Column<bool>(nullable: false),
                    client_claims_prefix = table.Column<string>(maxLength: 200, nullable: true),
                    pair_wise_subject_salt = table.Column<string>(maxLength: 200, nullable: true),
                    user_sso_lifetime = table.Column<int>(nullable: true),
                    user_code_type = table.Column<string>(maxLength: 100, nullable: true),
                    device_code_lifetime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_clients", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_device_flow_codes",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    device_code = table.Column<string>(maxLength: 200, nullable: false),
                    user_code = table.Column<string>(maxLength: 200, nullable: false),
                    subject_id = table.Column<string>(maxLength: 200, nullable: true),
                    client_id = table.Column<string>(maxLength: 200, nullable: false),
                    expiration = table.Column<DateTime>(nullable: false),
                    data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_device_flow_codes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_identity_resources",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    display_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    enabled = table.Column<bool>(nullable: false),
                    required = table.Column<bool>(nullable: false),
                    emphasize = table.Column<bool>(nullable: false),
                    show_in_discovery_document = table.Column<bool>(nullable: false),
                    properties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_identity_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_persisted_grants",
                columns: table => new
                {
                    key = table.Column<string>(maxLength: 200, nullable: false),
                    id = table.Column<Guid>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 40, nullable: true),
                    type = table.Column<string>(maxLength: 50, nullable: false),
                    subject_id = table.Column<string>(maxLength: 200, nullable: true),
                    client_id = table.Column<string>(maxLength: 200, nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    expiration = table.Column<DateTime>(nullable: true),
                    data = table.Column<string>(maxLength: 10000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_persisted_grants", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "abp_audit_log_actions",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    tenant_id = table.Column<Guid>(nullable: true),
                    audit_log_id = table.Column<Guid>(nullable: false),
                    service_name = table.Column<string>(maxLength: 256, nullable: true),
                    method_name = table.Column<string>(maxLength: 128, nullable: true),
                    parameters = table.Column<string>(maxLength: 2000, nullable: true),
                    execution_time = table.Column<DateTime>(nullable: false),
                    execution_duration = table.Column<int>(nullable: false),
                    extra_properties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_audit_log_actions", x => x.id);
                    table.ForeignKey(
                        name: "FK_abp_audit_log_actions_abp_audit_logs_audit_log_id",
                        column: x => x.audit_log_id,
                        principalTable: "abp_audit_logs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abp_entity_changes",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    audit_log_id = table.Column<Guid>(nullable: false),
                    tenant_id = table.Column<Guid>(nullable: true),
                    change_time = table.Column<DateTime>(nullable: false),
                    change_type = table.Column<byte>(nullable: false),
                    entity_tenant_id = table.Column<Guid>(nullable: true),
                    entity_id = table.Column<string>(maxLength: 128, nullable: false),
                    entity_type_full_name = table.Column<string>(maxLength: 128, nullable: false),
                    extra_properties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_entity_changes", x => x.id);
                    table.ForeignKey(
                        name: "FK_abp_entity_changes_abp_audit_logs_audit_log_id",
                        column: x => x.audit_log_id,
                        principalTable: "abp_audit_logs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abp_organization_unit_roles",
                columns: table => new
                {
                    role_id = table.Column<Guid>(nullable: false),
                    organization_unit_id = table.Column<Guid>(nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    tenant_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_organization_unit_roles", x => new { x.organization_unit_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_abp_organization_unit_roles_abp_organization_units_organizat~",
                        column: x => x.organization_unit_id,
                        principalTable: "abp_organization_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_abp_organization_unit_roles_abp_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "abp_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abp_role_claims",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    tenant_id = table.Column<Guid>(nullable: true),
                    claim_type = table.Column<string>(maxLength: 256, nullable: false),
                    claim_value = table.Column<string>(maxLength: 1024, nullable: true),
                    role_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "FK_abp_role_claims_abp_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "abp_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abp_tenant_connection_strings",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(maxLength: 64, nullable: false),
                    value = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_tenant_connection_strings", x => new { x.tenant_id, x.name });
                    table.ForeignKey(
                        name: "FK_abp_tenant_connection_strings_abp_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "abp_tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abp_user_claims",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    tenant_id = table.Column<Guid>(nullable: true),
                    claim_type = table.Column<string>(maxLength: 256, nullable: false),
                    claim_value = table.Column<string>(maxLength: 1024, nullable: true),
                    user_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "FK_abp_user_claims_abp_users_user_id",
                        column: x => x.user_id,
                        principalTable: "abp_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abp_user_logins",
                columns: table => new
                {
                    user_id = table.Column<Guid>(nullable: false),
                    login_provider = table.Column<string>(maxLength: 64, nullable: false),
                    tenant_id = table.Column<Guid>(nullable: true),
                    provider_key = table.Column<string>(maxLength: 196, nullable: false),
                    provider_display_name = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_user_logins", x => new { x.user_id, x.login_provider });
                    table.ForeignKey(
                        name: "FK_abp_user_logins_abp_users_user_id",
                        column: x => x.user_id,
                        principalTable: "abp_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abp_user_organization_units",
                columns: table => new
                {
                    user_id = table.Column<Guid>(nullable: false),
                    organization_unit_id = table.Column<Guid>(nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    tenant_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_user_organization_units", x => new { x.organization_unit_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_abp_user_organization_units_abp_organization_units_organizat~",
                        column: x => x.organization_unit_id,
                        principalTable: "abp_organization_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_abp_user_organization_units_abp_users_user_id",
                        column: x => x.user_id,
                        principalTable: "abp_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abp_user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(nullable: false),
                    role_id = table.Column<Guid>(nullable: false),
                    tenant_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_abp_user_roles_abp_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "abp_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_abp_user_roles_abp_users_user_id",
                        column: x => x.user_id,
                        principalTable: "abp_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "abp_user_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(nullable: false),
                    login_provider = table.Column<string>(maxLength: 64, nullable: false),
                    name = table.Column<string>(maxLength: 128, nullable: false),
                    tenant_id = table.Column<Guid>(nullable: true),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "FK_abp_user_tokens_abp_users_user_id",
                        column: x => x.user_id,
                        principalTable: "abp_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "demob_sale_order_detail",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    creation_time = table.Column<DateTime>(nullable: false),
                    creator_id = table.Column<Guid>(nullable: true),
                    last_modification_time = table.Column<DateTime>(nullable: true),
                    last_modifier_id = table.Column<Guid>(nullable: true),
                    is_deleted = table.Column<bool>(nullable: false, defaultValue: false),
                    deleter_id = table.Column<Guid>(nullable: true),
                    deletion_time = table.Column<DateTime>(nullable: true),
                    tenant_id = table.Column<Guid>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: false),
                    line_no = table.Column<int>(nullable: false),
                    product_sku_code = table.Column<string>(maxLength: 64, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demob_sale_order_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_demob_sale_order_detail_demob_sale_order_parent_id",
                        column: x => x.parent_id,
                        principalTable: "demob_sale_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_api_claims",
                columns: table => new
                {
                    type = table.Column<string>(maxLength: 200, nullable: false),
                    api_resource_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_api_claims", x => new { x.api_resource_id, x.type });
                    table.ForeignKey(
                        name: "FK_identity_server_api_claims_identity_server_api_resources_api~",
                        column: x => x.api_resource_id,
                        principalTable: "identity_server_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_api_scopes",
                columns: table => new
                {
                    api_resource_id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false),
                    display_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    required = table.Column<bool>(nullable: false),
                    emphasize = table.Column<bool>(nullable: false),
                    show_in_discovery_document = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_api_scopes", x => new { x.api_resource_id, x.name });
                    table.ForeignKey(
                        name: "FK_identity_server_api_scopes_identity_server_api_resources_api~",
                        column: x => x.api_resource_id,
                        principalTable: "identity_server_api_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_api_secrets",
                columns: table => new
                {
                    type = table.Column<string>(maxLength: 250, nullable: false),
                    value = table.Column<string>(maxLength: 300, nullable: false),
                    api_resource_id = table.Column<Guid>(nullable: false),
                    description = table.Column<string>(maxLength: 2000, nullable: true),
                    expiration = table.Column<DateTime>(nullable: true)
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
                name: "identity_server_client_claims",
                columns: table => new
                {
                    client_id = table.Column<Guid>(nullable: false),
                    type = table.Column<string>(maxLength: 250, nullable: false),
                    value = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_client_claims", x => new { x.client_id, x.type, x.value });
                    table.ForeignKey(
                        name: "FK_identity_server_client_claims_identity_server_clients_client~",
                        column: x => x.client_id,
                        principalTable: "identity_server_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_client_cors_origins",
                columns: table => new
                {
                    client_id = table.Column<Guid>(nullable: false),
                    origin = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_client_cors_origins", x => new { x.client_id, x.origin });
                    table.ForeignKey(
                        name: "FK_identity_server_client_cors_origins_identity_server_clients_~",
                        column: x => x.client_id,
                        principalTable: "identity_server_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_client_grant_types",
                columns: table => new
                {
                    client_id = table.Column<Guid>(nullable: false),
                    grant_type = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_client_grant_types", x => new { x.client_id, x.grant_type });
                    table.ForeignKey(
                        name: "FK_identity_server_client_grant_types_identity_server_clients_c~",
                        column: x => x.client_id,
                        principalTable: "identity_server_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_client_id_prestrictions",
                columns: table => new
                {
                    client_id = table.Column<Guid>(nullable: false),
                    provider = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_client_id_prestrictions", x => new { x.client_id, x.provider });
                    table.ForeignKey(
                        name: "FK_identity_server_client_id_prestrictions_identity_server_clie~",
                        column: x => x.client_id,
                        principalTable: "identity_server_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_client_post_logout_redirect_uris",
                columns: table => new
                {
                    client_id = table.Column<Guid>(nullable: false),
                    post_logout_redirect_uri = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_client_post_logout_redirect_uris", x => new { x.client_id, x.post_logout_redirect_uri });
                    table.ForeignKey(
                        name: "FK_identity_server_client_post_logout_redirect_uris_identity_se~",
                        column: x => x.client_id,
                        principalTable: "identity_server_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_client_properties",
                columns: table => new
                {
                    client_id = table.Column<Guid>(nullable: false),
                    key = table.Column<string>(maxLength: 250, nullable: false),
                    value = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_client_properties", x => new { x.client_id, x.key });
                    table.ForeignKey(
                        name: "FK_identity_server_client_properties_identity_server_clients_cl~",
                        column: x => x.client_id,
                        principalTable: "identity_server_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_client_redirect_uris",
                columns: table => new
                {
                    client_id = table.Column<Guid>(nullable: false),
                    redirect_uri = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_client_redirect_uris", x => new { x.client_id, x.redirect_uri });
                    table.ForeignKey(
                        name: "FK_identity_server_client_redirect_uris_identity_server_clients~",
                        column: x => x.client_id,
                        principalTable: "identity_server_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_client_scopes",
                columns: table => new
                {
                    client_id = table.Column<Guid>(nullable: false),
                    scope = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_client_scopes", x => new { x.client_id, x.scope });
                    table.ForeignKey(
                        name: "FK_identity_server_client_scopes_identity_server_clients_client~",
                        column: x => x.client_id,
                        principalTable: "identity_server_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_client_secrets",
                columns: table => new
                {
                    type = table.Column<string>(maxLength: 250, nullable: false),
                    value = table.Column<string>(maxLength: 300, nullable: false),
                    client_id = table.Column<Guid>(nullable: false),
                    description = table.Column<string>(maxLength: 2000, nullable: true),
                    expiration = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_client_secrets", x => new { x.client_id, x.type, x.value });
                    table.ForeignKey(
                        name: "FK_identity_server_client_secrets_identity_server_clients_clien~",
                        column: x => x.client_id,
                        principalTable: "identity_server_clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_identity_claims",
                columns: table => new
                {
                    type = table.Column<string>(maxLength: 200, nullable: false),
                    identity_resource_id = table.Column<Guid>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "abp_entity_property_changes",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    tenant_id = table.Column<Guid>(nullable: true),
                    entity_change_id = table.Column<Guid>(nullable: false),
                    new_value = table.Column<string>(maxLength: 512, nullable: true),
                    original_value = table.Column<string>(maxLength: 512, nullable: true),
                    property_name = table.Column<string>(maxLength: 128, nullable: false),
                    property_type_full_name = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_abp_entity_property_changes", x => x.id);
                    table.ForeignKey(
                        name: "FK_abp_entity_property_changes_abp_entity_changes_entity_change~",
                        column: x => x.entity_change_id,
                        principalTable: "abp_entity_changes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identity_server_api_scope_claims",
                columns: table => new
                {
                    type = table.Column<string>(maxLength: 200, nullable: false),
                    api_resource_id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identity_server_api_scope_claims", x => new { x.api_resource_id, x.name, x.type });
                    table.ForeignKey(
                        name: "FK_identity_server_api_scope_claims_identity_server_api_scopes_~",
                        columns: x => new { x.api_resource_id, x.name },
                        principalTable: "identity_server_api_scopes",
                        principalColumns: new[] { "api_resource_id", "name" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_abp_audit_log_actions_audit_log_id",
                table: "abp_audit_log_actions",
                column: "audit_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_abp_audit_log_actions_tenant_id_service_name_method_name_exe~",
                table: "abp_audit_log_actions",
                columns: new[] { "tenant_id", "service_name", "method_name", "execution_time" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_audit_logs_tenant_id_execution_time",
                table: "abp_audit_logs",
                columns: new[] { "tenant_id", "execution_time" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_audit_logs_tenant_id_user_id_execution_time",
                table: "abp_audit_logs",
                columns: new[] { "tenant_id", "user_id", "execution_time" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_background_jobs_is_abandoned_next_try_time",
                table: "abp_background_jobs",
                columns: new[] { "is_abandoned", "next_try_time" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_entity_changes_audit_log_id",
                table: "abp_entity_changes",
                column: "audit_log_id");

            migrationBuilder.CreateIndex(
                name: "IX_abp_entity_changes_tenant_id_entity_type_full_name_entity_id",
                table: "abp_entity_changes",
                columns: new[] { "tenant_id", "entity_type_full_name", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_entity_property_changes_entity_change_id",
                table: "abp_entity_property_changes",
                column: "entity_change_id");

            migrationBuilder.CreateIndex(
                name: "IX_abp_feature_values_name_provider_name_provider_key",
                table: "abp_feature_values",
                columns: new[] { "name", "provider_name", "provider_key" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_organization_unit_roles_role_id_organization_unit_id",
                table: "abp_organization_unit_roles",
                columns: new[] { "role_id", "organization_unit_id" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_organization_units_code",
                table: "abp_organization_units",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "IX_abp_organization_units_parent_id",
                table: "abp_organization_units",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_abp_permission_grants_name_provider_name_provider_key",
                table: "abp_permission_grants",
                columns: new[] { "name", "provider_name", "provider_key" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_role_claims_role_id",
                table: "abp_role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_abp_roles_normalized_name",
                table: "abp_roles",
                column: "normalized_name");

            migrationBuilder.CreateIndex(
                name: "IX_abp_settings_name_provider_name_provider_key",
                table: "abp_settings",
                columns: new[] { "name", "provider_name", "provider_key" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_tenants_name",
                table: "abp_tenants",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_abp_user_claims_user_id",
                table: "abp_user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_abp_user_logins_login_provider_provider_key",
                table: "abp_user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_user_organization_units_user_id_organization_unit_id",
                table: "abp_user_organization_units",
                columns: new[] { "user_id", "organization_unit_id" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_user_roles_role_id_user_id",
                table: "abp_user_roles",
                columns: new[] { "role_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_abp_users_email",
                table: "abp_users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_abp_users_normalized_email",
                table: "abp_users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "IX_abp_users_normalized_user_name",
                table: "abp_users",
                column: "normalized_user_name");

            migrationBuilder.CreateIndex(
                name: "IX_abp_users_user_name",
                table: "abp_users",
                column: "user_name");

            migrationBuilder.CreateIndex(
                name: "IX_demob_sale_order_detail_parent_id",
                table: "demob_sale_order_detail",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_identity_server_clients_client_id",
                table: "identity_server_clients",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_identity_server_device_flow_codes_device_code",
                table: "identity_server_device_flow_codes",
                column: "device_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_server_device_flow_codes_expiration",
                table: "identity_server_device_flow_codes",
                column: "expiration");

            migrationBuilder.CreateIndex(
                name: "IX_identity_server_device_flow_codes_user_code",
                table: "identity_server_device_flow_codes",
                column: "user_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identity_server_persisted_grants_expiration",
                table: "identity_server_persisted_grants",
                column: "expiration");

            migrationBuilder.CreateIndex(
                name: "IX_identity_server_persisted_grants_subject_id_client_id_type",
                table: "identity_server_persisted_grants",
                columns: new[] { "subject_id", "client_id", "type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "abp_audit_log_actions");

            migrationBuilder.DropTable(
                name: "abp_background_jobs");

            migrationBuilder.DropTable(
                name: "abp_claim_types");

            migrationBuilder.DropTable(
                name: "abp_entity_property_changes");

            migrationBuilder.DropTable(
                name: "abp_feature_values");

            migrationBuilder.DropTable(
                name: "abp_organization_unit_roles");

            migrationBuilder.DropTable(
                name: "abp_permission_grants");

            migrationBuilder.DropTable(
                name: "abp_role_claims");

            migrationBuilder.DropTable(
                name: "abp_settings");

            migrationBuilder.DropTable(
                name: "abp_tenant_connection_strings");

            migrationBuilder.DropTable(
                name: "abp_user_claims");

            migrationBuilder.DropTable(
                name: "abp_user_logins");

            migrationBuilder.DropTable(
                name: "abp_user_organization_units");

            migrationBuilder.DropTable(
                name: "abp_user_roles");

            migrationBuilder.DropTable(
                name: "abp_user_tokens");

            migrationBuilder.DropTable(
                name: "demob_capture_currency");

            migrationBuilder.DropTable(
                name: "demob_exchange_rate");

            migrationBuilder.DropTable(
                name: "demob_exchange_rate_capture_batch");

            migrationBuilder.DropTable(
                name: "demob_sale_order_detail");

            migrationBuilder.DropTable(
                name: "identity_server_api_claims");

            migrationBuilder.DropTable(
                name: "identity_server_api_scope_claims");

            migrationBuilder.DropTable(
                name: "identity_server_api_secrets");

            migrationBuilder.DropTable(
                name: "identity_server_client_claims");

            migrationBuilder.DropTable(
                name: "identity_server_client_cors_origins");

            migrationBuilder.DropTable(
                name: "identity_server_client_grant_types");

            migrationBuilder.DropTable(
                name: "identity_server_client_id_prestrictions");

            migrationBuilder.DropTable(
                name: "identity_server_client_post_logout_redirect_uris");

            migrationBuilder.DropTable(
                name: "identity_server_client_properties");

            migrationBuilder.DropTable(
                name: "identity_server_client_redirect_uris");

            migrationBuilder.DropTable(
                name: "identity_server_client_scopes");

            migrationBuilder.DropTable(
                name: "identity_server_client_secrets");

            migrationBuilder.DropTable(
                name: "identity_server_device_flow_codes");

            migrationBuilder.DropTable(
                name: "identity_server_identity_claims");

            migrationBuilder.DropTable(
                name: "identity_server_persisted_grants");

            migrationBuilder.DropTable(
                name: "abp_entity_changes");

            migrationBuilder.DropTable(
                name: "abp_tenants");

            migrationBuilder.DropTable(
                name: "abp_organization_units");

            migrationBuilder.DropTable(
                name: "abp_roles");

            migrationBuilder.DropTable(
                name: "abp_users");

            migrationBuilder.DropTable(
                name: "demob_sale_order");

            migrationBuilder.DropTable(
                name: "identity_server_api_scopes");

            migrationBuilder.DropTable(
                name: "identity_server_clients");

            migrationBuilder.DropTable(
                name: "identity_server_identity_resources");

            migrationBuilder.DropTable(
                name: "abp_audit_logs");

            migrationBuilder.DropTable(
                name: "identity_server_api_resources");
        }
    }
}
