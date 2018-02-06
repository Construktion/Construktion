namespace Construktion.Samples.XUnit.Entities
{
    using System.Collections.Generic;

    public class PlayerBag
    {
        public bool IsActive { get; set; }
        public IEnumerable<Player> Players { get; set; }
    }
}