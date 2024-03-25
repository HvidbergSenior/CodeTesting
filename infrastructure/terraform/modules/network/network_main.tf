resource "azurerm_resource_group" "base" {
  name     = "DEFTQ-${var.prefix}-Resource-Group"
  location = var.location
}