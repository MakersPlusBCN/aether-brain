using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class SetNextPhase : StateBase
{
    public StatesEnum nextState = StatesEnum.SetNextGesture;

    private bool phaseAvailable;
    private bool goingToNext;

    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: SetNextPhase");
        base.OnEnterState();

        GameManager.Instance.ResetGesturesListPulseraA();
        GameManager.Instance.ResetGesturesListPulseraB();


        phaseAvailable = GameManager.Instance.StartNextPhase();

         

        goingToNext = false;

    }

 

    public override void UpdateState()
    {
        base.UpdateState();
        if (!goingToNext && phaseAvailable)
        {
            goingToNext = true;
            Next();
        }

        if (!goingToNext && !phaseAvailable)
        {
            goingToNext = true;
            StateMachine.Instance.ChangeState(StatesEnum.EndExperience);
        }


    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: SetNextPhase");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        StateMachine.Instance.ChangeState(nextState);
    }

}