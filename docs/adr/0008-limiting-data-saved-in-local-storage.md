# 8. limiting data saved in local storage

Date: 2022-05-02

## Status

Proposed

## Context

Local storage on user devices is limited and therefore it is important to prioritize what data is available offline. DEFTQ application contains large library of materials and modules used by electricians to register their work. 

## Decision

In offline mode, the library of materials will be limited to the most commonly used. They will also not be displayed with photos.

## Consequences

- Application users will be limited in what they can register while offline
- Local storage capacity will no be exhausted on that feature only.
