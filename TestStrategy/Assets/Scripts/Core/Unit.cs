using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour
    {
        public static HashSet<Unit> Units = new();

        [SerializeField] private int _team;
        public int Team => _team;

        private NavMeshAgent _navAgent;
        public NavMeshAgent NavAgent => _navAgent = _navAgent != null ? _navAgent : _navAgent = GetComponent<NavMeshAgent>();

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

        public async void ChangePath(Vector3 targetPos) => await NavAgent.MoveTo(targetPos);
    }
}