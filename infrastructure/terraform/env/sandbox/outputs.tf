output "env_name" {
  description = "Name of environment"
  value = "${local.env}"
}

output "resource_group_name" {
  description = "Name of resource group"
  value = module.environment.resource_group_name
}

