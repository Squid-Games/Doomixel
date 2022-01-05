using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

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
    public List<Vector2Int> PositionsBehindWalls { get; set; }
    public GameObject Ceiling { get; set; }
}

class Room
{
    public List<RoomTile> RoomTiles { get; set; }
    public GameObject ClosedDoor { get; set; }
    public Vector2Int? ClosedNextTilePosition { get; set; }
    public GameObject Door { get; set; }
    public Vector2Int NextTilePosition { get; set; }
    public Vector2Int DoorPosition { get; set; }
    public List<GameObject> Enemies { get; set; }
}

public class GameLogic : MonoBehaviour
{
    [Serializable]
    public struct EnemyType
    {
        public GameObject prefab;
        public float      chance;
    }

    private const int              MAX_ROOMS_IN_QUEUE = 3;
    public        GameObject       playerObject;
    public        GameObject       floorPrefab;
    public        GameObject       wallPrefab;
    public        GameObject       ceilingPrefab;
                                   
    public        GameObject       doorPrefab;
    public        GameObject       closedDoorPrefab;
                                   
    public        List<EnemyType>  enemyTypes;
                                   
    public        NavMeshSurface   navMeshSurface;
                                   
    public        int              minTilesRoom;
    public        int              minRoomRectangleWidth;
    public        int              maxRoomRectangleWidth;
                                   
    public        float            roomTileScale = 1.0f;
    public        float            panelSize = 10.0f;
                  
    public        int              enemiesCount;

    private       LinkedList<Room> _roomsList;
    private       Room             _currentRoom;
    public        bool             gamePaused = false;
    public        GameObject       pauseMenuComponent;

    void Start()
    {
        AssignPlayer();
        _roomsList = new LinkedList<Room>();
        _currentRoom = CreateRoom(null);
        _roomsList.AddLast(_currentRoom);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.gamePaused)
                this.ContinueGame();
            else
                this.PauseGame();
            return;
        }
        
        if (playerObject is null)
            AssignPlayer();

        if (playerObject is null)
            return;

        if (_roomsList.Last().Door.transform.GetComponentInChildren<Door>().open && _roomsList.Count < MAX_ROOMS_IN_QUEUE)
            _roomsList.AddLast(CreateRoom(_roomsList.Last()));

        UpdateCurrentRoom();
    }
    
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        this.gamePaused = true;
        this.pauseMenuComponent.SetActive(true);
    }

    public void ContinueGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        this.gamePaused = false;
        this.pauseMenuComponent.SetActive(false);
    }
    
    public void ExitGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void UpdateCurrentRoom()
    {
        var currentRoom = _roomsList.Find(_currentRoom);
        var nextRoom = currentRoom.Next.Value;

        if (nextRoom != null)
        {
            bool changeRoom = false;

            Vector3 currentPlayerTile = new Vector3((playerObject.transform.position.x) / (panelSize * roomTileScale), 
                0.0f,
                (playerObject.transform.position.z) / (panelSize * roomTileScale));
            Vector2Int playerTilePos = new Vector2Int((int)Math.Round(currentPlayerTile.x), (int)Math.Round(currentPlayerTile.z));
            foreach (var tile in nextRoom.RoomTiles)
            {
                if (nextRoom.ClosedNextTilePosition.HasValue)
                    if (playerTilePos.x == tile.Position.x &&
                        playerTilePos.z == tile.Position.z &&
                        tile.Position.x != nextRoom.ClosedNextTilePosition.Value.x &&
                        tile.Position.z != nextRoom.ClosedNextTilePosition.Value.z)
                        changeRoom = true;
            }

            if (changeRoom)
            {
                _currentRoom = nextRoom;

                if (_currentRoom.ClosedDoor != null)
                    _currentRoom.ClosedDoor.GetComponentInChildren<ClosedDoor>().closing = true;

                if (_currentRoom.Door != null)
                    _currentRoom.Door.GetComponentInChildren<Door>().Open();
            }
        }

        _roomsList.First();

        if (_roomsList.Count >= 2)
        {
            var first = _roomsList.First;
            var second = first.Next.Value;
            if (second.ClosedDoor.GetComponentInChildren<ClosedDoor>().closed)
            {
                DestroyRoom(first.Value);
                _roomsList.RemoveFirst();
            }
        }
    }

    private void AssignPlayer()
    {
        playerObject = GameObject.FindWithTag("Player");
    }

    private Room CreateRoom(Room previousRoom)
    {
        var result = new Room();

        result.RoomTiles = new List<RoomTile>();

        HashSet<Vector2Int> chosenTiles = new HashSet<Vector2Int>();

        while (chosenTiles.Count < minTilesRoom)
        {
            Vector2Int origin = previousRoom == null ? new Vector2Int(0, 0) : previousRoom.NextTilePosition;
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

            bool canAddRectangle = true;
            if (previousRoom != null)
            {
                for (int x = origin.x; x <= topRight.x; x++)
                {
                    for (int z = origin.z; z <= topRight.z; z++)
                    {
                        Vector2Int newPoint = new Vector2Int(x, z);
                        if (!chosenTiles.Contains(newPoint))
                        {
                            foreach (var room in _roomsList)
                                if (room.RoomTiles.FindAll(x => x.Position.x == newPoint.x && x.Position.z == newPoint.z).Count > 0)
                                    canAddRectangle = false;
                        }
                    }
                }
            }
            if (canAddRectangle)
            {
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
        }

        foreach (var tile in chosenTiles)
        {
            var floor = Instantiate(floorPrefab);
            floor.transform.position = new Vector3(tile.x * panelSize * roomTileScale, 
                0.0f,
                tile.z * panelSize * roomTileScale);
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
                PositionsBehindWalls = new List<Vector2Int>(),
                Ceiling = ceiling
            };

            result.RoomTiles.Add(roomTile);
        }

        AddWalls(chosenTiles, result, previousRoom);
        // update the navMeshSurface
        navMeshSurface.BuildNavMesh();

        //SpawnEnemies(result);

        return result;
    }

    private void SpawnEnemies(Room room)
    {
        while (room.Enemies.Count < enemiesCount)
        {
            var enemy = RouletteWheelSelection();
            if (enemy)
            {

            }
        }
    }

    GameObject RouletteWheelSelection()
    {
        float chancesSum = 0.0f;
        foreach (var enemyType in enemyTypes)
            chancesSum += enemyType.chance;

        float chosenPoint = Random.Range(0.0f, chancesSum);
        
        float accumulated = 0.0f;

        foreach (var enemyType in enemyTypes)
        {
            accumulated += enemyType.chance;
            if (accumulated >= chosenPoint)
                return enemyType.prefab;
        }

        return null;
    }

    private bool Lee(Vector2Int nextDoorPosition)
    {
        Queue<Vector2Int>   testPositions  = new Queue<Vector2Int>();
        HashSet<Vector2Int> goodPositions  = new HashSet<Vector2Int>();
        HashSet<Vector2Int> triedPositions = new HashSet<Vector2Int>();

        testPositions.Enqueue(nextDoorPosition);
        triedPositions.Add(nextDoorPosition);

        while (testPositions.Count > 0 && goodPositions.Count < minTilesRoom)
        {
            bool goodTile = true;
            var currentPosition = testPositions.Dequeue();

            foreach (var gameRoom in _roomsList)
                if (gameRoom.RoomTiles.Any(x => x.Position.x == currentPosition.x && x.Position.z == currentPosition.z))
                    goodTile = false;

            if (goodTile)
            {
                goodPositions.Add(currentPosition);

                int[] dx = { 0, -1, 0, 1 };
                int[] dz = { -1, 0, 1, 0 };

                for (int i = 0; i < dx.Length; i++)
                {
                    Vector2Int nextPos = currentPosition + new Vector2Int(dx[i], dz[i]);
                    if (!triedPositions.Contains(nextPos))
                    {
                        testPositions.Enqueue(nextPos);
                        triedPositions.Add(nextPos);
                    }
                }
            }
        }

        return goodPositions.Count >= minTilesRoom;
    }

    private void AddWalls(HashSet<Vector2Int> chosenTiles, Room room, Room previousRoom)
    {
        List<GameObject> addedWalls = new List<GameObject>();
        List<Vector2Int> addedWallsPositions = new List<Vector2Int>();

        room.ClosedDoor = null;
        room.ClosedNextTilePosition = null;

        foreach (var tile in room.RoomTiles)
        {
            int[] dx = { 0, -1, 0, 1 };
            int[] dz = { -1, 0, 1, 0 };

            for (int i = 0; i < dx.Length; i++)
            {
                Vector2Int nextPos = tile.Position + new Vector2Int(dx[i], dz[i]);
                bool closedDoor = false;

                if (previousRoom == null)
                {
                    closedDoor = false;
                }
                else if (previousRoom.DoorPosition.x == nextPos.x &&
                        previousRoom.DoorPosition.z == nextPos.z &&
                        tile.Position.x == previousRoom.NextTilePosition.x &&
                        tile.Position.z == previousRoom.NextTilePosition.z)
                {
                    closedDoor = true;
                }

                if (!chosenTiles.Contains(nextPos))
                {
                    var prefab = closedDoor ? closedDoorPrefab : wallPrefab;
                    var wallObject = Instantiate(prefab);
                    wallObject.transform.position = tile.Floor.transform.position;
                    wallObject.transform.rotation = Quaternion.Euler(0.0f, i * 90.0f, 0.0f);

                    var actualWall = wallObject.transform.GetChild(0);
                    actualWall.transform.localScale = new Vector3(roomTileScale, 1.0f, roomTileScale);
                    actualWall.transform.localPosition = new Vector3(actualWall.transform.localPosition.x,
                        actualWall.transform.localPosition.y * roomTileScale,
                        actualWall.transform.localPosition.z * roomTileScale);

                    if (closedDoor)
                    {
                        room.ClosedDoor = wallObject;
                        room.ClosedNextTilePosition = tile.Position;
                        actualWall.transform.localPosition = actualWall.transform.localPosition + new Vector3(0.0f, panelSize * roomTileScale * 2.0f, 0.0f);
                        actualWall.GetComponent<ClosedDoor>().panelSize = panelSize;
                        actualWall.GetComponent<ClosedDoor>().tileScale = roomTileScale;
                    }

                    tile.Walls.Add(wallObject);
                    tile.PositionsBehindWalls.Add(nextPos);
                    addedWalls.Add(wallObject);
                    addedWallsPositions.Add(nextPos);
                }
            }
        }

        int index = Random.Range(0, addedWalls.Count);
        var chosenWall = addedWalls[index];
        var chosenWallPosition = addedWallsPositions[index];
        bool validDoor = Lee(chosenWallPosition);

        while (chosenWall == room.ClosedDoor || !validDoor)
        {
            index = Random.Range(0, addedWalls.Count); 
            chosenWall = addedWalls[index];
            chosenWallPosition = addedWallsPositions[index];
            validDoor = Lee(chosenWallPosition);
        }

        foreach (var tile in room.RoomTiles)
        {
            for (int i = 0; i < tile.Walls.Count; i++)
            {
                if (tile.Walls[i] == chosenWall)
                {
                    var doorObject = Instantiate(doorPrefab);
                    doorObject.transform.position = tile.Floor.transform.position;
                    doorObject.transform.rotation = chosenWall.transform.rotation;

                    var actualDoor = doorObject.transform.GetChild(0);
                    var actualWall = chosenWall.transform.GetChild(0);

                    actualDoor.transform.localScale = actualWall.transform.localScale;
                    actualDoor.transform.localPosition = actualWall.transform.localPosition;

                    actualDoor.GetComponent<Door>().roomTileScale = roomTileScale;
                    actualDoor.GetComponent<Door>().panelSize = panelSize;
                    if (previousRoom == null)
                        actualDoor.GetComponent<Door>().Open();
                    
                    Destroy(tile.Walls[i]);

                    tile.Walls[i] = doorObject;

                    room.Door = doorObject;
                    room.NextTilePosition = tile.PositionsBehindWalls[i];
                    room.DoorPosition = tile.Position;
                }
            }
        }
    }

    private void DestroyRoom(Room room)
    {
        foreach (var tile in room.RoomTiles)
        {
            Destroy(tile.Floor);
            Destroy(tile.Ceiling);
            foreach (var wall in tile.Walls)
                Destroy(wall);

            tile.Walls.Clear();
            tile.PositionsBehindWalls.Clear();
        }
        room.RoomTiles.Clear();
        room.Door = null;
        room.ClosedDoor = null;
    }
}
