using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    public static class UnitEx
    {
        public static bool CheckDistance(this (Vector3 targetLeft, Vector3 targetRight) targets, float distance) =>
            (targets.targetRight.ToNavArea() - targets.targetLeft.ToNavArea()).sqrMagnitude < distance * distance;

        public static bool CheckDistance(this (Vector3 targetLeft, Unit targetRight) targets, float distance) =>
            (targets.targetRight.GetPosition(), targets.targetLeft.ToNavArea()).CheckDistance(distance);

        public static bool CheckDistance(this (Unit targetLeft, Unit targetRight) targets, float distance) =>
            (targets.targetRight.GetPosition(), targets.targetLeft.GetPosition()).CheckDistance(distance);

        public static Vector3 GetPosition(this Unit target) => target.transform.position.ToNavArea();

        public static Unit GetNearestEnemyUnit(this Unit owner, out float distance)
        {
            var targetUnit = owner.GetNearestEnemyUnit();
            distance = targetUnit != null ? Vector3.Distance(targetUnit.GetPosition(), owner.GetPosition()) : -1;
            return targetUnit;
        }

        public static Unit GetNearestEnemyUnit(this Unit owner) => 
            Unit.Units.Where(u => u.Team != owner.Team)
            .OrderBy(u => (u.transform.position - owner.transform.position).sqrMagnitude)
            .FirstOrDefault();
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour
    {
        public static HashSet<Unit> Units = new();

        [SerializeField] private int _team;
        public int Team => _team;

        private NavMeshAgent _navAgent;
        public NavMeshAgent NavAgent => _navAgent = _navAgent != null 
            ? _navAgent 
            : _navAgent = GetComponent<NavMeshAgent>();

        public FSM FiniteStateMachine { get; private set; }

        private void Awake()
        {
            FiniteStateMachine = new FSM(this);
        }

        private void OnEnable()
        {
            Units.Add(this);
        }

        private void OnDisable()
        {
            Units.Remove(this);
        }

        public void OnSelected()
        {

        }

        public void OnDeselected()
        {

        }

        private void Update()
        {
            FiniteStateMachine.UpdateState();
        }
    }
}