using Scripts.Entity_Components;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.DebugScripts
{
    public class DrawPaths : MonoBehaviour
    {
        private bool _showing;
        public void Click()
        {
            var objects = FindObjectsOfType<NavMeshAgent>();
            foreach (var playerComponent in objects)
            {
                if (!_showing)
                {
                    playerComponent.gameObject.AddComponent<PathDrawer>();
                }
                else
                {
                    try
                    {
                        Destroy(playerComponent.gameObject.GetComponent<PathDrawer>());
                    }
                    catch (System.Exception)
                    {
                    }
                }
            }

            _showing = !_showing;
        }
    }
}
