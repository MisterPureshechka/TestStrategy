using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour
    {
        public static HashSet<Unit> Units = new HashSet<Unit>();

        [SerializeField] private int _team;
        public int Team => _team;

        private NavMeshAgent _navAgent;
        private NavMeshAgent NavAgent => _navAgent ??= _navAgent = GetComponent<NavMeshAgent>();

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

        public async void ChangePath(Vector3 targetPos) => await NavAgent.MoveTo(targetPos);
    }
}