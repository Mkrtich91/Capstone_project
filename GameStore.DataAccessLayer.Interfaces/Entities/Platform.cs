namespace GameStore.DataAccessLayer.Interfaces.Entities
{
    using System;
    using System.Collections.Generic;

    public class Platform
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public ICollection<GamePlatform> GamePlatforms { get; set; }
    }
}
