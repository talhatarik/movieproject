using System;

namespace restapi.Types
{
    public class MovieContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FavoriteCount { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public int MyRating { get; set; }
        public string MyNotes { get; set; }
    }

}