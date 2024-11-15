using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public enum ItemType
    {
        AccessCard,
        PortableBattery
    }

    public enum AccessCardColor
    {
        None, // ������������ ��� ��������, �� ���������� ������ �������
        Red,
        Green,
        Blue
    }

    [Header("Item Settings")]
    public ItemType itemType;
    public AccessCardColor cardColor = AccessCardColor.None;
}