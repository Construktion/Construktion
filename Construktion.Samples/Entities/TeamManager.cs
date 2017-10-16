namespace Construktion.Samples.Entities
{
    public class TeamManager
    {
        private readonly AgentService _agentService;

        public TeamManager(AgentService agentService)
        {
            _agentService = agentService;
        }

        public void Contact(Agent agent)
        {
            var calledAgent = _agentService.CallAgent(agent.Id);

            if (calledAgent)
            {
                agent.ContactedAgent();
            }
        }
    }

    public interface AgentService
    {
        bool CallAgent(int agentId);
    }

    public class Agent : Entity
    {
        public int Id { get; set; }
        public bool ContactedByTeam { get; set; }

        public void ContactedAgent()
        {
            ContactedByTeam = true;
        }
    }
}