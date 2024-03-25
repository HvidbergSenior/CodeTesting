terraform {
    backend "local" {}
}

module "application" {
  source = "../../modules/app"
  prefix = "prod"
  port = 8002
}