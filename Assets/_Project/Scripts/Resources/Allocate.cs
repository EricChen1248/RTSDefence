using UnityEngine;

namespace Scripts.Resources
{
    public class Allocate : MonoBehaviour
    {
        private GameObject _resourceGameObject;
        public Terrain WorldTerrain;
        // Use this for initialization
        public void Awake()
        {
            _resourceGameObject = new GameObject {name = "Resources"};
            WorldTerrain = GetComponent<Terrain>();

            InstantiateRandomPosition("Prefabs/Resources/Nodes/tree1", 200, 10);
            InstantiateRandomPosition("Prefabs/Resources/Nodes/tree2", 200, 10);
            InstantiateRandomPosition("Prefabs/Resources/Nodes/stone1", 400, 20);
            InstantiateRandomPosition("Prefabs/Resources/Nodes/coal", 200, 20);
            InstantiateRandomPosition("Prefabs/Resources/Nodes/Gold", 100, 20);
        }
        
        public void InstantiateRandomPosition (string resource, int amount, int groupCount) {
            // define variable
            // loop through the amount
            // generate random position
            var i = 0;
            var go = UnityEngine.Resources.Load<GameObject>(resource);

            do
            {
                var radius = Random.Range(5, WorldTerrain.terrainData.size.x / 2);
                var groupPos = Random.insideUnitCircle * radius;
            
                if (groupPos.sqrMagnitude < 256f)
                {
                    continue;
                }

                var randomPosition = new Vector3(groupPos.x, 0, groupPos.y);

                for (var j = 0; j < groupCount; j++)
                {
                    var pos = Random.insideUnitCircle * 5;
                    var newPos = randomPosition + new Vector3(pos.x, 0, pos.y);

                    var r = Instantiate(go, newPos, Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0));
                    r.transform.parent = _resourceGameObject.transform;
                }

                i += groupCount;

            } while (i < amount);

		
        }
    }
}
