[![Build status](https://ci.appveyor.com/api/projects/status/elx7hqdcjl9v46mf?svg=true)](https://ci.appveyor.com/project/JoeGannon/construktion)
[![NuGet](http://img.shields.io/nuget/v/Construktion.svg)](https://www.nuget.org/packages/Construktion/)
[![Join the chat at https://gitter.im/Construktion_/Lobby](https://badges.gitter.im/Construktion_/Lobby.svg)](https://gitter.im/Construktion_/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

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
