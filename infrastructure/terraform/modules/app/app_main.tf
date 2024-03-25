terraform {
  required_providers {
      azurerm = {
        source  = "hashicorp/azurerm"
        version = ">=3.0.0"
      }
      random = {
        source = "hashicorp/random"
      }
  }
}

resource "azurerm_app_service_plan" "base" {
  name                = "${var.env}-deftq-app-plan"
  location            = var.resourcegroup.location
  resource_group_name = var.resourcegroup.name
  
  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_app_service" "base" {
  name                = "${var.env}-deftq-app"
  location            = var.resourcegroup.location
  resource_group_name = var.resourcegroup.name
  app_service_plan_id = azurerm_app_service_plan.base.id
  identity {
    type = "SystemAssigned"
  }
  site_config {
    use_32_bit_worker_process = true
    cors {
      allowed_origins = ["*"]
    }
  }
  app_settings = {   
    APPINSIGHTS_INSTRUMENTATIONKEY = var.application_insights_instrumentation_key
    APPLICATIONINSIGHTS_CONNECTION_STRING = var.application_insights_connectionstring
  }
}
