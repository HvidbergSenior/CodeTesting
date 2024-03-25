output "instrumentation_key" {
  value = azurerm_application_insights.application_insights.instrumentation_key
}

output "connectionstring" {
  value = azurerm_application_insights.application_insights.connection_string
}

output "app_id" {
  value = azurerm_application_insights.application_insights.app_id
}
