output "resource_group_name" {
  description = "Name of resource group"
  value = data.azurerm_resource_group.base.name
}
