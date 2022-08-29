using Autofac;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, ICollection<IIntegrationEventHandler>> _eventHandlers = new ConcurrentDictionary<Type, ICollection<IIntegrationEventHandler>>();
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILifetimeScope _autofac;

        private readonly string AUTOFAC_SCOPE_NAME = "eshop_event_bus";

        public InMemoryEventBus(IServiceProvider serviceProvider, ILifetimeScope autofac)
        {
            _subsManager = new InMemoryEventBusSubscriptionsManager();
            _serviceProvider = serviceProvider;
            _autofac = autofac;
        }

        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name;
            if (!_subsManager.HasSubscriptionsForEvent(eventName))
                return;

            var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME);
            var subscriptions = _subsManager.GetHandlersForEvent(@event.GetType().Name);
            foreach (var subscription in subscriptions)
            {
                if (subscription.IsDynamic)
                {
                    var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                    if (handler == null) 
                        continue;

                    handler.Handle(@event).Wait();
                }
                else
                {
                    var handler = scope.ResolveOptional(subscription.HandlerType);
                    if (handler == null) continue;
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    var resultTask = (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    resultTask.Wait();
                }
            }

        }

        public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            _subsManager.AddSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public ICollection<IIntegrationEventHandler> GetHandlers(Type eventType)
        {
            var eventHandlers = _eventHandlers.GetOrAdd(eventType, type =>
            {
                var handlerTypes = _subsManager.GetHandlersForEvent(eventType.Name);
                var handlers = handlerTypes
                    .Select(t => (IIntegrationEventHandler)_serviceProvider.GetService(t.HandlerType))
                    .ToArray();
                return handlers;
            });
            return eventHandlers;
        }
    }
}
