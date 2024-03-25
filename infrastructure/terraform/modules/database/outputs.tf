output "psqladmin" {
  value = random_password.admin_password.result
}

output "server_fqdn" {
  value = azurerm_postgresql_server.postgresserver.fqdn
}

output "admin_connection_string" {
  value = "Host=${azurerm_postgresql_server.postgresserver.fqdn};Port=5432;Database=${local.database_name};Username=${local.admin_username}@${local.server_name};Password=${random_password.admin_password.result}"
}

output "connection_string" {
  value = "Host=${azurerm_postgresql_server.postgresserver.fqdn};Port=5432;Database=${local.database_name};Username=${local.deftq_username}@${local.server_name};Password=${random_password.deftq_user_password.result}"
}
