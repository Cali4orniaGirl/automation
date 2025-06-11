namespace Models
{
    public class UserTestData
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public bool ShouldSucceed { get; set; }
    }
}