namespace At.luki0606.DartZone.API.Mappers
{
    public interface IDtoMapper<in TEntity, out TDto>
        where TEntity : class
        where TDto : class
    {
        TDto Map(TEntity entity);
    }
}
