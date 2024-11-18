namespace UserDataFetcher.Models
{
    public class User
    {
        private string firstName;
        private string lastName;
        private string email;
        private string sourceId;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value ?? string.Empty; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value ?? string.Empty; }
        }

        public string Email
        {
            get { return email; }
            set { email = value ?? string.Empty; }
        }

        public string SourceId
        {
            get { return sourceId; }
            set { sourceId = value ?? string.Empty; }
        }

        public User()
        {
            firstName = string.Empty;
            lastName = string.Empty;
            email = string.Empty;
            sourceId = string.Empty;
        }
    }
}