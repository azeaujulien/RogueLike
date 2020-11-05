using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class LevelBuilder : MonoBehaviour
{
    public GameObject player;
    
    public Room startRoomPrefab;
    public Room endRoomPrefab;

    public List<RoomsList> roomsType = new List<RoomsList>();
    public List<Room> roomsPrefabs = new List<Room>();
    public Vector2Int iterationRange = new Vector2Int(3, 10);

    private List<Doorway> _availableDoorways = new List<Doorway>();

    private StartRoom _startRoom;
    private EndRoom _endRoom;
    private List<Room> _placedRooms = new List<Room>();

    private NavMeshSurface _navMeshSurface;
    
    private LayerMask _roomLayerMask;

    private void Start()
    {
        player.SetActive(false);
        _roomLayerMask = LayerMask.GetMask("Room");
        _navMeshSurface = GetComponent<NavMeshSurface>();
        StartCoroutine(GenerateLevel());
    }

    private IEnumerator GenerateLevel()
    {
        WaitForSeconds startup = new WaitForSeconds(1);
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        yield return startup;
        
        // Place start room
        PlaceStartRoom();
        yield return interval;
        
        // Random Iteration
        int iterations = Random.Range(iterationRange.x, iterationRange.y);

        for (int i = 0; i < iterations; i++) {
            // Place random room from list
            PlaceRoom();
            yield return interval;
        }
        
        // Place end room
        PlaceEndRoom();
        yield return interval;
        
        // Bake NavMesh
        _navMeshSurface.BuildNavMesh();
        
        // Level generation finished
        Debug.Log("Level generation finished");
        
        // Set player active
        player.SetActive(true);
    }

    private void PlaceStartRoom()
    {
        Debug.Log("Place start room");
        
        // Instantiate start room
        _startRoom = Instantiate(startRoomPrefab, transform) as StartRoom;
        
        // Get doorways from current room and add them randomly to the list of available doorways
        AddDoorwaysToList(_startRoom, ref _availableDoorways);
        
        // Position room
        _startRoom.transform.position = Vector3.zero;
        _startRoom.transform.rotation = Quaternion.identity;
    }

    private void PlaceRoom()
    {
        Debug.Log("Place random room from list");
        
        // Instantiate room
        Room currentRoom = Instantiate(ChooseRoom(), transform);
        
        // Create doorway lists to loop over
        List<Doorway> allAvailableDoorways = new List<Doorway>(_availableDoorways.Where(doorway =>
        {
            if (currentRoom.type.soloRoom) {
                if (doorway.room.type.soloRoom) {
                    return (Doorway) null;
                }
                return doorway ;
            }

            return doorway;
        }).ToList());
        List<Doorway> currentRoomDoorways = new List<Doorway>();
        AddDoorwaysToList(currentRoom, ref currentRoomDoorways);
        
        // Get doorways from current room add thm randomly to the list of available doorways
        AddDoorwaysToList(currentRoom, ref _availableDoorways);

        bool roomPlaced = false;
        
        // Try all available doorways
        foreach (Doorway availableDoorway in allAvailableDoorways) {
            // Try all available doorways in current room
            foreach (Doorway currentDoorway in currentRoomDoorways) {
                // Position room
                PositionRoomAtDoorway(ref currentRoom, currentDoorway, availableDoorway);
                
                // Check room overlaps
                if (CheckRoomOverlap(currentRoom)) {
                    continue;
                }

                roomPlaced = true;
                
                // Add room to list
                _placedRooms.Add(currentRoom);
                
                // Remove occupied doorways
                currentDoorway.gameObject.SetActive(false);
                _availableDoorways.Remove(currentDoorway);
                
                availableDoorway.gameObject.SetActive(false);
                _availableDoorways.Remove(availableDoorway);
                
                // Exit loop
                break;
            }
            
            // Exit loop if room has been placed
            if (roomPlaced) {
                break;
            }
        }
        
        // Room couldn't be placed, restart generator
        if (!roomPlaced) {
            Destroy(currentRoom.gameObject);
            ResetLevelGenerator();
        }
    }

    private void PlaceEndRoom()
    {
        Debug.Log("Place end room");
        
        // Instantiate end room
        _endRoom = Instantiate(endRoomPrefab, transform) as EndRoom;
        
        // Create doorway lists to loop over
        List<Doorway> allAvailableDoorways = new List<Doorway>(_availableDoorways);
        Doorway doorway = _endRoom.doorways[Random.Range(0, _endRoom.doorways.Length)];
        
        bool roomPlaced = false;
        
        // Try all available doorways
        foreach (Doorway availableDoorway in allAvailableDoorways) {
            // Position room
            Room room = (Room) _endRoom;
            PositionRoomAtDoorway(ref room, doorway, availableDoorway);
                
            // Check room overlaps
            if (CheckRoomOverlap(_endRoom)) {
                continue;
            }

            roomPlaced = true;
            
            // Remove occupied doorways
            doorway.gameObject.SetActive(false);
            _availableDoorways.Remove(doorway);
                
            availableDoorway.gameObject.SetActive(false);
            _availableDoorways.Remove(availableDoorway);
                
            // Exit loop
            break;
        }
        
        // Room couldn't be placed, restart generator
        if (!roomPlaced) {
            ResetLevelGenerator();
        }
    }

    private void PositionRoomAtDoorway(ref Room room, Doorway roomDoorway, Doorway targetDoorway)
    {
        // Reset room position and rotation
        room.transform.position = Vector3.zero;
        room.transform.rotation = Quaternion.identity;
        
        // Rotate to match doorway orientation
        Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
        Vector3 roomDoorwayEuler = roomDoorway.transform.eulerAngles;
        float deltaAngle = Mathf.DeltaAngle(roomDoorwayEuler.y, targetDoorwayEuler.y);
        Quaternion currentRoomTargetRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        room.transform.rotation = currentRoomTargetRotation * Quaternion.Euler(0, 180f, 0);
        
        // Position room
        Vector3 roomPositionOffset = roomDoorway.transform.position - room.transform.position;
        room.transform.position = targetDoorway.transform.position - roomPositionOffset;
    }
    
    private bool CheckRoomOverlap(Room room)
    {
        Bounds bounds = room.RoomBounds;
        bounds.Expand(-0.1f);
        bounds.center = room.transform.position;

        Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.size / 2, room.transform.rotation, _roomLayerMask);
        if (colliders.Length > 0) {
            // Ignore collisions with the current room
            foreach (Collider c in colliders) {
                Debug.Log("C parent : " + c.transform.parent.name);
                Debug.Log("Room : " + room.gameObject.name);
                if (!c.transform.parent.gameObject.Equals(room.gameObject)) {
                    Debug.LogError("Overlap detected");
                    return true;
                }
            }
        }

        return false;
    }
    
    public void ResetLevelGenerator()
    {
        Debug.Log("Reset level generator");
        StopCoroutine(GenerateLevel());
        
        // Delete all rooms
        if (_startRoom) {
            Destroy(_startRoom.gameObject);
        }

        foreach (Room room in _placedRooms) {
            Destroy(room.gameObject);
        }
        
        if (_endRoom) {
            Destroy(_endRoom.gameObject);
        }
        
        // Clear lists
        _placedRooms.Clear();
        _availableDoorways.Clear();
        
        // Reset coroutine
        StartCoroutine(GenerateLevel());
    }

    private void AddDoorwaysToList(Room room, ref List<Doorway> list)
    {
        foreach (Doorway doorway in room.doorways) {
            int r = Random.Range(0, list.Count);
            list.Insert(r, doorway);
        }
    }

    private Room ChooseRoom()
    {
        float total = 0;
        roomsType.ForEach(type => total += type.probability);
        
        float randomNumber = Random.Range(0, total);

        List<RoomsList> allRooms = roomsType;
        allRooms = roomsType.OrderBy(type => -type.probability).ToList();
        RoomsList usedRooms = allRooms.First();

        foreach (RoomsList room in allRooms) {
            if (randomNumber <= room.probability) {
                usedRooms = room;
                break;
            }

            randomNumber -= room.probability;
        }

        return usedRooms.rooms[Random.Range(0, usedRooms.rooms.Count)];
    }
}

[System.Serializable]
public class RoomsList
{
    public RoomType type;
    [Range(1, 100)] public float probability;
    public List<Room> rooms = new List<Room>();
}