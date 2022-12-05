using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class FSM
    {
        public abstract class State
        {
            protected Unit _unit;
            public State(Unit unit) => _unit = unit;
            public abstract void OnEnter();
            public abstract void OnUpdate();
            public abstract void OnExit();
        }

        public class Default : State
        {
            private float _agrDistance = 5f;
            private float _randomRadius = 15f;

            private Unit NearestUnit => Unit.Units.Where(u => u.Team != _unit.Team)
                    .OrderBy(u => Vector3.Distance(u.transform.position, _unit.transform.position))
                    .FirstOrDefault(u => Vector3.Distance(u.transform.position, _unit.transform.position) < _agrDistance);

            public Default(Unit unit) : base(unit) { }

            public override void OnEnter()
            {
                _unit.NavAgent.destination = _unit.transform.position + UnityEngine.Random.insideUnitSphere * _randomRadius;
                _unit.NavAgent.isStopped = false;
            }

            public override void OnExit() { }

            public override void OnUpdate() 
            {
                if (_unit != null)
                    return;

                if (NearestUnit != null)
                    _unit.FiniteStateMachine.ChangeState(typeof(Agressive));
                else if (Vector3.Distance(_unit.NavAgent.destination, _unit.transform.position) < 2f)
                    _unit.FiniteStateMachine.ChangeState(typeof(Default));
            }
        }

        public class Agressive : State
        {
            private float _defaultDistance = 10f;
            private float _attackDistance = 3f;

            private Unit NearestUnit => Unit.Units.Where(u => u.Team != _unit.Team)
                    .OrderBy(u => Vector3.Distance(u.transform.position, _unit.transform.position))
                    .FirstOrDefault(u => Vector3.Distance(u.transform.position, _unit.transform.position) > _defaultDistance);

            public Agressive(Unit unit) : base(unit) { }

            public override void OnEnter() { }

            public override void OnExit() { }

            public override void OnUpdate() 
            {
                if (_unit != null)
                    return;

                var nearestUnit = NearestUnit;
                if (nearestUnit == null)
                    _unit.FiniteStateMachine.ChangeState(typeof(Default));
                else if (Vector3.Distance(nearestUnit.transform.position, _unit.transform.position) < _attackDistance)
                {
                    _unit.NavAgent.isStopped = true;
                }
                else
                {
                    _unit.NavAgent.destination = nearestUnit.transform.position;
                    _unit.NavAgent.isStopped = false;
                } 
            }
        }

        private Dictionary<Type, State> _states;

        private Unit _unit;
        private State _currentState;

        public FSM(Unit unit)
        {
            _unit = unit;
            _states = new()
            {
                { typeof(Default), new Default(_unit) },
                { typeof(Agressive), new Agressive(_unit) },
            };
            ChangeState(typeof(Default));
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