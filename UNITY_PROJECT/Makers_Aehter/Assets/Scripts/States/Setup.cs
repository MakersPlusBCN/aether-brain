using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : StateBase
{
    public StatesEnum nextState = StatesEnum.CallToAction;

    private bool gointToNext;

    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: Setup");
        base.OnEnterState();

        gointToNext = false;

    }



    public override void UpdateState()
    {
        base.UpdateState();

        if (!gointToNext && GameManager.Instance.appIsReady)
            Next();
    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: Setup");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        gointToNext = true;
        StateMachine.Instance.ChangeState(nextState);
    }

}
