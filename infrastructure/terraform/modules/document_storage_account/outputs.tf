output "document_storage_account_connection_string" {
  description = "Connection string for document storage account"
  value = azurerm_storage_account.document_storage_account.primary_connection_string
}