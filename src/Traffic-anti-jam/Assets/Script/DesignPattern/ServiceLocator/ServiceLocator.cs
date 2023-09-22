using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceLocator :  IServiceLocator
{
	
	Dictionary<Type,object> services = new();

	public T Resolve<T>()
	{
		var type = typeof(T);
		if(services.TryGetValue(type,out var instance))
		{
			return (T)instance;
		}
		Debug.LogWarningFormat("{0} is not registered",type.Name);
		return default(T);
	}

	public void Register<T, T2>(T2 instance) where T2 : T
	{
		if (!services.TryAdd(typeof(T), instance))
		{
			Debug.LogWarning($"{typeof(T).Name} is already registered");
		}
	}

	public void Unregister<T>(T instance)
	{
		var type = typeof(T);
		if (services.TryGetValue(type, out var _instance))
		{
			if (_instance.GetHashCode() != instance.GetHashCode())
			{
				throw new ArgumentException("Unregister instance is not registered one");
			}
			
			services.Remove(type);
		}
		Debug.LogWarningFormat("{0} is not registered", type.Name);
	}
}
