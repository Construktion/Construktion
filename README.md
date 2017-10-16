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

//XUnit isn't supported out of the box, but the wiki details
//how to add support using a custom "ConstruktionData" class
[Theory, ConstruktionData]
public void should_join_team(Team team, Player player)
{
    DbSave(player, team);
    
    player.JoinTeam(team);

    var result = Query(db => db.Players.FirstOrDefault(x => x.Id == player.Id));
    result.ShouldNotBeNull();
    result.Name.ShouldBe(player.Name);
    result.TeamId.ShouldBe(team.Id);
}
```

Customizing
---
The [wiki](https://github.com/Construktion/Construktion/wiki) has more details and documentation. 

Questions, Comments, Concerns
---
For any questions or help you can hop on over to the [gitter room](https://gitter.im/Construktion_/Lobby) or file an [issue](https://github.com/Construktion/Construktion/issues). All feedback is welcomed!

