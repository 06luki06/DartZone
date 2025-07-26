namespace At.luki0606.DartZone.API.Mappers
{
    public interface IDtoMapperFactory
    {
        IDtoMapper<TEntity, TDto> GetMapper<TEntity, TDto>()
            where TEntity : class
            where TDto : class;
    }
}
