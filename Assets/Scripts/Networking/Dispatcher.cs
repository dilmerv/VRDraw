using System;
using System.Collections;
using System.Collections.Generic;
using DilmerGames.Core.Singletons;

public class Dispatcher : Singleton<Dispatcher>
{
    private static readonly Queue<Action> actions = new Queue<Action>();

    void Update()
    {
        lock(actions)
        {
            while(actions.Count > 0)
            {
                actions.Dequeue().Invoke();
            }
        }
    }

    public void Enqueue(IEnumerator action)
    {
        lock(actions)
        {
            actions.Enqueue(() => 
            {
                StartCoroutine(action);
            });
        }
    }

    public void Enqueue(Action action) => Enqueue(ActionWrapper(action));

    IEnumerator ActionWrapper(Action action)
    {
        action();
        yield return null;
    }
}