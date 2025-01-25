namespace GameStore.DataAccessLayer.Interfaces.Entities
{
    using System;
    using System.Collections.Generic;

    public class Genre
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ParentGenreId { get; set; }

        public ICollection<GameGenre> GameGenres { get; set; }
    }
}
