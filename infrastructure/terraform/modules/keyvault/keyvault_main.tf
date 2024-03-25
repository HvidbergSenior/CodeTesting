terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">=3.0.0"
    }
  }
}

data "azurerm_client_config" "current" {

}

resource "azurerm_key_vault" "keyvault" {
  name = "${var.env}deftqkeyvault"
  location = var.resourcegroup.location
  resource_group_name = var.resourcegroup.name
  tenant_id = data.azurerm_client_config.current.tenant_id
  sku_name = "standard"
  soft_delete_retention_days = 7
  enable_rbac_authorization = true
  #enabled_for_template_deployment = true

  depends_on = [
    azurerm_role_assignment.keyvault_owner
  ]

  #access_policy {
  #  tenant_id = data.azurerm_client_config.current.tenant_id
  #  object_id = data.azurerm_client_config.current.object_id

  #  key_permissions = [
  #    "Create",
  #    "Get",
  #    "List"
  #  ]

  #  secret_permissions = [
  #    "Set",
  #    "Get",
  #    "Delete",
  #    "Purge",
  #    "Recover",
  #    "List"
  #  ]
  #} 
}

resource "azurerm_role_assignment" "keyvault_owner" {
  scope                = "/subscriptions/${var.subscription_id}/resourceGroups/${var.resourcegroup.name}"
  role_definition_name = "Key Vault Secrets Officer"
  principal_id         = data.azurerm_client_config.current.object_id
}
