using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class CallToAction : StateBase
{
    public StatesEnum nextState = StatesEnum.OneUserReady;


    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: CallToAction");
        base.OnEnterState();

        GameManager.Instance.ResetGame();

        //Active Receive info Arduino - sensor pulseras -
        /*if (ArduinoManager.Instance.gameObject.activeSelf)
        {
            ArduinoManager.Instance.SetupArduinoManager();
            ArduinoManager.Instance.StartReceivingData();
        }*/
      


        GameManager.Instance.mqttManager.gameObject.SetActive(false);

        //Set light blinking (caja zona pulseras)

        //Turn off box ligths (symbol, instructions and gestures)
        //if (ArduinoManager.Instance.gameObject.activeSelf)
        //{
            ArduinoManager.Instance.TurnOffSymbols();

        //}

        //Play sound for state
        StartCoroutine(SoundsManager.Instance.PlayBaseWelcome());
    }

 

    public override void UpdateState()
    {
        base.UpdateState();

        if (Input.GetKeyDown(KeyCode.Space))
            Next();


        //Check Arduino serial messages
        if (ArduinoManager.Instance.gameObject.activeSelf)
        {
            if (ArduinoManager.Instance.IsSensorAOn() || ArduinoManager.Instance.IsSensorBOn())
            {
                ActionsBeforeExit();
            }
        }



        //----- DEBUG WRITE MESSAGES ----
           //Water
           if (Input.GetKeyDown(KeyCode.W))
               ArduinoManager.Instance.SendMessageToArduino("W");

           //Earth
           if (Input.GetKeyDown(KeyCode.E))
               ArduinoManager.Instance.SendMessageToArduino("E");

           //Fire
           if (Input.GetKeyDown(KeyCode.F))
               ArduinoManager.Instance.SendMessageToArduino("F");

           //Air
           if (Input.GetKeyDown(KeyCode.A))
               ArduinoManager.Instance.SendMessageToArduino("A");


           //OFF
           if (Input.GetKeyDown(KeyCode.O))
               ArduinoManager.Instance.TurnOffSymbols();

        if (Input.GetKeyDown(KeyCode.X))
        {
            //ArduinoManager.Instance.TurnOffSymbols();
            ArduinoManager.Instance.SendMessageToArduino("X");
        }
         


    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: CallToAction");
        StopAllCoroutines();
        base.OnExitState();
    }


    private void ActionsBeforeExit()
    {
        if (ArduinoManager.Instance.gameObject.activeSelf)
        {

            if (ArduinoManager.Instance.IsSensorAOn())
                GameManager.Instance.StartPlayingPulseraA();


            if (ArduinoManager.Instance.IsSensorBOn())
                GameManager.Instance.StartPlayingPulseraB();

        }

        Next();

    }

    private void Next()
    {
        StateMachine.Instance.ChangeState(nextState);
    }

}