using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rooms/RoomsList")]
public class RoomsList : ScriptableObject
{
    public List<RoomInfo> roomsInfo;
}
