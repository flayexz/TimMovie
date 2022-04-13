using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;

namespace TimMovie.Core.Services;

public class BannerService
{
    private readonly IRepository<Banner> _bannerRepository;

    public BannerService(IRepository<Banner> bannerRepository)
    {
        _bannerRepository = bannerRepository;
    }
    
    public async Task<IEnumerable<Banner>> GetBannersAsync()=> 
        await _bannerRepository.GetAllAsync();
}