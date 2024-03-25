terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">=3.36.0"
    }
    random = {
      source = "hashicorp/random"
    }
  }
}

locals {
  postgres_admin_connection_string = "Postgres-admin-connectionstring"
  postgres_connection_string = "Marten--ConnectionString"
  application_insights_connection_string = "ApplicationInsights--ConnectionString"
  document_storage_account_connection_string = "DocumentStorage--StorageAccountConnectionString"
}

data "azurerm_resource_group" "base" {
  name = "DEFTQ-${var.environment_name}-Resource-Group"
}

# Keyvault for storing secrets
module "keyvault" {
  source = "../keyvault"
  subscription_id = var.subscription_id
  env = var.environment_name
  providers = {
    azurerm = azurerm
  }
  resourcegroup = data.azurerm_resource_group.base
}

# Storage account for uploaded documents
module "document_storage_account" {
  source = "../document_storage_account"
  env = var.environment_name
  providers = {
    azurerm = azurerm
  }
  resourcegroup = data.azurerm_resource_group.base
}

resource "azurerm_key_vault_secret" "document_storage_account_connection_string" {
  name         = local.document_storage_account_connection_string
  value        = module.document_storage_account.document_storage_account_connection_string
  key_vault_id = module.keyvault.keyvault_id
}

# Postgres database
module "database" {
  source = "../database"
  env = var.environment_name
  providers = {
      random = random
      azurerm = azurerm
  }
  resourcegroup = data.azurerm_resource_group.base
}

resource "azurerm_key_vault_secret" "postgresconnectionstring" {
  name         = local.postgres_connection_string
  value        = module.database.connection_string
  key_vault_id = module.keyvault.keyvault_id
}

# Backend web application
module "application" {
  source = "../app"
  env = var.environment_name
  providers = {
      random = random
      azurerm = azurerm
  }
  resourcegroup = data.azurerm_resource_group.base
  application_insights_connectionstring = module.application_insights.connectionstring
  application_insights_instrumentation_key = module.application_insights.instrumentation_key
}

# Assign web app access to keyvault secrets
resource "azurerm_role_assignment" "appkeyvaultreader" {
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${data.azurerm_resource_group.base.name}"
  role_definition_name = "Key Vault Secrets User"
  principal_id         = module.application.managed_identity_principal_id
}

# Application Insights for log collection
module "application_insights" {
  source = "../application_insights"
  env = var.environment_name
  retention_in_days = 30
  providers = {
      azurerm = azurerm
  }
  resourcegroup = data.azurerm_resource_group.base
}

resource "azurerm_key_vault_secret" "application_insights_connection_string" {
  name         = local.application_insights_connection_string
  value        = module.application_insights.connectionstring
  key_vault_id = module.keyvault.keyvault_id
}