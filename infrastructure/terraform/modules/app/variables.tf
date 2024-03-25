variable "env" {
  type = string
  description = "The prefix used for all resources. Type: string"
}

variable "resourcegroup"{
  description = "The appservice resource group to use. Type: object (azurerm_resource_group)"
}

variable "application_insights_instrumentation_key" {
  type = string
  description = "The Application Insights instrumentation key. Type: string"
}

variable "application_insights_connectionstring" {
  type = string
  description = "The Application Insights connection string. Type: string"
}