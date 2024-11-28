public enum EEnemyState 
{
    Patrolling,
    Alerted,
    Chasing,
    WaitAlert,
    Searching,
    WaitChasing
}

public enum AccessCardColor
{
    None, // ������������ ��� ��������, �� ���������� ������ �������
    Red,
    Green,
    Blue
}

public enum ItemType 
{
    AccessCard,
    PortableBattery,
    Collectible,
    StunGun,
    Securcard
}

public enum CollectibleType
{
    None,
    Reports,
    AudioRecords  
}

public enum TextType 
{
    None,
    TMP,
    TextMesh
}