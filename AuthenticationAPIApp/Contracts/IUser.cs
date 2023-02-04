namespace AuthenticationAPIApp.Contracts
{
    public interface IUser
    {
        string UserName { get; set; }
        string Password { get; set; }
        string Role { get; set; }
        string Scope { get; set; }
    }
}
