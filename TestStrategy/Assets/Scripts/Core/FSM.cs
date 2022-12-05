using System;
using System.Collections.Generic;

namespace Core
{
    public class FSM
    {
        public abstract class State
        {
            public abstract void OnEnter();
            public abstract void OnUpdate();
            public abstract void OnExit();
        }

        public class Default : State
        {
            public override void OnEnter() { }

            public override void OnExit() { }

            public override void OnUpdate() { }
        }

        public class Agressive : State
        {
            public override void OnEnter() { }

            public override void OnExit() { }

            public override void OnUpdate() { }
        }

        private readonly Dictionary<Type, State> _states = new()
        {
            { typeof(Default), new Default() },
            { typeof(Agressive), new Agressive() },
        };

        private State _currentState;

        public void ChangeState(Type stateType)
        {
            _currentState?.OnExit();
            _currentState = _states.TryGetValue(stateType, out var result) ? result : null;
            _currentState?.OnEnter();
        }

        public void UpdateState() => _currentState?.OnUpdate();
    }
}