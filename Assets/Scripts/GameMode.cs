using System;

public static class GameMode
{
    private static FirstPersonLook _playerCamera;
    private static PersonHand _personHand;
    private static PlayerUI _playerUI;
    private static EnemyManager _EnemyManager;


    public static event Action OnInteractGenerator;
    public static event Action<bool> OnBalckOut;
    public static event Action<bool> OnOpenDoor;

    public static PersonHand PersonHand
    {
        get { return _personHand; }
        set { _personHand = value; }
    }

    public static FirstPersonLook FirstPersonLook
    {
        get { return _playerCamera; }
        set { _playerCamera = value; }
    }

    public static PlayerUI PlayerUI
    {
        get { return _playerUI; }
        set { _playerUI = value; }
    }

    public static EnemyManager EnemyManager
    {
        get { return _EnemyManager; }
        set { _EnemyManager = value; }
    }

    /// <summary>
    /// Активация генератора
    /// </summary>
    public static void InteractGenerator() 
    {
        OnInteractGenerator?.Invoke();
    }

    /// <summary>
    /// Запуск/остановка блекаута
    /// </summary>
    /// <param name="state"></param>
    public static void ChangeStateBlackOut(bool state) 
    {
        OnBalckOut?.Invoke(state);
    }

    /// <summary>
    /// Открытие/закрытие всех доступных дверей
    /// </summary>
    /// <param name="state"></param>
    public static void ChangeOpenDoor(bool state)
    {
        OnOpenDoor?.Invoke(state);
    }

}
