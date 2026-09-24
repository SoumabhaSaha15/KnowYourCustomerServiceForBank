using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KnowYourCustomerServiceForBank.Server.Config;

public class DistributedCacheTicketStore(IDistributedCache cache) : ITicketStore
{
  private const string KeyPrefix = "AuthSession-";

  public async Task<string> StoreAsync(AuthenticationTicket ticket)
  {
    var key = $"{KeyPrefix}{Guid.NewGuid():N}";
    await RenewAsync(key, ticket);
    return key;
  }

  public async Task RenewAsync(string key, AuthenticationTicket ticket)
  {
    var val = TicketSerializer.Default.Serialize(ticket);
    var options = new DistributedCacheEntryOptions();

    if (ticket.Properties.ExpiresUtc.HasValue)
      options.SetAbsoluteExpiration(ticket.Properties.ExpiresUtc.Value);
    else
      options.SetSlidingExpiration(TimeSpan.FromHours(8));

    await cache.SetAsync(key, val, options);
  }

  public async Task<AuthenticationTicket?> RetrieveAsync(string key)
  {
    var val = await cache.GetAsync(key);
    return val == null ? null : TicketSerializer.Default.Deserialize(val);
  }

  public async Task RemoveAsync(string key)
  {
    await cache.RemoveAsync(key);
  }
}