﻿namespace DeveloperStore.Repositories.DataMapper;

public static class SimpleMapper
{
    private readonly static SimpleMapperWorker mapper = new(new SimpleMapperCache());

    public static IEnumerable<string> Map<T, U>(T source, U target)
        where T : class
        where U : class
        => mapper.Map(source, target);

    public static U Map<U>(object source) where U : class, new()
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var target = new U();

        var sourceType = source.GetType();
        var targetType = typeof(U);

        Map(source, target, mapper.GetMapping(sourceType, targetType));

        return target;
    }

    public static void Map(object source, object target, IEnumerable<Mapping> mappings)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (target == null)
            throw new ArgumentNullException(nameof(target));
        if (mappings == null)
            throw new ArgumentNullException(nameof(mappings));

        foreach (var item in mappings)
            item.Target.SetValue(target, item.Source.GetValue(source));
    }
}
