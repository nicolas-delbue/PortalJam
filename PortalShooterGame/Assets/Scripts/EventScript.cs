using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{
    public static EventScript current;
    public void Awake()
    {
        if (current != null)
        {
            Debug.LogWarning("Two instances of EvenControllerHub in Scene");
        }
        current = this;
    }

    public event Action<int> onUpdateAmmo;
    public void UpdateAmmo(int amount)
    {
        if (onUpdateAmmo != null)
        {
            onUpdateAmmo(amount);
        }
    }
    public event Action<int> onUpdateMaxAmmo;
    public void UpdateMaxAmmo(int amount)
    {
        if (onUpdateMaxAmmo != null)
        {
            onUpdateMaxAmmo(amount);
        }
    }
    public event Action<int> onUpdateScore;
    public void UpdateScore(int amount)
    {
        if (onUpdateScore != null)
        {
            onUpdateScore(amount);
        }
    }


    //Base Event
    public event Action onBaseEvent;
    /// <summary>
    /// Basic event
    /// </summary>
    public void BaseEvent()
    {
        if (onBaseEvent != null)
        {
            onBaseEvent();
        }
    }

    //This is an event where you pass paramaters too actions.
    public event Action<int> onIdEvent;
    /// <summary>
    /// Should do if(id == this.id) where the event is being listened too (Reminder for wiki)
    /// </summary>
    /// <param name="id"></param>
    public void IdEvent(int id)
    {
        if (onIdEvent != null)
        {
            onIdEvent(id);
        }
    }

    //Event that sends info on a variable
    /// <summary>
    /// Event that you check game object with.
    /// </summary>
    public event Action<GameObject> OnTestRequestObj;
    /// <summary>
    /// Event to listen and call to send the game object as event.
    /// </summary>
    /// <param name="returnEvent"></param>
    public void TestRequestObj(GameObject returnEvent)
    {
        if (OnTestRequestObj != null)
        {
            OnTestRequestObj(returnEvent);
        }
    }
}