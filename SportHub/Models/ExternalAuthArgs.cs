namespace SportHub.Models
{
    public class ExternalAuthArgs
    {
        public string UserToken { get; set; }
        public string AuthProvider { get; set; }
        public bool IsCreationRequired { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
