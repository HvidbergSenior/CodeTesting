
# Tempo Integration
Piecework functionality in Akkord+ requires information about materials and operations. Two broad categories of information are required:

1) Metadata used when searching and presenting materials and operations in the user interface.
2) Operation time information used when calculating piecework sums.


## Integration Overview
Tempo provides an existing integration option where data is exported as a full snapshot using files over FTP.

![Folders](/docs/C4/Tempo%20integration.png)


## Tempo Export
Tempo data is exported at a rate of 3-4 times per year. Export is triggered on demand. Data is exported as a series of text files in a fixed column format. The files are zipped in a single file and placed on an FTP server.

File timestamp on the server designate when the export was stored, and can be used by the consumer system to check for new exports. 
The FTP server contains the newest export and not historic exports and the export is a full export.


## Akkord+ Import
Akkord+ contains a `Catalog` module containing functionality for searching materials and operations.
This module is used by other Akkord+ modules when searching for data and when registering work in a piecework project.

## Operation time

Materials and assemblies in the Tempo export contain operation time provided in millisecond resolution. An operation has a specific operation time, while the operation time for a material is decided by taking into account master and replacement materials as well as supplement operations.

- The assemblies from a master material will always precede a materials own assemblies.
  -  If the master reference is removed the materials own assemblies should be used.
- A master material will never be marked as replaced. In this case the assemblies on the master will be moved to a new master material and all pointers to the old master moved to the new.
- A material replaced by another material will always use its own assemblies, unless a master material is specified. 
  - The assemblies from the replacement materials are never used. 

## Non-functional requirements
1) Anti-Corruption layer, internal implementation of the `Catalog` module should not be dictated by the Tempo integration. Interface between Catalog and other Akkord+ modules should not be dictated by Tempo integration.

2) Import history should be maintained. In case of corrupt files it should be possible to roll-back to a previously imported catalog.

3) It should be possible to trace a material or operation in Akkord+ back to it's source in a Tempo exported file.


## File format
File format record layout is described in `Tempo Export Record Layout 1.3.pdf`. This description can be used to parse individual records, but additional information is needed to build a catalog of material.


### Materials - `Products.txt`
In general the records are related by the order in which they are encountered in the file. Record type 1 signals the beginning of a new material. All records encountered before another type 1 record belongs to this material.

Same approach applies with fx. record type 3 and 4. All records of type 4 belongs to the last record with type 3.


#### Record type 1 - `Material` 
`Varegruppenummer` is a 4 digit number used to divide materials into a standard business specific hierarchy in two levels. The first 2 digits is first level and last 2 digits is second level. The names for each entry in the hiararchy is fixed by the standard (and not provided in the record).

`MasterProductNumber` can point to another material with an assembly operation time. The level of indirection does not exceed one, so if there is a master product number in this field, the master product should contain assembly lines. 

There are cases where the master product does not contain assemblies but has been discontinued in favor of a new product. The new product is referenced in the master products field `ReplacedByProductNumber`.


#### Record type 2 - `ydelse`
These operations are private with respect to a material. In other words it makes no sense to use the operation in a project without a material.


#### Record type 4 - `Montagelinje`
The individual operations measured by the Tempo consultants. These operations sum up to the total assembly time. In most cases these can be ignored and the total assembly time used instead.

In some cases an assembly requires an unknown number of items. An example is number of conductors in a cable when assembling a fuse box. The `Quantity` field will contain 0 to indicate that the numnber must entered when registering the work.

Only a single of `OperationId`, `OperationNumber` or `ProductNumber` is filled in. These will point to record type 2, `Operations.txt` or record type 1 respectively.


#### Record type 4 - `Relateret ydelse`
Will point to an operation in `Operations.txt`. 


#### Record type 9 - `Tekst`
Related to the material and spanning all `Montage` records.