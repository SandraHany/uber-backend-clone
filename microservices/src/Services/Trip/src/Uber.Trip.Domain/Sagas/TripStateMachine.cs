using Automatonymous;
using MassTransit;
using System;
/*
public class TripStateMachine : MassTransitStateMachine<TripState>
{
    public Event<TripRequested> TripRequested { get; private set; }
    public Event<TripAccepted> TripAccepted { get; private set; }
    public Event<TripCancelled> TripCancelled { get; private set; }
    public Event<TripCompleted> TripCompleted { get; private set; }
    public Event<TripInProgress> TripInProgress { get; private set; }
    public Event<TripStarted> TripStarted { get; private set; }


    public TripStateMachine()
    {
       /* Event(() => TripRequested, x => x.CorrelateById(mbox => mbox.Message.TripId));
        Event(() => TripAccepted, x => x.CorrelateById(mbox => mbox.Message.TripId));
        Event(() => TripStarted, x => x.CorrelateById(mbox => mbox.Message.TripId));
        Event(() => TripCompleted, x => x.CorrelateById(mbox => mbox.Message.TripId));
        Event(() => TripCancelled, x => x.CorrelateById(mbox => mbox.Message.TripId));
        Event(() => TripInProgress, x => x.CorrelateById(mbox => mbox.Message.TripId)); 

        InstanceState(x->x.CurrentState);
        Initially(
            When(TripRequested)
                .Then(context =>
                {
                    context.Saga.TripRequestedAt = DateTime.UtcNow;

                })
                .PublishAsync(context => context.Init < ProcessTripRequest)
        );*/
 //   }

//}