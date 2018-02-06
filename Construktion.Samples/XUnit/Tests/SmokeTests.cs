namespace Construktion.Samples.XUnit.Tests
{
    using System.Linq;
    using Entities;
    using Shouldly;
    using Xunit;
    using static TestDSL;

    public class SmokeTests
    {
        [Theory, ConstruktionData]
        public void should_fill(string name, int age)
        {
            name.ShouldNotBeNullOrWhiteSpace();
            age.ShouldNotBe(0);
        }

        [Theory, ConstruktionData]
        public void should_insert(Team team)
        {
            Insert(team);

            var foundTeam = Query(db => db.Teams.FirstOrDefault(x => x.Id == team.Id));

            foundTeam.ShouldNotBeNull();
            foundTeam.Id.ShouldNotBe(0);
            foundTeam.Name.ShouldBe(team.Name);
        }

        [Theory, ConstruktionData]
        public void should_update(Team team)
        {
            Insert(team);
            team.Name = "Updated";
            Update(team);

            var foundTeam = Query(db => db.Teams.FirstOrDefault(x => x.Id == team.Id));

            foundTeam.ShouldNotBeNull();
            foundTeam.Id.ShouldNotBe(0);
            foundTeam.Name.ShouldBe("Updated");
        }
    }
}