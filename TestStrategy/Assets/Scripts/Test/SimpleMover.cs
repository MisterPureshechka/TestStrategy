using Core;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Test
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SimpleMover : MonoBehaviour
    {
        private NavMeshAgent _navAgent;
        private NavMeshAgent NavAgent => _navAgent ??= _navAgent = GetComponent<NavMeshAgent>();


        private async void Start()
        {
            await NavAgent.MoveTo(Vector3.zero);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
                ChangePath();
        }

        private async void ChangePath()
        {
            await NavAgent.MoveTo(Random.insideUnitSphere * 10f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(NavAgent.destination, 0.25f);
            Gizmos.DrawLine(NavAgent.transform.position, NavAgent.destination);
        }
    }
}