using System.Collections.Generic;

namespace Construktion.Samples.Entities
{
    public class PlayerBag
    {
        public bool IsActive { get; set; }
        public IEnumerable<Player> Players { get; set; }
    }
}