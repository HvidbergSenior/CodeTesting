terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">=3.0.0"
    }
  }
}

resource "azurerm_storage_account" "document_storage_account" {
  name = "${var.env}deftqdocstorage"
  location            = var.resourcegroup.location
  resource_group_name = var.resourcegroup.name
  account_tier = "Standard"
  account_replication_type = "LRS"
}
