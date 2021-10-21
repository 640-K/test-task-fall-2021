using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventInvoker : MonoBehaviour
{
    public UnityEvent eventToInvoke;

    void Invoke() => eventToInvoke.Invoke();
}
