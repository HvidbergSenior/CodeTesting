terraform {
  required_providers {
      azurerm = {
        source  = "hashicorp/azurerm"
        version = ">=3.0.0"
      }
      random = {
        source = "hashicorp/random"
      }
      postgresql = {
        source  = "cyrilgdn/postgresql"
        version = ">=1.18.0"
    }
  }
}

provider "postgresql" {
  superuser = false
  host     = azurerm_postgresql_server.postgresserver.fqdn
  username = "${local.admin_username}@${local.server_name}"
  password = random_password.admin_password.result
}

locals {
  server_name = "${var.env}-deftq-postgres"
  database_name = "deftq_db"
  admin_username = "psqladmin"
  deftq_username = "deftq_user"
}

resource "random_password" "admin_password" {
  length = 16
  special = true
  override_special = "_%@"
}

resource "random_password" "deftq_user_password" {
  length = 16
  special = true
  override_special = "_%@"
}

resource "azurerm_postgresql_server" "postgresserver" {
  name                = local.server_name
  resource_group_name = var.resourcegroup.name
  location            = var.resourcegroup.location

  administrator_login          = local.admin_username
  administrator_login_password = random_password.admin_password.result

  sku_name   = "B_Gen5_1"
  version    = "11"
  storage_mb = 5120

  backup_retention_days        = 7
  geo_redundant_backup_enabled = false
  auto_grow_enabled            = true

  public_network_access_enabled    = true
  ssl_enforcement_enabled          = true
  ssl_minimal_tls_version_enforced = "TLS1_2"

  infrastructure_encryption_enabled = false

  tags = {
    Environment = "${var.env}",
  }
}

resource "azurerm_postgresql_configuration" "postgressconfig" {
  name                = "backslash_quote"
  resource_group_name = var.resourcegroup.name
  server_name         = azurerm_postgresql_server.postgresserver.name
  value               = "on"
}

resource "azurerm_postgresql_firewall_rule" "postgressfirewallazure" {
  name                = "azure"
  resource_group_name = var.resourcegroup.name
  server_name         = azurerm_postgresql_server.postgresserver.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_postgresql_firewall_rule" "postgressfirewallmia1" {
  name                = "mia-1"
  resource_group_name = var.resourcegroup.name
  server_name         = azurerm_postgresql_server.postgresserver.name
  start_ip_address    = "87.116.45.64"
  end_ip_address      = "87.116.45.78"
}

resource "azurerm_postgresql_firewall_rule" "postgressfirewallmia2" {
  name                = "mia-2"
  resource_group_name = var.resourcegroup.name
  server_name         = azurerm_postgresql_server.postgresserver.name
  start_ip_address    = "87.116.45.32"
  end_ip_address      = "87.116.45.62"
}

resource "azurerm_postgresql_database" "postgresdatabase" {
  name                = local.database_name
  resource_group_name = var.resourcegroup.name
  server_name         = azurerm_postgresql_server.postgresserver.name
  charset             = "UTF8"
  collation           = "da-DK"
  depends_on = [
    postgresql_role.user
  ]
}

resource "postgresql_role" "user" {
  name                = local.deftq_username
  login               = true
  superuser           = false
  create_database     = false
  create_role         = false
  inherit             = true
  replication         = false
  skip_reassign_owned = true
  #skip_drop_role      = true
  password            = random_password.deftq_user_password.result
  depends_on = [
    #azurerm_postgresql_database.postgresdatabase, azurerm_postgresql_firewall_rule.postgressfirewallmia1, azurerm_postgresql_firewall_rule.postgressfirewallmia2
    azurerm_postgresql_firewall_rule.postgressfirewallmia1, azurerm_postgresql_firewall_rule.postgressfirewallmia2
  ]
}

resource "postgresql_extension" "postgres_trgm_extension" {
  name = "pg_trgm"
  database = local.database_name
  drop_cascade = true
  depends_on = [
    azurerm_postgresql_database.postgresdatabase
  ]
}

resource "postgresql_grant" "deftq_user_table_privileges" {
  database    = local.database_name
  schema      = "public"
  role        = local.deftq_username
  object_type = "table"
  privileges  = ["SELECT", "INSERT", "UPDATE", "DELETE", "TRUNCATE"]
  depends_on = [
    postgresql_role.user, azurerm_postgresql_database.postgresdatabase
  ]
}

resource "postgresql_grant" "deftq_user_schema_privileges" {
  database    = local.database_name
  schema      = "public"
  role        = local.deftq_username
  object_type = "schema"
  privileges  = ["CREATE"]
  depends_on = [
    postgresql_role.user, azurerm_postgresql_database.postgresdatabase
  ]
}

resource "postgresql_grant" "deftq_user_datebase_privileges" {
  database    = local.database_name
  schema      = "public"
  role        = local.deftq_username
  object_type = "database"
  privileges  = ["CREATE"]
  depends_on = [
    postgresql_role.user, azurerm_postgresql_database.postgresdatabase
  ]
}

resource "postgresql_grant" "deftq_user_procedure_privileges" {
  database    = local.database_name
  schema      = "public"
  role        = local.deftq_username
  object_type = "procedure"
  privileges  = ["EXECUTE"]
  depends_on = [
    postgresql_role.user, azurerm_postgresql_database.postgresdatabase
  ]
}
