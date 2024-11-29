using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public event Action OnInteractGenerator;
    public event Action<bool> OnBalckOut;
    public event Action<bool> OnOpenDoor;

    private void Awake()
    {
        GameMode.Events = this;
    }


    /// <summary>
    /// ��������� ����������
    /// </summary>
    public void InteractGenerator()
    {
        OnInteractGenerator?.Invoke();
    }

    /// <summary>
    /// ������/��������� ��������
    /// </summary>
    /// <param name="state"></param>
    public void ChangeStateBlackOut(bool state)
    {
        OnBalckOut?.Invoke(state);
    }

    /// <summary>
    /// ��������/�������� ���� ��������� ������
    /// </summary>
    /// <param name="state"></param>
    public void ChangeOpenDoor(bool state)
    {
        OnOpenDoor?.Invoke(state);
    }
}
