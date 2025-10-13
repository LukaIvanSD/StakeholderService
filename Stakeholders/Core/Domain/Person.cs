namespace Stakeholders.Core.Domain
{
    public class Person : Entity
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string? PictureBase64 { get; private set; }
        public string? Bio { get; private set; }
        public string? Moto { get; private set; }
        public long UserId { get; private set; }
        public Person() { }

        public Person(long userId,string name,string surname,string? pictureBase64,string? bio,string? moto)
        {
            UserId = userId;
            Name = name;
            Surname = surname;
            PictureBase64 = pictureBase64;
            Bio = bio;
            Moto = moto;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name");
            if (string.IsNullOrWhiteSpace(Surname)) throw new ArgumentException("Invalid Surname");
        }

        public void UpdateProfile(string name, string surname, string picture, string bio, string moto)
        {
            Name = name;
            Surname = surname;
            PictureBase64 = picture;
            Bio = bio;
            Moto = moto;
            Validate();
        }
    }
}
