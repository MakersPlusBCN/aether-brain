using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class GestureFailed : StateBase
{
    public StatesEnum nextState = StatesEnum.SyncGesture;


    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: GestureFailed");
        base.OnEnterState();

        GameManager.Instance.FailureGesture();


        //sound feedback
        SoundsManager.Instance.FeedbackFailGesture();

        //ArduinoManager.Instance.TurnOffSymbols();

        
        ArduinoManager.Instance.SendMessageToArduino("-");
        
        

        //Send arduino messages to complete effects
        if (GameManager.Instance.isMQTTActive)
            GameManager.Instance.mqttManager.ResetPublicMessage();

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
        Debug.Log("EXIT STATE: GestureFailed");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        StateMachine.Instance.ChangeState(nextState);
    }

}