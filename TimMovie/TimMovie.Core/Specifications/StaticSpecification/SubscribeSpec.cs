using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.StaticSpecification;

public class SubscribeSpec
{
    public static readonly Specification<UserSubscribe> ActiveSubscribe =
        new(subscribe => subscribe.EndDate > DateTime.Now);
}