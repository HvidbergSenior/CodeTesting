# Readme

To get help on how to use ADR tool type the following command into your command prompt:

```bash
adr help
```

## Add new Architecture Decision Record

```bash

adr new "Title of new ADR"

```

Ex.

```bash
adr new Use Azure AD B2B as identity provider
```

## Supersede existing Architecture Decision Record

Changes happens during the life time of a project and new decisions are made. We can supersede previous decisions using the command below.

This will create a new ADR file that is flagged as superseding ADR 9, and changes the status of ADR 9 to indicate that it is superseded by the new ADR. It then opens the new ADR in your editor of choice.

```bash
adr new -s "existing adr number" "Title of new ADR"
```

Ex.

```bash
adr new -s 3 Use Azure AD B2C as identity provider
```
