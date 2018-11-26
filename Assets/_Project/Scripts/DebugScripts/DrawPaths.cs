using Scripts.Entity_Components;
using Scripts.Entity_Components.Friendlies;
using UnityEngine;

namespace Scripts.DebugScripts
{
    public class DrawPaths : MonoBehaviour
    {

        private bool _showing = true;
        public void Click()
        {

            var objects = FindObjectsOfType<PlayerComponent>();
            foreach (var playerComponent in objects)
            {
                if (!_showing)
                {
                    playerComponent.gameObject.AddComponent<PathDrawer>();
                }
                else
                {
                    Destroy(playerComponent.gameObject.GetComponent<PathDrawer>());
                }
            }

            _showing = !_showing;
        }
    }
}
