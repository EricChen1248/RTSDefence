using UnityEngine;

namespace Scripts.Resources
{
    public class Allocate : MonoBehaviour
    {
        private GameObject _resourceGameObject;
        public Terrain WorldTerrain;
        // Use this for initialization
        public void Start()
        {
            _resourceGameObject = new GameObject {name = "Resources"};
            WorldTerrain = GetComponent<Terrain>();

            InstantiateRandomPosition("Prefabs/Resources/Nodes/tree1", 100, 10, 256f, 5);
            InstantiateRandomPosition("Prefabs/Resources/Nodes/tree2", 100, 10, 256f, 5);
            InstantiateRandomPosition("Prefabs/Resources/Nodes/tree1", 100, 25, 400f, 8);
            InstantiateRandomPosition("Prefabs/Resources/Nodes/stone1", 400, 20, 256f, 5);
            InstantiateRandomPosition("Prefabs/Resources/Nodes/coal", 200, 20, 256f, 5);
            InstantiateRandomPosition("Prefabs/Resources/Nodes/Gold", 100, 20, 256f, 5);
        }
        
        public void InstantiateRandomPosition (string resource, int amount, int groupCount, float minSize, int groupRadius) {
            // define variable
            // loop through the amount
            // generate random position
            var i = 0;
            var go = UnityEngine.Resources.Load<GameObject>(resource);

            do
            {
                var radius = Random.Range(5, WorldTerrain.terrainData.size.x / 2);
                var groupPos = Random.insideUnitCircle * radius;
            
                if (groupPos.sqrMagnitude < minSize)
                {
                    continue;
                }

                var randomPosition = new Vector3(groupPos.x, 0, groupPos.y);

                for (var j = 0; j < groupCount; j++)
                {
                    var pos = Random.insideUnitCircle * groupRadius;
                    var newPos = randomPosition + new Vector3(pos.x, 0, pos.y);

                    var r = Instantiate(go, newPos, Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0));
                    r.transform.parent = _resourceGameObject.transform;
                }

                i += groupCount;

            } while (i < amount);

		
        }
    }
}
