using JWT_Authentication.DTO;

namespace JWT_Authentication.Services;

public class UserService
{
    private static readonly List<User> Users = new()
        {
            new User { Id= 1, Username = "admin", Password = "password", Name = "Admin",      Role = "Admin" },
            new User { Id= 2, Username = "john",  Password = "password", Name = "John Cena",  Role = "User" },
            new User { Id= 3, Username = "sam",   Password = "password", Name = "Sam Curran", Role = "User" }
        };

    public User? ValidateUser(string username, string password)
    {
        return Users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}