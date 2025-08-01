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
            return _serviceProvider.GetRequiredService<IDtoMapper<TEntity, TDto>>();
        }
    }
}
