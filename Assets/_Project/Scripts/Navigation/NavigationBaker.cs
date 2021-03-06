﻿using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Navigation
{
    [DefaultExecutionOrder(-102)]
    public class NavigationBaker : MonoBehaviour
    {
        private NavMeshSurface[] _surface;

        private AsyncOperation[] _updateOps;
        public static NavigationBaker Instance { get; private set; }

        public void Start()
        {
            Instance = this;
            _surface = GetComponents<NavMeshSurface>();
            Build();

            _updateOps = new AsyncOperation[_surface.Length];
        }

        public void Rebuild()
        {
            for (var i = 0; i < _surface.Length; i++)
            {
                if (_updateOps[i] != null && !_updateOps[i].isDone) return;
                var op = _surface[i].UpdateNavMesh(_surface[i].navMeshData);
                _updateOps[i] = op;
            }
        }

        public void Build()
        {
            foreach (var navMeshSurface in _surface) navMeshSurface.BuildNavMesh();
        }
    }
}