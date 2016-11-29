using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public delegate void EventDeletegate(EventPayLoad data);
public class EventDispatcher
{

    private Dictionary<string, List<EventDeletegate>> _eventListers = new Dictionary<string, List<EventDeletegate>>();

    public void AddEventListner(string eventName , EventDeletegate callBack)
    {
        List<EventDeletegate> deletegates;
        if (_eventListers.TryGetValue(eventName,out deletegates))
        {
            deletegates.Remove(callBack);
            deletegates.Add(callBack);
        }
        else
        {
            deletegates = new List<EventDeletegate>();
            deletegates.Add(callBack);
            _eventListers.Add(eventName,deletegates);
        }
    }

    public void RemoveEventListner(string eventName, EventDeletegate callBack)
    {
        List<EventDeletegate> deletegates;
        if (_eventListers.TryGetValue(eventName, out deletegates))
        {
            deletegates.Remove(callBack);
        }
    }

    public void dispatchEvent(string eventName, EventPayLoad data)
    {
        List<EventDeletegate> deletegates;
        if (_eventListers.TryGetValue(eventName, out deletegates))
        {
            for (int idx = 0; idx < deletegates.Count; idx++)
            {
                deletegates[idx](data);
            }
        }
    }
}

public class EventPayLoad
{
    public Object data;
}
