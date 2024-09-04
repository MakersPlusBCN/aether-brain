using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatesEnum
{
    Setup=0,
    CallToAction,
    OneUserReady,
    AllUsersReady,
    SetNextPhase,
    SetNextGesture,
    SyncGesture,
    GestureCompleted,
    GestureFailed,
    RestartPhase,
    EndExperience

}

public class StateBase : MonoBehaviour
{
    protected float timeInState;
    public List<GameObject> UIElements;

    public bool active = false;

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {

    }

    public virtual void OnEnterState()
    {
        StartCoroutine(ShowElements()); 
    }

    public virtual void OnExitState()
    {
        StartCoroutine(HideElements());
    }

    public virtual void Reset()
    {
    }

    public virtual void UpdateState()
    {
        timeInState += Time.deltaTime;
    }

    protected IEnumerator HideElements()
    {
        active = false;

        for (int k = 0; k < UIElements.Count; k++)
        {
            GameObject element = UIElements[k];
            element.SetActive(false);
        }

        yield return null;
    }

    protected IEnumerator ShowElements()
    {
        active = false;

        timeInState = 0;
        for (int k = 0; k < UIElements.Count; k++)
        {
            GameObject element = UIElements[k];
            element.SetActive(true);
        }

        yield return null;

        active = true;
    }
}

