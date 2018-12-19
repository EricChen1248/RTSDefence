using System.Collections;
using UnityEngine;

public class Allocate : MonoBehaviour
{

    public Terrain WorldTerrain;
    public LayerMask TerrainLayer;
    public static float TerrainLeft, TerrainRight, TerrainTop,
    TerrainBottom, TerrainWidth, TerrainLength, TerrainHeight;

    public static ArrayList units = new ArrayList();
    public static ArrayList positions = new ArrayList();
    public static ArrayList rotations = new ArrayList();



    // Use this for initialization
    public void Awake()
    {
        WorldTerrain = GetComponent<Terrain>();
        TerrainLeft = WorldTerrain.transform.position.x;
        TerrainBottom = WorldTerrain.transform.position.z;
        TerrainWidth = WorldTerrain.terrainData.size.x;
        TerrainLength = WorldTerrain.terrainData.size.z;
        TerrainHeight = WorldTerrain.terrainData.size.y;
        TerrainRight = TerrainLeft + TerrainWidth;
        TerrainTop = TerrainBottom + TerrainLength;

        InstantiateRandomPosition("Prefabs/Resources/Nodes/tree1", 100, 10);
        InstantiateRandomPosition("Prefabs/Resources/Nodes/tree2", 100, 10);
        InstantiateRandomPosition("Prefabs/Resources/Nodes/stone1", 200, 20);

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
            var radius = Random.Range(5, (TerrainRight - TerrainLeft) / 2);
            var groupPos = Random.insideUnitCircle * radius;
            print(groupPos);
            print(groupPos.sqrMagnitude);
            if (groupPos.sqrMagnitude < 100f)
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
