# EasyJsonT

EasyJsonT is an open source JSON library that help us to transform a JSON string to your expected.

## CI Build Status

| Platform | Build Server | Status  |
|--------- |------------- |---------|
| AppVeyor |  Windows/Ubuntu1804 |[![Build status](https://ci.appveyor.com/api/projects/status/515995895xeunq4o/branch/master?svg=true)](https://ci.appveyor.com/project/catcherwong/easyjsont/branch/master) |

## Nuget Packages

| Package Name |  Version | Downloads
|--------------|  ------- | ----
| EasyJsonT | ![](https://img.shields.io/nuget/v/EasyJsonT.svg) | ![](https://img.shields.io/nuget/dt/EasyJsonT.svg)

## What EasyJsonT Do ?

### Filter Nodes

Filter the nodes that your want to remove or reserve.

### Add Nodes

Add some additional nodes to a exist JSON string or rebuild itself as a new node.

### Rename Nodes

Rename the nodes based on your needy.

### Translate Values For Nodes

Change the value of nodes based on what you want.

## Basic Usage

### Filter Nodes

```cs
var filterNodes = new List<string> { "UserName", "age" };

var json = @"{'userName':'catcherwong','age':18,'hobbies':['write','running']}";
var filterJsonResult11 = JsonTProvider.FilterNodes(json, filterNodes, true);
//{"hobbies": ["write, "running"]}
var filterJsonResult12 = JsonTProvider.FilterNodes(json, filterNodes, false);
//{"userName": "catcherwong", "age": 18}
```

### Add Nodes

```cs
var addNodesDict = new Dictionary<string, object>
{
    {"age",18},
    {"subObj",new{prop1="123"}},
    {"subArray",new List<string> {"a","b"}}
};

var json = @"{'userName':'catcherwong'}";
var res = JsonTProvider.AddNodes(json, addNodesDict);
//{"userName":"catcherwong","age":18,"subObj":{"prop1":"123"},"subArray":["a","b"]}
```

### Rename Nodes

```cs
var renameDict = new Dictionary<string, string>
{
    {"name","userName"},{"nl","age"}
};

var json = @"{'name':'catcherwong','nl':18}";
var res = JsonTProvider.RenameNodes(json, renameDict);
//{"userName":"catcherwong","age":18}          
```

### Translate Values For Nodes

```cs
var translateValueDict = new Dictionary<string, Dictionary<object, object>>
{
    {"Code",new Dictionary<object, object>{{-1,0},{-2,1}}},
    {"messAge",new Dictionary<object, object>{{"yes","Success"},{"no","Error"}}}
};

var json = @"{'code':-1,'message':'yes'}";
var res = JsonTProvider.TranslateValues(json, translateValueDict);
//{"code":0,"message":"Success"}
```

For more usages, please visit the [sample project](https://github.com/catcherwong/EasyJsonT/blob/master/sample/EasyJsonT.Demo/Program.cs)