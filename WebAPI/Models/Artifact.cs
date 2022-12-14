namespace WebAPI.Models
{
    public class Artifact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int ArtifactTypeId { get; set; }
        public ArtifactType ArtifactType { get; set; }
    }
}
