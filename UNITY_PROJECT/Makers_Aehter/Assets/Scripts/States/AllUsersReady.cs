using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class AllUsersReady : StateBase
{
    public StatesEnum nextState = StatesEnum.SetNextPhase;


    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: AllUsersReady");
        base.OnEnterState();

        StartCoroutine(playAudioSequence());
  
    }

    private IEnumerator playAudioSequence()
    {
        SoundsManager.Instance.FeedbackPulseraActiva();
        yield return new WaitForSeconds(2f);
        SoundsManager.Instance.FeedbackAllUsersReady();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (timeInState > 3)
            Next();


    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: AllUsersReady");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        StateMachine.Instance.ChangeState(nextState);
    }

}