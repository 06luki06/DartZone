using At.luki0606.DartZone.Shared.Results;

namespace At.luki0606.DartZone.API.Mappers;

internal interface IDtoMapperFactory
{
    Result<IDtoMapper<TEntity, TDto>> GetMapper<TEntity, TDto>()
        where TEntity : class
        where TDto : class;
}
