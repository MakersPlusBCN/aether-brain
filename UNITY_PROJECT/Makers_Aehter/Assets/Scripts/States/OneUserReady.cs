using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class OneUserReady : StateBase
{
    public StatesEnum nextState = StatesEnum.OneUserReady;

    public TMP_Text feedback;


    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: OneUserReady");
        base.OnEnterState();
        feedback.text = "";

        if (GameManager.Instance.IsPulseraBPlaying())
        {
            feedback.text += " Player B is ready";
            GameManager.Instance.PlayerBReady();
        }


        if (GameManager.Instance.IsPulseraAPlaying())
        {
            feedback.text += " Player A is ready";
            GameManager.Instance.PlayerAReady();
        }


        //Active Receive info Arduino - sensor pulseras -
        /*if (ArduinoManager.Instance.gameObject.activeSelf)
        {
            ArduinoManager.Instance.SetupArduinoManager();
            ArduinoManager.Instance.StartReceivingData();
        }*/


        //Set light blinking (caja zona pulseras) para la pulsera que sigue en la caja
        //turn off light (caja zona pulseras) zona de la pulsera que ya esta fuera de la caja (en uso)


        //Keep off box ligths (symbol, instructions and gestures) para zona pulsera que sigue en la caja

        //Turn on lights luces zona pulsera 

        //Play sound for state
        SoundsManager.Instance.FeedbackPulseraActiva();


    }



    public override void UpdateState()
    {
        base.UpdateState();
        if (Input.GetKeyDown(KeyCode.Space))
            Next();


        if (ArduinoManager.Instance.gameObject.activeSelf)
        {
            if (!GameManager.Instance.IsPulseraBPlaying())
            {
                if (ArduinoManager.Instance.IsSensorBOn())
                {
                    ActionsBeforeExit();
                }
            }

            if (!GameManager.Instance.IsPulseraAPlaying())
            {
                if (ArduinoManager.Instance.IsSensorAOn())
                {
                    ActionsBeforeExit();
                }
            }
        }

    }

    private void ActionsBeforeExit()
    {

        if (ArduinoManager.Instance.gameObject.activeSelf)
        {
            ArduinoManager.Instance.StoptReceivingData();


            if (!GameManager.Instance.IsPulseraAPlaying() && ArduinoManager.Instance.IsSensorAOn())
                GameManager.Instance.StartPlayingPulseraA();


            if (!GameManager.Instance.IsPulseraBPlaying() && ArduinoManager.Instance.IsSensorBOn())
                GameManager.Instance.StartPlayingPulseraB();

        }

        Next();

    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: OneUserReady");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        StateMachine.Instance.ChangeState(nextState);
    }

}