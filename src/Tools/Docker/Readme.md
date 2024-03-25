# How to build the API and deploy it in local Docker container
The scripts here build the DEFTQ docker image and runs the DEFTQ API. Part of
the build is to generate a server developments certificate to use for SSL calls. 
The certificate needs to be installed int the local key store to trust the 
server running the API.

## Building the API
The batch script `build_and_start_api.bat` builds and starts the API. The build
proces produces a `deftq_image` docker image and copies the certificate to the
folder where the script is run from. The certificate is automatically added to
the Certificate Service and trusted.

## Files in the Docker folder
An overview of the files in the Docker folder:

| File                          | Purpose                                                                                                                                     |
|:------------------------------|:--------------------------------------------------------------------------------------------------------------------------------------------|
| Dockerfile                    | Creates a docker image of the DEFTQ API                                                                                                     |
| docker-compose.yml            | Starts the required containers, ie. postgres db and pgadmin. If the feature flag `api` is added, the DEFTQ API container is also started.   |
| build_and_start_api.bat       | Deletes old deftq_image and rebuilds the api.                                                                                               |
| start_api.bat                 | Starts the last built api.                                                                                                                  |
| stop_api.bat                  | Stops the running api                                                                                                                       |
| test_ssl.bat                  | Calls the api endpoint `api/config` using SSL                                                                                               |
| untrust_dev_certificate.bat   | Untrust and delete all installed development certificates. A prerequisite is the .NET SDK.                                                  |

