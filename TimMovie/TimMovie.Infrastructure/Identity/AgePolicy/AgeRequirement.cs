using Microsoft.AspNetCore.Authorization;

namespace TimMovie.Infrastructure.Identity;

public class AgeRequirement : IAuthorizationRequirement
{
    protected internal int Age { get; set; }
     
    public AgeRequirement(int age)
    {
        Age = age;
    }
}