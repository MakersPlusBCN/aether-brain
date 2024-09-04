using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class RestartPhase : StateBase
{
    public StatesEnum nextState = StatesEnum.SyncGesture;


    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: RestartPhase");
        base.OnEnterState();

        GameManager.Instance.ResetPhase();
    }

 

    public override void UpdateState()
    {
        base.UpdateState();
        if (Input.GetKeyDown(KeyCode.Space))
            Next();

        if (timeInState > 1)
            Next();

    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: RestartPhase");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        StateMachine.Instance.ChangeState(nextState);
    }

}