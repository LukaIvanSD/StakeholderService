namespace Stakeholders.Api.Dtos
{
    public class PersonDto
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Bio { get; set; }
        public string Moto { get; set; }
        public string PictureBase64 { get; set; }

    }
}
