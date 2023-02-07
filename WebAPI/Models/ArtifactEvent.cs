namespace WebAPI.Models
{
    public class ArtifactEvent
    {
        public int Id { get; set; }
        public int ArtifactId { get; set; }
        public EventAction Action { get; set; }
        public int PatchId { get; set; }
        public Patch Patch { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int? ArtifactTypeId { get; set; }
        public ArtifactType ArtifactType { get; set; }

        public override string ToString()
        {
            return $"{ArtifactId} {Name} dbid:{Id}  ;Action:{Action} ;Patch:{PatchId} {Patch?.Version} ;Type:{ArtifactTypeId} {ArtifactType?.Name}";
        }
    }
}
