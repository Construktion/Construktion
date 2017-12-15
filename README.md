[![Build status](https://ci.appveyor.com/api/projects/status/elx7hqdcjl9v46mf?svg=true)](https://ci.appveyor.com/project/JoeGannon/construktion)
[![NuGet](http://img.shields.io/nuget/v/Construktion.svg)](https://www.nuget.org/packages/Construktion/)
[![Join the chat at https://gitter.im/Construktion_/Lobby](https://badges.gitter.im/Construktion_/Lobby.svg)](https://gitter.im/Construktion_/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Quick Start
---
The Construktion class will build objects with randomized values. If you're using xUnit, you can inject constructed 
values automatically. 

```c#
var construktion = new Construktion();

var string = construktion.Construct<string>();
var person = construktion.Construct<Person>();

string.ShouldNotBeNullOrWhiteSpace();
person.Name.ShouldStartWith("Name-");
person.Age.ShouldNotBe(0);

//overloads like ConstructMany<T> or ConstructMany<T>(Action<T> hardCodes) are available.

//xUnit isn't supported out of the box, but the wiki details
//how to add support
[Theory, ConstruktionData]
public void should_join_team(Team team, Player player)
{
    DbSave(player, team);
    
    player.JoinTeam(team);

    var foundPlayer = Query(db => db.Players.FirstOrDefault(x => x.Id == player.Id));
    foundPlayer.ShouldNotBeNull();
    foundPlayer.Name.ShouldBe(player.Name);
    foundPlayer.TeamId.ShouldBe(team.Id);
}
```

Customizing
---
At the heart of the library are the blueprints. They are used to customize how 
objects are built. Below is an example of a blueprint that will assign all bool
properties named IsActive to true.

```c#
public class IsActiveBlueprint : Blueprint
{
    public bool Matches(ConstruktionContext context) => context.RequestType == typeof(bool) &&
                                                        context.PropertyInfo?.Name == "IsActive";

    public object Construct(ConstruktionContext context, ConstruktionPipeline pipeline) => true;
}

var construktion = new Construktion().With(new IsActiveBlueprint());
```

Customizations can be added through the construktion instance. For most scenarios the registry class should be used to group all configured settings.

```c#
var construktion = new Construktion().With(new CustomRegistry());

public class CustomRegistry : ConstruktionRegistry
{
    public CustomRegistry()
    {
        AddBlueprint<CustomBlueprint>();
        OmitIds();
        OmitProperties(typeof(List<>));
        Register<IService, Service>();
        //other blueprints and customization options
    }   
}
```
The [wiki](https://github.com/Construktion/Construktion/wiki) contains full details and documentation. 

Questions and Comments
---
For any questions or help you can hop on over to the [gitter room](https://gitter.im/Construktion_/Lobby) or file an [issue](https://github.com/Construktion/Construktion/issues). If you're currently using Construktion, I'd love to hear any feedback. Right now it's pretty much just me thinking of what people *might* want—instead of what they actually want. 

