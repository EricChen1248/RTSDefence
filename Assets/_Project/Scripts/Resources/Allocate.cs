using System.Collections;
using UnityEngine;

public class Allocate : MonoBehaviour
{

    public Terrain WorldTerrain;
    // Use this for initialization
    public void Awake()
    {
        WorldTerrain = GetComponent<Terrain>();

        InstantiateRandomPosition("Prefabs/Resources/Nodes/tree1", 200, 10);
        InstantiateRandomPosition("Prefabs/Resources/Nodes/tree2", 200, 10);
        InstantiateRandomPosition("Prefabs/Resources/Nodes/stone1", 400, 20);
        InstantiateRandomPosition("Prefabs/Resources/Nodes/coal", 200, 20);
        InstantiateRandomPosition("Prefabs/Resources/Nodes/gold", 100, 20);

    }

    // Update is called once per frame
    public void InstantiateRandomPosition (string Resource, int Amount, int groupCount) {
        //define variable
        //loop throught the amount
        //generate random position
        var i = 0;
        var go = Resources.Load<GameObject>(Resource);

        do
        {
            var radius = Random.Range(5, WorldTerrain.terrainData.size.x / 2);
            var groupPos = Random.insideUnitCircle * radius;
            
            if (groupPos.sqrMagnitude < 256f)
            {
                continue;
            }

            var randomPosition = new Vector3(groupPos.x, 0, groupPos.y);

            for (int j = 0; j < groupCount; j++)
            {
                var pos = Random.insideUnitCircle * 5;
                var newPos = randomPosition + new Vector3(pos.x, 0, pos.y);

                Instantiate(go, newPos, Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0));
            }
            //print("done");

            i += groupCount;

        } while (i < Amount);

		
	}
}
