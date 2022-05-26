public class UserRepository : IUserRepository
{
    List<UserDto> _users => new()
    {
        new UserDto("John", "123"),
        new UserDto("Monica", "123"),
        new UserDto("Nancy", "123")
    };

    public UserDto GetUser(UserModel userModel) =>
        _users.FirstOrDefault(x => 
            x.UserName == userModel.UserName &&
            x.Password == userModel.Password) ??
            throw new Exception();
}