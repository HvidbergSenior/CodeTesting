terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.36.0"
    }
    random = {
      source = "hashicorp/random"
    }
  }
  backend "azurerm" {
    subscription_id      = "d6b72ae8-7168-43e5-be36-c55ec09eba5b"
    resource_group_name  = "DEFTQ-dev-Resource-Group"
    storage_account_name = "devdeftqinfrastructure"
    container_name       = "tfstate"
    key                  = "dev.terraform.tfstate"
  }
}

locals {
  subscription_id = "d6b72ae8-7168-43e5-be36-c55ec09eba5b"
  tenant_id = "2a41d3b1-09a3-406a-b705-d1c5a70cc21c"
  env = "dev"
}

provider "azurerm" {
  subscription_id = local.subscription_id
  tenant_id = local.tenant_id
  features {
    key_vault {
      purge_soft_deleted_secrets_on_destroy = true
      recover_soft_deleted_secrets          = true
    }
  }
}

module "environment" {
  source = "../../modules/environment"
  subscription_id = local.subscription_id
  tenant_id = local.tenant_id
  environment_name = local.env
  providers = {
      azurerm = azurerm
      random = random
  }
}
