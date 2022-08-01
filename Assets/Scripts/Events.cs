using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Events
{
    public static UnityEvent<int> onHealthChanged = new UnityEvent<int>();

    public static UnityEvent<float> onPurpleChanged = new UnityEvent<float>();

}
