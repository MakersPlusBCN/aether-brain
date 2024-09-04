using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class SetNextGesture : StateBase
{
    public StatesEnum nextState = StatesEnum.SyncGesture;

    private bool gestureIsSet;

    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: SetNextGesture");
        base.OnEnterState();

        gestureIsSet = GameManager.Instance.SetNextGesture();
       
    }

 

    public override void UpdateState()
    {
        base.UpdateState();
        if (timeInState > 1)
            CheckNext();




    }

    private void CheckNext()
    {
        if (gestureIsSet)
        {
            Next();
        }
        else
        {
            StateMachine.Instance.ChangeState(StatesEnum.SetNextPhase);
        }
    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: SetNextGesture");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        StateMachine.Instance.ChangeState(nextState);
    }

}