namespace WebAPI.Models
{
    public class AugmentEvent
    {
        public int Id { get; set; }
        public int AugmentId { get; set; }
        public EventAction Action { get; set; }
        public int PatchId { get; set; }
        public Patch Patch { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int? AugmentCategoryId { get; set; }
        public AugmentCategory AugmentCategory { get; set; }
        public int? AbilityTypeId { get; set; }
        public AbilityType AbilityType { get; set; }
        public int HeroId { get; set; }
        public Hero Hero { get; set; }

        public override string ToString()
        {
            return @$"{AugmentId} {Name} dbid:{Id} ;Action:{Action} ;Patch:{PatchId} {Patch?.Version} ;AugmentCategory:{AugmentCategoryId} {AugmentCategory?.Name} 
;AbilityType:{AbilityTypeId} {AbilityType?.Name} 
;Hero:{HeroId} {Hero?.Name}";
        }
    }
}
