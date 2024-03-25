output "managed_identity_principal_id" {
  value = azurerm_app_service.base.identity[0].principal_id 
}
