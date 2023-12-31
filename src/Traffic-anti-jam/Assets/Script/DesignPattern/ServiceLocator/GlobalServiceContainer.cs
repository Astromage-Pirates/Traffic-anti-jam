using System;
using System.Collections;
using System.Collections.Generic;
using AstroPirate.DesignPatterns;
using UnityEngine;

public static class GlobalServiceContainer
{
    private static IServiceLocator serviceLocator = new ServiceLocator();

    public static bool Resolve<T>(out T value)
    {
        value = serviceLocator.Resolve<T>();
        return !EqualityComparer<T>.Default.Equals(value, default);
    }

    public static void Register<T, T2>(T2 instance)
        where T2 : T
    {
        serviceLocator.Register<T, T2>(instance);
    }

    public static void Unregister<T>(T instance)
    {
        serviceLocator.Unregister<T>(instance);
    }

    static GlobalServiceContainer()
    {
        Register<IEventBus, EventBus>(new EventBus());
        Register<IMenuManager, MenuManager>(new MenuManager());
    }
}
