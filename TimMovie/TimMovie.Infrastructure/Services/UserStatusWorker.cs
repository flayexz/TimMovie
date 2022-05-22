using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TimMovie.Core.Entities;
using TimMovie.Core.Entities.Enums;

namespace TimMovie.Infrastructure.Services;

public class UserStatusWorker : BackgroundService
{
    private readonly UserManager<User> _userManager;
    private const int CountMinutes = 5;
    private const int Delay = 60 * 1000;

    public UserStatusWorker(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var users = _userManager.Users
                .Include(u => u.Status)
                .Where(u => u.Status != null && u.Status.UserStatusEnum != UserStatusEnum.Offline)
                .ToList();
            foreach (var user in users)
            {
                if (DateTime.Now - user.Status!.DateLastChange <= TimeSpan.FromMinutes(CountMinutes)) continue;
                user.Status.UserStatusEnum = UserStatusEnum.Offline;
                user.Status.DateLastChange = DateTime.Now;
                await _userManager.UpdateAsync(user);
            }
            await Task.Delay(Delay, stoppingToken);
        }
    }
}