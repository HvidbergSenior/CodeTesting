# 1. Record architecture decisions

Date: 21-04-2023

## Status

Proposed

## Context

It is a requirement that Akkord+ supports data exports in file formats, that allow further processing in spreadsheet software (Microsoft Excel).

## Decision

We need to decide which format to export. The format must be supported by Microsoft Excel, and it may be usable in other contexts (although not requried).

The Open XML format by microsoft is supported by Excel and is based on standard technologies such as XML and ZIP.

[https://learn.microsoft.com/en-us/office/open-xml/open-xml-sdk](https://learn.microsoft.com/en-us/office/open-xml/open-xml-sdk)

[https://github.com/dotnet/Open-XML-SDK](https://github.com/dotnet/Open-XML-SDK)

### Pros

- Direct support for Microsoft Excel. The end-user experience will be seamless when using Excel as the file downloaded from Akkord+ will be an .xlsx file that opens directly in Excel.

- Mature SDK with support for C# and .Net Core.

- Free-to-use

### Cons

- Files in .xlsx format may required additional processing before imported into other systems. It may require and additional manuel step in Excel where the data is exported in a different format.

- The Open XML format is open, other vendors may implement processing of files exported from Akkord+.
 
## Consequences

The exported format is not directly readably by humans.
