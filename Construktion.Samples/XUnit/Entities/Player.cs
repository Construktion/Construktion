namespace Construktion.Samples.XUnit.Entities
{
    public class Player : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? TeamId { get; set; }
        public Team Team { get; set; }

        public void SignWith(Team team)
        {
            Team = team;
        }

        public void LeaveTeam()
        {
            TeamId = null;
            Team = null;
        }
    }
}