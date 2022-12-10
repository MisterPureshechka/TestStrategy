using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    public static class PathFinder
    {
        private class PathFinderException : Exception
        {
            public PathFinderException(string message) : base($"Path finder ERROR: {message}") { }
        }

        public static Vector3 ToNavArea(this Vector3 value)
        {
            if (!NavMesh.SamplePosition(value, out var hit, float.MaxValue, -1))
                throw new PathFinderException("Can`t find navigation mesh area");
            return hit.position;
        }
    }    
}