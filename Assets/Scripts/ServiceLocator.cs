using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static Dictionary<Type, object> services = new Dictionary<Type, object>();

    // Register service instance
    public static void Register<T>(T service)
    {
        var type = typeof(T);

        if (services.ContainsKey(type))
        {
            throw new Exception($"Service of type {type} is already registered.");
        }

        services[type] = service;
    }

    // Replace (or Register if not exists)
    public static void Replace<T>(T service)
    {
        var type = typeof(T);
        services[type] = service;
    }

    // Resolve service instance
    public static T Get<T>()
    {
        var type = typeof(T);

        if (services.TryGetValue(type, out var service))
        {
            return (T)service;
        }

        throw new Exception($"Service of type {type} is not registered.");
    }

    // Unregister a service (optional)
    public static void Unregister<T>()
    {
        var type = typeof(T);

        if (services.ContainsKey(type))
        {
            services.Remove(type);
        }
    }

    // Clear all (optional)
    public static void ClearAll()
    {
        services.Clear();
    }
}