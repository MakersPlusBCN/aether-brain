using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class GestureCompleted : StateBase
{
    public StatesEnum nextState = StatesEnum.SetNextGesture;


    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: GestureCompleted");
        base.OnEnterState();

        //sound feedback
        SoundsManager.Instance.FeedbackCompleteGesture();

        //Send arduino messages to complete effects
        //FEEDBACK leds
        ArduinoManager.Instance.SendMessageToArduino("+");

    }

 

    public override void UpdateState()
    {
        base.UpdateState();
        if (Input.GetKeyDown(KeyCode.Space))
            Next();


        if (timeInState < 3)
            Next();


    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: GestureCompleted");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        StateMachine.Instance.ChangeState(nextState);
    }

}