@echo off
REM -------------------------------------------------------------
REM Script for stopping the DEFTQ API for frontend development.
REM -------------------------------------------------------------

docker compose -f docker-compose.yml --profile api down