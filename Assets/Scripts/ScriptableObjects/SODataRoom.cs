using UnityEngine;


[CreateAssetMenu(fileName = "Rooms",
                 menuName = "Rooms / Data room", order = 55)]

public class SODataRoom : ScriptableObject
{
    public GameObject prefab;
    public GameObject checkedBox;
    public int roomSize;
    public bool isEndRoom;
}

