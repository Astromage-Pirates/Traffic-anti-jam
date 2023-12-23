using AstroPirate.DesignPatterns;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class EventBus : IEventBus
{
	private readonly Dictionary<System.Type, Dictionary<int, EventHandler>> m_listeners = new();
	public void Register<T>(System.Action<T> listener) where T : EventContext
	{
		var t = typeof(T);
		m_listeners.TryAdd(t, new Dictionary<int, EventHandler>());

		void typeConverter(EventContext @event)
		{
			listener((T) @event);
		}

		m_listeners[t].TryAdd(listener.GetHashCode(),typeConverter);

	}

	public void UnRegister<T>(System.Action<T> listener) where T : EventContext
	{
		var t = typeof(T);
		if(m_listeners.TryGetValue(t,out var listeners))
		{
			if(listeners.ContainsKey(listener.GetHashCode()))
			{
				listeners.Remove(listener.GetHashCode());
			}
			else
			{
				Debug.LogWarning($"Listener is not registered");
			}
		}
		else
		{
			Debug.LogWarning($"there is no listener for type [{t.Name}]");
		}

	}

	public void Send<T>(T ctx) where T : EventContext
	{
		var t = typeof(T);
		if (m_listeners.TryGetValue(t, out var listeners))
		{
            foreach (var item in listeners)
            {
				item.Value(ctx);
            }
        }
	}
}


