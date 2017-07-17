Quick Start
---
The Construktion class will build objects with randomized values.

```c#
var construktion = new Construktion();

var string = construktion.Construct<string>();

var int = construktion.Construct<int>();

var person = construktion.Construct<Person>();

string.ShouldNotBeNullOrWhiteSpace();
int.ShouldNotBe(0);
person.Name.ShouldStartWith("Name-");
person.Age.ShouldNotBe(0);

```

Customizing
---
The [wiki](https://github.com/Construktion/Construktion/wiki) has more details and documentation. 

Nuget
---
This [package](https://www.nuget.org/packages/Construktion) supports .NetFramework 4.5 and .NetStandard 1.6.
```
Install-Package Construktion -Pre
```