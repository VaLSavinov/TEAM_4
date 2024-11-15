using UnityEngine;

public class RoomAccessControl : MonoBehaviour
{
    // Перечисление для карты доступа
    public enum AccessLevel
    {
        None,
        Red,
        Green,
        Blue
    }

    // Поле для выбора карты доступа
    [SerializeField]
    private AccessLevel requiredAccessLevel = AccessLevel.None;

    // Поле для статуса питания
    [SerializeField]
    private bool hasPower = true; // По умолчанию комнаты имеют питание

    // Свойства для доступа к полям
    public AccessLevel RequiredAccessLevel
    {
        get => requiredAccessLevel;
        set => requiredAccessLevel = value;
    }

    public bool HasPower
    {
        get => hasPower;
        set => hasPower = value;
    }

    // Метод для проверки доступа
    public bool CanAccess(AccessLevel playerAccessLevel)
    {
        // Проверка наличия питания и карты доступа
        return hasPower && playerAccessLevel >= requiredAccessLevel;
    }
}
