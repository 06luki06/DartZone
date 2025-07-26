using System;
using Microsoft.Extensions.DependencyInjection;

namespace At.luki0606.DartZone.API.Mappers
{
    public class DtoMapperFactory : IDtoMapperFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DtoMapperFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IDtoMapper<TEntity, TDto> GetMapper<TEntity, TDto>()
            where TEntity : class
            where TDto : class
        {
            IDtoMapper<TEntity, TDto> mapper = _serviceProvider.GetService<IDtoMapper<TEntity, TDto>>();
            return mapper ?? throw new InvalidOperationException($"No mapper registered for {typeof(TEntity).Name} to {typeof(TDto).Name}");
        }
    }
}
