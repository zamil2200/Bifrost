﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Bifrost.Events;
using Bifrost.Events.InProcess;
using Bifrost.Execution;

namespace Bifrost.Configuration
{
    /// <summary>
    /// Represents an implementation of <see cref="IEventsConfiguration"/>
    /// </summary>
    public class EventsConfiguration : ConfigurationStorageElement, IEventsConfiguration
    {
        /// <summary>
        /// Initializes an instance of <see cref="EventsConfiguration"/>
        /// </summary>
        public EventsConfiguration()
        {
            CommittedEventStreamSender = typeof(CommittedEventStreamSender);
            CommittedEventStreamReceiver = typeof(CommittedEventStreamReceiver);
            EventProcessorLog = typeof(NullEventProcessorLog);

            EventStore = new EventStoreConfiguration();
            EventSourceVersions = new EventSourceVersionsConfiguration();
            EventSequenceNumbers = new EventSequenceConfiguration();
            EventProcessorStates = new EventProcessorStatesConfiguration();
        }

        /// <inheritdoc/>
        public Type CommittedEventStreamSender { get; set; }

        /// <inheritdoc/>
        public Type CommittedEventStreamReceiver { get; set; }

        /// <inheritdoc/>
        public Type EventProcessorLog { get; set; }

        /// <inheritdoc/>
        public EventStoreConfiguration EventStore { get; }

        /// <inheritdoc/>
        public EventSourceVersionsConfiguration EventSourceVersions { get; }

        /// <inheritdoc/>
        public EventSequenceConfiguration EventSequenceNumbers { get; }

        /// <inheritdoc/>
        public EventProcessorStatesConfiguration EventProcessorStates { get; }

        /// <inheritdoc/>
        public override void Initialize(IContainer container)
        {
            container.Bind<ICanSendCommittedEventStream>(CommittedEventStreamSender, BindingLifecycle.Singleton);
            container.Bind<ICanReceiveCommittedEventStream>(CommittedEventStreamReceiver, BindingLifecycle.Singleton);
            container.Bind<IEventStore>(EventStore.EventStore, BindingLifecycle.Singleton);
            container.Bind<IEventSourceVersions>(EventSourceVersions.EventSourceVersions, BindingLifecycle.Singleton);
            container.Bind<IEventSequenceNumbers>(EventSequenceNumbers.EventSequenceNumbers, BindingLifecycle.Singleton);
            container.Bind<IEventProcessorLog>(EventProcessorLog, BindingLifecycle.Singleton);
            container.Bind<IEventProcessorStates>(EventProcessorStates.EventProcessorStates, BindingLifecycle.Singleton);

            if (EntityContextConfiguration != null)
            {
                EntityContextConfiguration.BindEntityContextTo<IEvent>(container);
                base.Initialize(container);
            }
        }
    }
}