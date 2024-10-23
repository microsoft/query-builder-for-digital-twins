# Azure Digital Twins Query Builder

[![Build](https://github.com/microsoft/query-builder-for-digital-twins/actions/workflows/build.yml/badge.svg)](https://github.com/microsoft/query-builder-for-digital-twins/actions/workflows/build.yml) 
[![Release](https://github.com/microsoft/query-builder-for-digital-twins/actions/workflows/release.yml/badge.svg)](https://github.com/microsoft/query-builder-for-digital-twins/actions/workflows/release.yml)
[![NuGet](https://img.shields.io/nuget/v/Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder.svg)](https://www.nuget.org/packages/Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder)

The Azure Digital Twins (ADT) QueryBuilder provides a C# based fluent query builder that helps you build and query an Azure Digital Twin instance in an easy and predictable way with familiar C# based programming constructs.
The QueryBuilder factory supports two flows we're identifying as the ***Typed*** flow and the ***Dynamic*** flow.
The Typed flow starts with the method `.From<T>()` and uses generics constrained by the BasicDigitalTwin class provided by the ADT Client SDK and provided Linq-like predicates to create queries.
The Dynamic flow supports two querying methods: `.FromTwins()` and `.FromRelationships()` and both support building queries in scenarios where the C# type of the model is not known, thus the syntax requires a bit more verbosity. More often than not, you'll likely find that it to be a better experience to use the Typed query flow over the Dynamic one that has niche use cases. For a quick comparison between the Dynamic and Typed flows read [here](#typed-vs-dynamic-comparison).

Queries generated follows a grammar of custom SQL-like query language called [Azure Digital Twins query language](https://docs.microsoft.com/en-us/azure/digital-twins/concepts-query-language).

___

## Prerequisites

There are some assumptions the Typed QueryBuilder is making so that it can be most useful (please refer to [ADT SDK](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/digitaltwins/Azure.DigitalTwins.Core#azure-iot-digital-twins-client-library-for-net) for more context):

- Digital Twin Definition Language (DTDL) models are represented in C# classes (C# models).
- C# models inherit from BasicDigitalTwin class supplied by ADT SDK.
- DTDL properties are mapped to C# properties in their corresponding models.
- C# model properties have JsonPropertyName attributes mapping to their DTDL model property names, following the standards of ADT classes.
- Enum values have EnumMember attributes mapping to the DTDL model enum values.
- Models have references to their relationships and the relationship classes extend BasicRelationship class supplied by ADT SDK.
- C# relationship classes set Name property at construction.
  > Note: It's important to understand this because of the distinction in behavior between the Typed and Dynamic flows. In the Dynamic flow, you'll need to be cognizant of the json representation of the property name, whereas in the Typed flow, this is taken care of for you.

___

## Typed Samples

Here are some samples of how the Typed flow can be used to construct complex queries:

```csharp
var query = QueryBuilder
                .From<Building>()
                .Join<Building, Device>(b => b.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
                .Select<Sensor>();

var stringQuery = query.BuildAdtQuery();

/*
Generated query - Gets you all sensor twins in the specified building

SELECT sensor
FROM DIGITALTWINS building
JOIN device RELATED building.hasDevices
JOIN sensor RELATED device.hasSensors
WHERE IS_OF_MODEL(building, 'dtmi:microsoft:Space:Building;1')
AND IS_OF_MODEL(device, 'dtmi:microsoft:Device;1')
AND IS_OF_MODEL(sensor, 'dtmi:microsoft:Sensor;1')
AND building.$dtId = 'ID'
*/
```

``` csharp
var query = QueryBuilder
            .From<Building>()
            .WhereIn<Building>(b => b.Name, new string[] { "name1", "name2" });

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses 'IS_OF_MODEL' and 'IN' ADT query operator

SELECT building
FROM DIGITALTWINS building
WHERE IS_OF_MODEL(building, 'dtmi:microsoft:Space:Building;1')
AND building.name IN ['name1','name2']
*/
```

``` csharp
var query = QueryBuilder
            .From<Building>()
            .Top(5)
            .WhereStartsWith<Building>(b => b.Name, "name");

var stringQuery = query.BuildAdtQuery();


/*
Generated query - uses 'IS_OF_MODEL', 'TOP' and 'STARTSWITH' ADT query operator

SELECT Top(5) building
FROM DIGITALTWINS building
WHERE IS_OF_MODEL(building, 'dtmi:microsoft:Space:Building;1')
AND STARTSWITH(building.name, 'name')
*/
```

``` csharp
var query = QueryBuilder
            .From<Building>()
            .Count()
            .WhereStartsWith<Building>(b => b.Name, "name");

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses 'IS_OF_MODEL', 'TOP' and 'STARTSWITH' ADT query operator

SELECT COUNT()
FROM DIGITALTWINS building
WHERE IS_OF_MODEL(building, 'dtmi:microsoft:Space:Building;1')
AND STARTSWITH(building.name, 'name')
*/
```

``` csharp
var query = QueryBuilder
            .From<Building>()
            .Count()
            .WhereContains<Building>(b => b.Name, "ame");

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses 'IS_OF_MODEL', 'TOP' and 'CONTAINS' ADT query operator

SELECT COUNT()
FROM DIGITALTWINS building
WHERE IS_OF_MODEL(building, 'dtmi:microsoft:Space:Building;1')
AND CONTAINS(building.name, 'ame')
*/
```

```csharp
var query = QueryBuilder
            .From<Building>("bldng")
            .Join<Building, ITSiteFunction>(b => b.HasITSiteFunction, "bldng", "itfunc", "rel")
            .Where<Building>(b => b.Id, ComparisonOperators.IsEqualTo, "ID")
            .Or(query => query
                .Where<Building>("count", ComparisonOperators.IsGreaterThan, 20, alias: "bldng")
                .And(q => q
                    .Where<Building>("count", ComparisonOperators.IsLessThan, 10, alias: "bldng")
                    .WhereEndsWith<BuildingHasITSiteFunctionRelationship>("maxPriority", "word")));

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses AND & OR

SELECT bldng
FROM DIGITALTWINS bldng
JOIN itfunc RELATED bldng.hasITSiteFunction rel
WHERE IS_OF_MODEL(bldng, 'dtmi:microsoft:Space:Building;1')
AND IS_OF_MODEL(itfunc, 'dtmi:microsoft:ITSiteFunction;1')
AND bldng.$dtId = 'ID' AND (bldng.count > 20
OR (bldng.count < 10 AND ENDSWITH(rel.maxPriority, 'word')))
```

```csharp
var query = QueryBuilder
                    .From<Space>()
                    .WhereStartsWith<Space>(s => s.Name, "word")
                    .Or(query => query
                        .WhereIsOfModel<Space, Building>()
                        .WhereIsOfModel<Space, Floor>())
                    .Not(query => query
                        .WhereIsOfModel<Space, ConferenceRoom>());
/* 
Generated query - uses IS_OF_MODEL & OR
SELECT space FROM DIGITALTWINS space 
WHERE IS_OF_MODEL(space, 'dtmi:microsoft:Space;1') 
AND STARTSWITH(space.name, 'word') 
AND (IS_OF_MODEL(space, 'dtmi:microsoft:Space:Building;1') OR IS_OF_MODEL(space, 'dtmi:microsoft:Space:Floor;1'))
AND NOT IS_OF_MODEL(space, 'dtmi:microsoft:Space:ConferenceRoom;1')
*/
```

```csharp
var query = QueryBuilder
                .From<BasicDigitalTwin>();
/* 
Generated query - select all twins
SELECT basicdigitaltwin 
FROM DIGITALTWINS basicdigitaltwin
*/
```
___

### Methods

Methods supported in the Typed flow.

- QueryBuilder
  - From\<TModel\>()
    - Where\<TModel\>(propertySelector, operation, value)
    - Where\<TModel\>(propertyName, operation, value)
    - Where\<TModel\>(propertySelector, scalarOperator)
    - Where\<TModel\>(propertyName, scalarOperator)
    - WhereStartsWith\<TModel\>(propertySelector, value)
    - WhereStartsWith\<TModel\>(propertyName, value)
    - WhereEndsWith\<TModel\>(propertySelector, value)
    - WhereEndsWith\<TModel\>(propertyName, value)
    - WhereContains\<TModel\>(propertySelector, value)
    - WhereContains\<TModel\>(propertyName, value)
    - WhereIn\<TModel\>(propertySelector, values)
    - WhereIn\<TModel\>(propertyName, values)
    - WhereNotIn\<TModel\>(propertySelector, values)
    - WhereNotIn\<TModel\>(propertyName, values)
    - WhereIsOfModel\<TBase,TDerived\>()
    - Join\<TJoinFrom,TJoinWith\>(relationship)
    - Select\<TSelect\>()
    - Top(numberOfRecords)
    - Count()
    - And(conditions)
    - Or(conditions)
    - Not(conditions)
    - BuildAdtQuery()
  - CountAllDigitalTwins()
    - BuildAdtQuery()

___

### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| TModel, TJoinFrom, TJoinWith, TSelect, TBase, TDerived | A [Type](https://docs.microsoft.com/en-us/dotnet/api/system.type) that's a sub-type of [Azure.DigitalTwins.Core.BasicDigitalTwin](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/digitaltwins/Azure.DigitalTwins.Core/src/Models/BasicDigitalTwin.cs). | The C# model of the twin. |
| propertySelector | [Expression<Func<TModel, object>>](https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1) | Expression to select any property of TModel type.|
| propertyName | string | JSON property name of TModel type.  |
| operation | ComparisonOperators | Operator for the condition. |
| scalarOperator | ScalarOperators | ADT scalar function.  |
| value | object | Value against which the where condition is applied. |
| values | string[] | values against which the where condition is applied. |
| conditions | [Action<AdtFilteredQuery\<TQuery\>>](https://docs.microsoft.com/en-us/dotnet/api/system.action-1) | An action used to apply a chain of filters to the query. |

___

### Operators

ComparisonOperators

- IsEqualTo
- IsGreaterThan
- IsGreaterThanOrEqualTo
- IsLessThan
- IsLessThanOrEqualTo
- NotEqualTo

ScalarOperators

- IS_BOOL
- IS_DEFINED
- IS_NULL
- IS_NUMBER
- IS_OBJECT
- IS_PRIMITIVE
- IS_STRING

___

## Typed Samples Using LINQ Expressions

Here are some samples of how the Typed flow can be used to construct complex queries:

```csharp
var query = QueryBuilder
                .From<Building>()
                .Join<Building, Device>(b => b.HasDevices)
                .Join<Device, Sensor>(d => d.HasSensors)
                .Where<Building>(b => b.Id == "ID")
                .Select<Sensor>();

var stringQuery = query.BuildAdtQuery();

/*
Generated query - Gets you all sensor twins in the specified building

SELECT sensor
FROM DIGITALTWINS building
JOIN device RELATED building.hasDevices
JOIN sensor RELATED device.hasSensors
WHERE IS_OF_MODEL(building, 'dtmi:microsoft:Space:Building;1')
AND IS_OF_MODEL(device, 'dtmi:microsoft:Device;1')
AND IS_OF_MODEL(sensor, 'dtmi:microsoft:Sensor;1')
AND building.$dtId = 'ID'
*/
```

``` csharp
var query = QueryBuilder
            .From<Building>()
            .Top(5)
            .Where<Building>(b => b.Name.StartsWith("name"));

var stringQuery = query.BuildAdtQuery();


/*
Generated query - uses 'IS_OF_MODEL', 'TOP' and 'STARTSWITH' ADT query operator

SELECT Top(5) building
FROM DIGITALTWINS building
WHERE IS_OF_MODEL(building, 'dtmi:microsoft:Space:Building;1')
AND STARTSWITH(building.name, 'name')
*/
```

``` csharp
var query = QueryBuilder
            .From<Building>()
            .Count()
            .Where<Building>(b => b.Name.Contains("ame"));

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses 'IS_OF_MODEL', 'TOP' and 'CONTAINS' ADT query operator

SELECT COUNT()
FROM DIGITALTWINS building
WHERE IS_OF_MODEL(building, 'dtmi:microsoft:Space:Building;1')
AND CONTAINS(building.name, 'ame')
*/
```

```csharp
var query = QueryBuilder
            .From<Building>()
            .Join<Building, Device>(building => building.HasDevices)
            .Join<Device, Sensor>(device => device.HasSensors)
            .Where<Building>(building => (building.Id != null && building.Id == "name"))
            .Where<Device>(device => (device.Name != null && device.Name == "name"))
            .Select<Sensor>();

var stringQuery = query.BuildAdtQuery();

/*
Generated query - Gets you all sensor twins in the specified building and device

SELECT sensor
FROM DIGITALTWINS building
JOIN device RELATED building.hasDevices spacehasdevicesrelationship
JOIN sensor RELATED device.hasSensors devicehassensorsrelationship
WHERE IS_OF_MODEL(building, 'dtmi:microsoft:Space:Building;1')
AND IS_OF_MODEL(device, 'dtmi:microsoft:Device;1')
AND IS_OF_MODEL(sensor, 'dtmi:microsoft:Sensor;1')
AND (NOT IS_NULL(building.$dtId) AND building.$dtId = 'name'))
AND (NOT IS_NULL(device.name) AND device.name = 'name'))
*/
```
___

### Methods

Methods supported in the Typed flow.

- QueryBuilder
  - From\<TModel\>()
    - Where\<TModel\>(expression)
    - Join\<TJoinFrom,TJoinWith\>(relationship)
    - Select\<TSelect\>()
    - Top(numberOfRecords)
    - Count()
    - BuildAdtQuery()
  - CountAllDigitalTwins()
    - BuildAdtQuery()

___

### Operators

ComparisonOperators

- =, !=: Used to compare equality of expressions.
- <, >: Used for ordered comparison of expressions.
- <=, >=: Used for ordered comparison of expressions, including equality.

ScalarOperators

- IS_BOOL
- IS_NULL
- IS_NUMBER
- IS_OBJECT
- IS_STRING
- NOT

LogicalOperators
- AndAlso
- OrElse

___

## Dynamic Samples

Here are some samples of how the Dynamic flow can be used to construct complex queries:

> Note: In comparing these nearly identical samples to the Typed flow, you'll want to take notice that while the Typed flow automatically applies the IS_OF_MODEL scalar function to filter for models based on the types used in the methods' generic parameters, the Dynamic flow does not, as it is not aware of any types. To filter for specific models, the Dynamic flow supports `.Where(t => t.IsOfModel("dtmi:sometwinmodel;1"))`

```csharp
var query = QueryBuilder
                .FromTwins()
                .Join(b => b
                    .With("device")
                    .RelatedBy("hasDevices")
                    .Join(d => d
                        .With("sensor")
                        .RelatedBy("hasSensors")))
                .Where(b => b.TwinProperty("$dtId").IsEqualTo("ID"))
                .Select("sensor");

var stringQuery = query.BuildAdtQuery();

/*
Generated query - Gets you all sensor twins based on the root twin with the $dtId of 'ID'

SELECT sensor
FROM DIGITALTWINS twin
JOIN device RELATED twin.hasDevices
JOIN sensor RELATED device.hasSensors
WHERE twin.$dtId = 'ID'
*/
```

``` csharp
var query = QueryBuilder
            .FromTwins()
            .Where(b => b
                .TwinProperty("name")
                .IsIn(new string[] { "name1", "name2" }));

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses 'IN' ADT query operator

SELECT twin
FROM DIGITALTWINS twin
WHERE twin.name IN ['name1','name2']
*/
```

``` csharp
var query = QueryBuilder
            .FromTwins()
            .Top(5)
            .Where(b => b
                .TwinProperty("name")
                .StartsWith("name"));

var stringQuery = query.BuildAdtQuery();


/*
Generated query - uses 'TOP' and 'STARTSWITH' ADT query operator

SELECT Top(5) twin
FROM DIGITALTWINS twin
WHERE STARTSWITH(twin.name, 'name')
*/
```

``` csharp
var query = QueryBuilder
            .FromTwins()
            .Count()
            .Where(b => b
                .TwinProperty("name")
                .StartsWith("name"));

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses 'COUNT' and 'STARTSWITH' ADT query operator

SELECT COUNT()
FROM DIGITALTWINS twin
WHERE STARTSWITH(twin.name, 'name')
*/
```

``` csharp
var query = QueryBuilder
                .FromTwins()
                .Top(5)
                .Where(b => b
                    .TwinProperty("name")
                    .Contains("ame"));

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses 'TOP' and 'CONTAINS' ADT query operator

SELECT TOP(5) twin
FROM DIGITALTWINS twin
WHERE CONTAINS(twin.name, 'ame')
*/
```

```csharp
var query = QueryBuilder
            .FromTwins("bldng")
            .Join(b => b
                .With("itfunc")
                .RelatedBy("hasITSiteFunction")
                .As("rel"))
            .Where(b => b
                .TwinProperty("$dtId")
                .IsEqualTo("ID")
                .And()
                .Precedence(p => p
                    .TwinProperty("count")
                    .IsGreaterThan(20)
                    .Or()
                    .Precedence(p => p
                        .TwinProperty("count")
                        .IsLessThan(10)
                        .And()
                        .RelationshipProperty("maxPriority", "rel")
                        .EndsWith("word"))));

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses AND & OR

SELECT bldng
FROM DIGITALTWINS bldng
JOIN itfunc RELATED bldng.hasITSiteFunction rel
WHERE bldng.$dtId = 'ID' AND (bldng.count > 20
OR (bldng.count < 10 AND ENDSWITH(rel.maxPriority, 'word')))
```

``` csharp
var query = QueryBuilder
            .FromTwins()
            .Where("startswith(twin.name, 'name1')");

var stringQuery = query.BuildAdtQuery();

/*
Generated query - uses 'STARTSWITH' ADT query operator

SELECT twin
FROM DIGITALTWINS twin
WHERE STARTSWITH(twin.name, 'name1')
*/
```
___

### Methods

Methods supported in the Dynamic flow.

- QueryBuilder
  - FromTwins()
    - Where(filter)
    - Where(whereLogic)
    - Join(joinLogic)
    - Join(joinAndWhereLogic)
    - Select(aliases)
    - Top(numberOfRecords)
    - Count()
    - BuildAdtQuery()
  - FromRelationships()
    - Where(filter)
    - Where(whereLogic)
    - Select(aliases)
    - Top(numberOfRecords)
    - Count()
    - BuildAdtQuery()
  - CountAllDigitalTwins()
    - BuildAdtQuery()

### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| TWhereStatement, CompoundWhereStatement, JoinWithStatement, JoinFinalStatement | | These types are all part of the Where and Join methods' inner fluent syntax that aid in building and enforcing particular query semantics. |
| whereLogic | [Func<TWhereStatement, CompoundWhereStatement<TWhereStatement>>](https://docs.microsoft.com/en-us/dotnet/api/system.func-2) | Func to include WHERE logic in the query. |
| filter | string | A string filter query that can contain one or multiple conditions |
| joinLogic | [Func\<JoinWithStatement\<TWhereStatement\>, JoinFinalStatement\<TWhereStatement\>\>](https://docs.microsoft.com/en-us/dotnet/api/system.func-2) | Func to include JOIN logic in the query. |
| joinAndWhereLogic | [Func\<JoinWithStatement\<TWhereStatement\>, CompoundWhereStatement\<TWhereStatement\>\>]() | Func to include JOIN logic in the query with additional WHERE logic scoped to the JOIN. |
| aliases | params object[] | This can be one or many aliases to override and apply to the SELECT clause of the query.
| numberOfRecords | int | The number of records to include in the a TOP query. |

___

### Operators

The operators used in the Dynamic flow are expressed as methods, but support remains the same for each.

___

## Typed vs Dynamic Comparison

| Feature | Typed | Dynamic |
| ------- | ----- | ------- |
| Query Twins Collection | :white_check_mark: | :white_check_mark: |
| Query Relationships Collection | :x: | :white_check_mark: |
| Select Twin Properties | :x: | :white_check_mark: |
| Select Relationship Properties | :x: | :white_check_mark: |
| Use Property Selectors | :white_check_mark: | :x: |
| Use Relationship Selectors | :white_check_mark: | :x: |

> Note: While not a comparison between the Typed or Dynamic flows, it's worth noting that Joins cannot be used when querying the Relationships collection.
___

## Change history

See [CHANGELOG](CHANGELOG.md) for change history of each version.

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit <https://cla.opensource.microsoft.com>.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Security

For guidance on reporing security issues, please refer to the [security](SECURITY.md) section.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft
trademarks or logos is subject to and must follow
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.


# Repository Migration Notice

This repository has been migrated to Azure DevOps (ADO). No new release versions of the NuGet package `Microsoft.DigitalWorkplace.DigitalTwins.QueryBuilder` will be released from this repository.

- **Last Version**: 1.4.0
- **Maintenance**: This repository is no longer maintained.


