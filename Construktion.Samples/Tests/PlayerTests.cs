namespace Construktion.Samples.Tests
{
    using System.Linq;
    using Entities;
    using Shouldly;
    using Xunit;
    using static TestDSL;

    public class PlayerTests
    {
        [Theory, ConstruktionData]
        public void should_sign_with_team(Team team, Player player)
        {
            player.SignWith(team);

            Insert(team, player);

            var foundPlayer = Query(db => db.Players.FirstOrDefault<Player>(x => x.Id == player.Id));

            foundPlayer.ShouldNotBeNull();
            foundPlayer.Name.ShouldBe(player.Name);
            foundPlayer.TeamId.ShouldBe(team.Id);
        }

        [Theory, ConstruktionData]
        public void should_leave_team(Player player)
        {
            AssignToATeam(player);

            player.LeaveTeam();
            Update(player);

            var foundPlayer = Query(db => db.Players.FirstOrDefault<Player>(x => x.Id == player.Id));

            foundPlayer.TeamId.ShouldBeNull();
            foundPlayer.Team.ShouldBeNull();
        }

        private void AssignToATeam(Player player)
        {
            var team = Construct<Team>();
            player.Team = team;
            Insert(player, team);
        }
    }
}