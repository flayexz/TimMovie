using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimMovie.Core.Entities;
using TimMovie.Core.Entities.Enums;
using TimMovie.Core.Interfaces;

namespace TimMovie.Infrastructure.Services;

public class UserStatusWorker : BackgroundService
{
    private readonly ILogger<UserStatusWorker> _logger;
    private readonly UserManager<User> _userManager;
    private const int Delay = 5 * 60 * 1000;
    private const int CountMinutes = 5;

    public UserStatusWorker(ILogger<UserStatusWorker> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var user in _userManager.Users)
            {
                if (user.Status is null || user.Status.UserStatusEnum == UserStatusEnum.Offline)
                    continue;
                if (DateTime.Now - user.Status!.DateLastChange <= TimeSpan.FromMinutes(CountMinutes)) continue;
                user.Status.UserStatusEnum = UserStatusEnum.Offline;
                user.Status.DateLastChange = DateTime.Now;
                await _userManager.UpdateAsync(user);
                _logger.LogInformation($"Воркер обновил статусы пользователей");
            }

            await Task.Delay(Delay, stoppingToken);
        }
    }
}