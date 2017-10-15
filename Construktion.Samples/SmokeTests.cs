using System;
using System.Linq;
using Construktion.Samples.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Shouldly;
using Xunit;

namespace Construktion.Samples
{
    public class SmokeTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public void should_fill(string name, int age)
        {
            name.ShouldNotBeNullOrWhiteSpace();
            age.ShouldNotBe(0);
        }

        //todo cleanup
        [Theory, ConstruktionData]
        public void should_save(Team team, Player player)
        {
            player.Team = team;

            var dbContextOptions =
                new DbContextOptionsBuilder<LeagueContext>().UseInMemoryDatabase("InMemoryDatabase")
                .Options;

            using (var dbContext = new LeagueContext(dbContextOptions))
            {
                dbContext.Teams.Add(team);
                dbContext.Players.Add(player);
                dbContext.SaveChanges();
            }

            using (var dbContext = new LeagueContext(dbContextOptions))
            {
                var foundPlayer = dbContext.Players.SingleOrDefault();

                foundPlayer.ShouldNotBeNull();
                foundPlayer.Name.ShouldBe(player.Name);
                foundPlayer.TeamId.ShouldBe(team.Id);
            }
        }
    }
}
