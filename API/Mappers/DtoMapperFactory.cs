using System;
using At.luki0606.DartZone.Shared.Results;
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

        public Result<IDtoMapper<TEntity, TDto>> GetMapper<TEntity, TDto>()
            where TEntity : class
            where TDto : class
        {
            IDtoMapper<TEntity, TDto> service = _serviceProvider.GetService<IDtoMapper<TEntity, TDto>>();
            if (service is null)
            {
                return Result<IDtoMapper<TEntity, TDto>>.Failure(
                    $"No mapper found for {typeof(TEntity).Name} to {typeof(TDto).Name}");
            }
            return Result<IDtoMapper<TEntity, TDto>>.Success(service);
        }
    }
}
