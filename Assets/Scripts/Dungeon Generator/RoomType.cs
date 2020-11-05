using UnityEngine;

[CreateAssetMenu(fileName = "New Type", menuName = "DungeonGenerator/RoomType", order = 0)]
public class RoomType : ScriptableObject
{
    public string typeName;
    public bool soloRoom;
}