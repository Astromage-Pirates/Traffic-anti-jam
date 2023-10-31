using System;

namespace AstroPirate.DesignPatterns 
{
	public abstract class EventContext { };

	public delegate void EventHandler(EventContext @env);

	public interface IEventBus
	{
		public void Register<T>(System.Action<T> eventBus) where T : EventContext;
		public void UnRegister<T>(System.Action<T> eventBus) where T : EventContext;
		public void Send<T>(T @event) where T : EventContext;
	}
}
