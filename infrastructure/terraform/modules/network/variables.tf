variable "prefix" {
  description = "The prefix used for all resources"
}

variable "port" {
  description = "The port used for exposing application"
  type = number
}

variable "location" {
  description = "The Azure location where all resources should be created"
  default = "northeurope"
}


