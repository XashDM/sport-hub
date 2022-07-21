namespace SportHub.Models
{
    public class RefreshTokenStates
    {
        public int Id { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
    }
}
