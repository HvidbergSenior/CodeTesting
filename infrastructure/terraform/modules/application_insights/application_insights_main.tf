terraform {
  required_providers {
      azurerm = {
        source  = "hashicorp/azurerm"
        version = ">=3.0.0"
      }
  }
}

resource "azurerm_application_insights" "application_insights" {
  name                = "${var.env}-deftq-app_insights"
  location            = var.resourcegroup.location
  resource_group_name = var.resourcegroup.name
  application_type    = "web"
  retention_in_days = var.retention_in_days
}
