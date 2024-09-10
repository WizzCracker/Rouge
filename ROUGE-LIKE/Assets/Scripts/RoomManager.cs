using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject bossRoomPrefab;
    [SerializeField] GameObject treasureRoomPrefab;
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;
    int roomWidth = 20;
    int roomHeight = 12;
    [SerializeField] int gridSizeX = 11;
    [SerializeField] int gridSizeY = 11;

    private List<GameObject> roomObjects = new List<GameObject>();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    private int[,] roomGrid;
        
    private int roomCount;

    private bool generationComplete = false;
    private bool treasureActive = false;
    private bool bossActive = false;

    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        if(roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            TryGenarateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenarateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenarateRoom(new Vector2Int(gridX, gridY + 1));
            TryGenarateRoom(new Vector2Int(gridX, gridY - 1));
        }
        else if(roomCount < minRooms || !treasureActive || !bossActive)
        {
            Debug.Log("Generation error. Trying again.");
            RegenerateRooms();
        }
        else if (!generationComplete)
        {
            Debug.Log($"Generation complete, {roomCount} rooms created.");
            generationComplete = true;
        }
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);
    }

    private bool TryGenarateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        if (x >= gridSizeX || y >= gridSizeY || x < 0 || y < 0)
            return false;

        if (roomGrid[x, y] != 0)
            return false;

        if (roomCount >= maxRooms)
            return false;

        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
            return false;

        if (CountAdjecentRooms(roomIndex) > 1)
            return false;

        GameObject newRoom;

        roomGrid[x, y] = 1;
        roomCount++;

        if (roomCount >= maxRooms / 3 && !treasureActive && Random.value < 0.5f)
        {
            newRoom = Instantiate(treasureRoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
            treasureActive = true;
            OpenDoors(newRoom, x, y, new Color(0.4f, 0.3f, 0));
        }
        else if(roomCount >= maxRooms / 2 && !bossActive && Random.value <= 0.20f + (roomCount / 18.75f))
        {
            newRoom = Instantiate(bossRoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
            bossActive = true;
            OpenDoors(newRoom, x, y, new Color(0.4f, 0f, 0f));
        }
        else
        {
            roomQueue.Enqueue(roomIndex);
            newRoom = Instantiate(Resources.Load<GameObject>("Prefabs/Room" + Random.Range(2,4).ToString()), GetPositionFromGridIndex(roomIndex), Quaternion.identity);
            OpenDoors(newRoom, x, y, Color.black);
        }
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        newRoom.name = $"Room-{roomCount}";
        roomObjects.Add(newRoom);

        return true;
    }

    void OpenDoors(GameObject room, int x, int y,Color color)
    {
        Room newRoomScript = room.GetComponent<Room>();

        //Adjecent rooms
        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));

        //Open doors in adjecent rooms
        if(x > 0 && roomGrid[x - 1, y] != 0)
        {
            //Room on left
            newRoomScript.OpenDoor(Vector2Int.left, color);
            leftRoomScript.OpenDoor(Vector2Int.right, color);
        }
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0)
        {
            //Room on right
            newRoomScript.OpenDoor(Vector2Int.right, color);
            rightRoomScript.OpenDoor(Vector2Int.left, color);
        }
        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            //Room on bottom
            newRoomScript.OpenDoor(Vector2Int.down, color);
            bottomRoomScript.OpenDoor(Vector2Int.up, color);
        }
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            //Room on top
            newRoomScript.OpenDoor(Vector2Int.up, color);
            topRoomScript.OpenDoor(Vector2Int.down, color);
        }

        Room GetRoomScriptAt(Vector2Int index)
        {
            GameObject roomObject = roomObjects.Find(roomObject => roomObject.GetComponent<Room>().RoomIndex == index);
            if(roomObject != null)
                return roomObject.GetComponent<Room>();
            return null;
        }
    }

    //Destroy rooms and generate new.
    private void RegenerateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;
        treasureActive = false;
        bossActive = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }
    private int CountAdjecentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && roomGrid[x - 1, y] != 0) count++; //Room on left
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0) count++; //Room on right
        if (y > 0 && roomGrid[x, y - 1] != 0) count++; //Room on bottom
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0) count++; //Room on top

        return count;
    }
    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2), roomHeight * (gridY - gridSizeY / 2));
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }
}
