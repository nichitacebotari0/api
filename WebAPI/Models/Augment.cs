using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Augment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int AugmentCategoryId { get; set; }
        public AugmentCategory AugmentCategory { get; set; }
        public int AbilityTypeId { get; set; }
        public AbilityType AbilityType { get; set; }
        public int HeroId { get; set; }
        public Hero Hero { get; set; }

        public override string ToString()
        {
            return @$"{Id} {Name} AugmentCategory:{AugmentCategoryId} {AugmentCategory?.Name} 
;AbilityType:{AbilityTypeId} {AbilityType?.Name} 
;Hero:{HeroId} {Hero?.Name}";
        }
    }
}
