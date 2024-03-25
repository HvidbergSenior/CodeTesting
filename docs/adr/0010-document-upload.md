# 1. Record architecture decisions

Date: 2022-08-17

## Status

Proposed

## Context

A document storage solution is needed that fullfils the requirements presented in the solution description. 

The presented design is a solution based on Azure BLOB storage with Microsoft Defender for Cloud as means for detecting threads.

## Decision

Microsoft Defender for Cloud scans files uploaded to BLOB storage. A scan results in an alert if something malicious was detected, however; no alert is triggered when scan is completed without finding anything malicious.

As a consequence of this, we cannot place a file in a temporary location until scan is completed (since we dont know when scan is completed). We can however mark a BLOB as malicious after Defender for Cloud has raised an alrrt.

![document upload data flow](/docs/Images/document_upload.png)

Because Defender for Cloud is using hash reputation based scanning, we could make upload an asynchronous operation. The file could be accepted and uploaded as first step, but not made available for download until later. This allows us to change scanning from Defender for Cloud to deep file scanning using different products. In the initial solution with Defender for Cloud, we could simply let the file be available after a fixed time.

## Consequences

DevOps work needed to deploy Azure logical app.

Document upload can be developed now, including support for malicious flag. Threat detection and handling can be added later.
