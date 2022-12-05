using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    public static class PathFinder
    {
        private const int UPDATE_MS = 100;
        public static async Task MoveTo(this NavMeshAgent navAgent, Vector3 targetPos, float doneRadius = 2f)
        {
            navAgent.transform.position = navAgent.transform.position.MoveToNavArea();
            navAgent.isStopped = true;

            await Task.Delay(UPDATE_MS);
            if (Application.isPlaying && navAgent.isActiveAndEnabled)
            {
                navAgent.destination = targetPos.MoveToNavArea();
                navAgent.isStopped = false;
            }
            while (Application.isPlaying && (!navAgent.isActiveAndEnabled || !navAgent.isStopped))
            {
                navAgent.isStopped = Vector3.Distance(navAgent.transform.position, navAgent.destination) < doneRadius;
                await Task.Delay(UPDATE_MS);
            }
        }

        private static Vector3 MoveToNavArea(this Vector3 value)
        {
            if (!NavMesh.SamplePosition(value, out var hit, float.MaxValue, -1))
                throw new PathFinderException("Can`t find navigation mesh area");
            return hit.position;
        }

        private class PathFinderException : Exception
        {
            public PathFinderException(string message) : base($"Path finder ERROR: {message}") { }
        }
    }    
}