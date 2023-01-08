using AutoMapper;
using ListerSS.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ListerSS.SignalR
{
    public class DataFilter : IHubFilter
    {
        private readonly IMapper _mapper;
        public DataFilter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext,
            Func<HubInvocationContext, ValueTask<object?>> next)
        {
            if (invocationContext.HubMethodArguments.SingleOrDefault() is HubMessage message)
            {
                //var message = _mapper.Map<HubMessage>(messageDto);
                message.Id = Guid.NewGuid();
                message.FromName = invocationContext.Context.User.FindFirstValue("Name");
                message.CreatedAt = DateTime.Now;
                var arguments = invocationContext.HubMethodArguments.ToArray();
                arguments[0] = message;

                invocationContext = new HubInvocationContext(invocationContext.Context,
                    invocationContext.ServiceProvider,
                    invocationContext.Hub,
                    invocationContext.HubMethod,
                    arguments);
            }
            return await next(invocationContext);
        }
    }
}

