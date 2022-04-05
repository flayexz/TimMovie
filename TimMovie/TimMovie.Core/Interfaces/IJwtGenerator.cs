using TimMovie.Core.Entities;

namespace TimMovie.Core.Interfaces;

public interface IJwtGenerator
{
    string CreateToken(User user);
}