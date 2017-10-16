using System.Linq;
using Construktion.Samples.Entities;
using Shouldly;
using Xunit;
using static Construktion.Samples.TestDSL;

namespace Construktion.Samples
{
    public class PlayerTests
    {
        [Theory, ConstruktionData]
        public void should_sign_with_team(Team team, Player player)
        {
            player.SignWith(team);

            Insert(team, player);

            var foundPlayer = Query(db => db.Players.FirstOrDefault(x => x.Id == player.Id));

            foundPlayer.ShouldNotBeNull();
            foundPlayer.Name.ShouldBe(player.Name);
            foundPlayer.TeamId.ShouldBe(team.Id);
        }

        [Theory, ConstruktionData]
        public void should_leave_team(Team team, Player player)
        {
            AssignToATeam(player);

            player.LeaveTeam();
            Update(player);

            var foundPlayer = Query(db => db.Players.FirstOrDefault(x => x.Id == player.Id));

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