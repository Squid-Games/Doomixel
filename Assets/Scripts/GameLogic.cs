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

struct RoomTile
{
    public Vector2Int Position { get; set; }
    public GameObject Floor { get; set; }
    public List<GameObject> Walls { get; set; }
    public GameObject Ceiling { get; set; }
}

public class GameLogic : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject ceilingPrefab;

    public int        minTilesRoom;
    public int        minRoomRectangleWidth;
    public int        maxRoomRectangleWidth;

    public float      roomTileScale = 1.0f;

    List<RoomTile>    _roomTiles;

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
        _roomTiles = new List<RoomTile>();

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
            floor.transform.position = new Vector3(tile.x * 10.0f * roomTileScale, 0.0f, tile.z * 10.0f * roomTileScale);
            floor.transform.localScale = new Vector3(roomTileScale, 1.0f, roomTileScale);

            
            var ceiling = Instantiate(ceilingPrefab);
            ceiling.transform.position = new Vector3(floor.transform.position.x, 0.0f, floor.transform.position.z);
            
            var actualCeiling = ceiling.transform.GetChild(0);
            actualCeiling.transform.localScale = new Vector3(roomTileScale, 1.0f, roomTileScale);
            actualCeiling.transform.localPosition = new Vector3(actualCeiling.transform.localPosition.x, 
                actualCeiling.transform.localPosition.y * roomTileScale, 
                actualCeiling.transform.localPosition.z);

            var roomTile = new RoomTile()
            {
                Position = tile,
                Floor = floor,
                Walls = new List<GameObject>(),
                Ceiling = ceiling
            };

            _roomTiles.Add(roomTile);
        }

        AddWalls(chosenTiles);
    }

    private void AddWalls(HashSet<Vector2Int> chosenTiles)
    {
        foreach (var tile in _roomTiles)
        {
            int[] dx = { 0, -1, 0, 1 };
            int[] dz = { -1, 0, 1, 0 };
            for (int i = 0; i < dx.Length; i++)
            {
                Vector2Int nextPos = tile.Position + new Vector2Int(dx[i], dz[i]);
                if (!chosenTiles.Contains(nextPos))
                {
                    var wallObject = Instantiate(wallPrefab);
                    wallObject.transform.position = tile.Floor.transform.position;
                    wallObject.transform.rotation = Quaternion.Euler(0.0f, i * 90.0f, 0.0f);

                    var actualWall = wallObject.transform.GetChild(0);
                    actualWall.transform.localScale = new Vector3(roomTileScale, 1.0f, roomTileScale);
                    actualWall.transform.localPosition = new Vector3(actualWall.transform.localPosition.x,
                        actualWall.transform.localPosition.y * roomTileScale,
                        actualWall.transform.localPosition.z * roomTileScale);

                    tile.Walls.Add(wallObject);
                }
            }
        }
    }
}
