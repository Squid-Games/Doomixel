using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

struct Vector2Int
{
    public Vector2Int(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        a.x += b.x;
        a.z += b.z;
        return a;
    }

    public int x;
    public int z;
}

public class GameLogic : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject floorPrefab;

    public int        minTilesRoom;
    public int        minRoomRectangleWidth;
    public int        maxRoomRectangleWidth;

    void Start()
    {
        AssignPlayer();
        CreateRoom();
    }

    void Update()
    {
        if (playerObject is null)
            AssignPlayer();
    }

    private void AssignPlayer()
    {
        playerObject = GameObject.FindWithTag("Player");
    }

    private void CreateRoom()
    {
        HashSet<Vector2Int> chosenTiles = new HashSet<Vector2Int>();

        while (chosenTiles.Count < minTilesRoom)
        {
            Vector2Int origin = new Vector2Int(0, 0);
            if (chosenTiles.Count > 0)
                origin = chosenTiles.ElementAt(Random.Range(0, chosenTiles.Count));

            Vector2Int rectSize = new Vector2Int(Random.Range(minRoomRectangleWidth, maxRoomRectangleWidth + 1),
                                                 Random.Range(minRoomRectangleWidth, maxRoomRectangleWidth + 1));

            Vector2Int pointsDifference = rectSize + new Vector2Int(-1, -1);

            Vector2Int topRight = origin + pointsDifference;

            if (Random.Range(0, 2) == 0)
            {
                origin = new Vector2Int(origin.x - pointsDifference.x, origin.z);
                topRight = new Vector2Int(topRight.x - pointsDifference.x, topRight.z);
            }

            if (Random.Range(0, 2) == 0)
            {
                origin = new Vector2Int(origin.x, origin.z - pointsDifference.z);
                topRight = new Vector2Int(topRight.x, topRight.z - pointsDifference.z);
            }

            for (int x = origin.x; x <= topRight.x; x++)
            {
                for (int z = origin.z; z <= topRight.z; z++)
                {
                    Vector2Int newPoint = new Vector2Int(x, z);
                    if (!chosenTiles.Contains(newPoint))
                        chosenTiles.Add(newPoint);
                }
            }
        }

        foreach (var tile in chosenTiles)
        {
            var floor = Instantiate(floorPrefab);
            floor.transform.position = new Vector3(tile.x * 10.0f * floor.transform.localScale.x, 0.0f, tile.z * 10.0f * floor.transform.localScale.z);
        }
    }
}
