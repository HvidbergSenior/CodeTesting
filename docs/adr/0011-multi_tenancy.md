# 1. Record architecture decisions

Date: 2022-11-09

## Status

Proposed

## Context

The system has from the beginning been set up as a multi-tenant solution with a shared database. All documents are stored with a tenant identifier, and all data access is performed using the tenant id.

The tenant id is selected dynamically based on the host information in the http requests. Because of this it's not well known at deployment time what possible tenant ids to expect. 

When performing a deployment, it is in some cases desirably to seed the database with some initial data. A concrete example is materials.

There are however no requirements for multi-tenancy. 

## Decision

Should we keep the tenanted solution? And if so how do we support data that is naturally shared between tenants? (such as imported materials and operations).

Possible decisions:

1. Keep tenanted solution and support shared data. Two options for shared data exists, either support specific data areas without tenants, or duplicate all shared data for each tenant.

2. Keep tenant solution, but use a single tenant for all data. An argument for this solution is that in the case we need to support tenants, the database schema is already prepared.

3. Remove tenant information completely.

## Consequences

Adding or removing tenant information in database schema requires schema changes.
