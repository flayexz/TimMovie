using TimMovie.Core.Entities;
using TimMovie.Core.Query;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services.Banners;

public class BannerService
{
    private readonly IRepository<Banner> _bannerRepository;

    public BannerService(IRepository<Banner> bannerRepository)
    {
        _bannerRepository = bannerRepository;
    }

    public IEnumerable<Banner> GetBanners()
    {
        var executor = new QueryExecutor<Banner>(_bannerRepository.Query, _bannerRepository);
        executor.IncludeInResult(b => b.Film);
        return executor.GetEntitiesWithPagination(0, 3);
    } 
}