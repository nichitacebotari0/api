namespace WebAPI.Models
{
    public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int HeroClassId { get; set; }
        public HeroClass HeroClass { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name} ;HeroClass:{HeroClassId} {HeroClass?.Name}";
        }
    }
}
