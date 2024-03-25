@echo off
REM Test deploy of DEFTQ Api with SSL certificate
echo Calling https://localhost:5001/api/config...
curl https://localhost:5001/api/config
