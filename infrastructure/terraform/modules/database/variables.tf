variable "env" {
  type = string
  description = "The prefix used for all resources. Type: string"
}

variable "resourcegroup"{
  description = "The appservice resource group to use. Type: object (azurerm_resource_group)"
}