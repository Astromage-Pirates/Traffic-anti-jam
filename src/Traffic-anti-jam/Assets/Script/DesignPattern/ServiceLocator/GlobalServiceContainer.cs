using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalServiceContainer
{
	private static IServiceLocator serviceLocator = new ServiceLocator();
	public static T Resolve<T>()
	{
		return serviceLocator.Resolve<T>();
	}

	public static void Register<T, T2>(T2 instance) where T2 : T
	{
		serviceLocator.Register<T, T2>(instance);
	}

	public static void Unregister<T>(T instance)
	{
		serviceLocator.Unregister<T>(instance);
	}
}
