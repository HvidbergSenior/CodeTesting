@ECHO OFF

REM ===========================================================================
REM This script deletes old builds and builds a new version of the DEFTQ API.
REM The new API is then deployed into a docker container and run.
REM
REM As part of the build, a SSL certificate is produced, and trusted on the 
REM local machine.
REM
REM The script must be executed from the folder, where the docker file 
REM resides:
REM
REM      <project-folder>/src/Tools/Docker
REM
REM ===========================================================================


if not exist Dockerfile (
  echo Must be executed from same folder as Dockerfile
  exit 1
)

echo -- Remove docker image...
docker image rm deftq_image
if exist deftq_api_dev_cert.pfx (
  echo -- Remove old ssl api development certificate...
  del deftq_api_dev_cert.pfx
)

echo -- Build docker image deftq_image...
docker build -f ./Dockerfile -t deftq_image ../../

echo -- Extract ssl api development certificate...
echo ---- Create container from image...
docker create --name deftq_container deftq_image
echo ---- Copy certificate from container...
docker cp deftq_container:/https/deftq_api_dev_cert.pfx .
echo ---- Remover container...
docker rm deftq_container
echo ---- Importing certificate...
echo ------ OBS: Approve import by clicking YES in the dialog !!!
certutil.exe -user -p ssl-password -importPFX .\deftq_api_dev_cert.pfx
echo ---- Certificate imported

echo -- Starting API...
CALL start_api.bat