using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private Dictionary<Type, State> _states = new Dictionary<Type, State>()
        {
            { typeof(Default), new Default() },
            { typeof(Agressive), new Agressive() },
        };

        private State _currentState;

        public void ChangeState(Type stateType)
        {
            _currentState?.OnExit();
            _currentState = _states[stateType];
            _currentState.OnEnter();
        }

        public void UpdateState()
        {
            _currentState?.OnUpdate();
        }
    }
}

