using System;
using System.Collections.Generic;

namespace Core
{
    public class FSM
    {
        public abstract class State
        {
            public sealed class Default : State
            {
                private float _stopPosition = 2f;
                private float _agrDistance = 5f;
                private float _randomRadius = 15f;

                public Default(Unit unit) : base(unit) { }

                public override void OnEnter()
                {
                    var dest = (_unit.GetPosition() + UnityEngine.Random.insideUnitSphere * _randomRadius).ToNavArea();
                    _unit.NavAgent.destination = dest;
                    _unit.NavAgent.isStopped = false;
                }

                public override void OnExit() { }

                public override void OnUpdate()
                {
                    if (_unit.GetNearestEnemyUnit(out var dist) != null && dist < _agrDistance)
                    {
                        _unit.FiniteStateMachine.ChangeState(typeof(Agressive));
                        return;
                    }

                    if ((_unit.NavAgent.destination, _unit).CheckDistance(_stopPosition))
                        _unit.FiniteStateMachine.ChangeState(typeof(Default));
                }
            }

            public sealed class DefaultPlayer : State
            {

                public DefaultPlayer(Unit unit) : base(unit) { }

                public override void OnEnter()
                {

                }

                public override void OnExit()
                {

                }

                public override void OnUpdate()
                {

                }
            }

            public sealed class Agressive : State
            {
                private float _defaultDistance = 10f;
                private float _attackDistance = 3f;

                public Agressive(Unit unit) : base(unit) { }

                public override void OnEnter() { }

                public override void OnExit() { }

                public override void OnUpdate()
                {
                    var nearestUnit = _unit.GetNearestEnemyUnit(out var distance);
                    if (nearestUnit == null || distance > _defaultDistance)
                    {
                        _unit.FiniteStateMachine.ChangeState(typeof(Default));
                        return;
                    }

                    _unit.NavAgent.destination = nearestUnit.GetPosition();
                    _unit.NavAgent.isStopped = (nearestUnit.GetPosition(), _unit.GetPosition()).CheckDistance(_attackDistance);
                }
            }

            protected Unit _unit;
            public State(Unit unit) => _unit = unit;
            public abstract void OnEnter();
            public abstract void OnUpdate();
            public abstract void OnExit();
        }

        private Dictionary<Type, State> _states;

        private Unit _unit;
        private State _currentState;

        public FSM(Unit unit)
        {
            _unit = unit;
            _states = new()
            {
                { typeof(State.Default), new State.Default(_unit) },
                { typeof(State.Agressive), new State.Agressive(_unit) },
            };
            ChangeState(typeof(State.Default));
        } 

        public void ChangeState(Type stateType)
        {
            _currentState?.OnExit();
            _currentState = _states.TryGetValue(stateType, out var result) ? result : null;
            _currentState?.OnEnter();
        }

        public void UpdateState() => _currentState?.OnUpdate();
    }
}