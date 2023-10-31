using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IServiceLocator 
{
	public T Resolve<T>();
	public void Register<T,T2>(T2 instance) where T2 : T;
	public void Unregister<T>(T instance);
}
