@echo off
REM -------------------------------------------------------------
REM Script for starting the DEFTQ API for frontend development.
REM -------------------------------------------------------------

docker compose -f docker-compose.yml --profile api up