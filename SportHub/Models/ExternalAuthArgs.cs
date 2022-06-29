namespace SportHub.Models
{
    public class ExternalAuthArgs
    {
        public string UserToken { get; set; }
        public string AuthProvider { get; set; }
        public bool IsCreationRequired { get; set; }
    }
}
