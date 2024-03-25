# 1. Record architecture decisions

Date: 2022-05-24

## Status

Proposed

## Context

A backup strategy is needed that will meet the requirements of the contract. The concrete solution should also be manageable by Mjølner.

The exact solution does not need to be decided now, but we need to have a solution in place that will meet the requirements.

## Decision

Backup persistent data in Azure is needed. The resources in question are `Postgres` database(s) as well as files in `blob storage`.

`Azure Database for PostgreSQL` includes automatic backup with a retention period of up to 35 days. The target for restore of automatic backup of data is limited to Azure.

We propose to use `Azure Backup Center` as central backup mechanism. It is possible to perform backup of a Postgres database with long term retention [Postgres backup with long term retention](https://docs.microsoft.com/en-us/azure/backup/backup-azure-database-postgresql). Offline backup is also possible [Azure Backup offline](https://docs.microsoft.com/en-us/azure/backup/backup-azure-backup-import-export).

## Consequences

Backup solution as-code.

Option of having offline backup.
