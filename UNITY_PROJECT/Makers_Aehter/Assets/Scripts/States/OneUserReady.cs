using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class OneUserReady : StateBase
{
    public StatesEnum nextState = StatesEnum.AllUsersReady;

    public TMP_Text feedback;


    bool goingToNext;

    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: OneUserReady");
        base.OnEnterState();
        feedback.text = "";

        goingToNext = false;

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
                    Debug.Log("[OneUserReady] Start playing pulsera B!");
                    GameManager.Instance.StartPlayingPulseraB();
                }
            }

            if (!GameManager.Instance.IsPulseraAPlaying())
            {
                if (ArduinoManager.Instance.IsSensorAOn())
                {
                    Debug.Log("[OneUserReady] Start playing pulsera A!");
                    GameManager.Instance.StartPlayingPulseraA();
                }
            }
        }
        else
        {
            Debug.Log("[OneUserReady] Arduino is not active! ");
        }

        //Check both players playing 
        if(!goingToNext && ArduinoManager.Instance.gameObject.activeSelf && GameManager.Instance.IsPulseraBPlaying() && GameManager.Instance.IsPulseraAPlaying())
        {
            goingToNext = true;
            Next();
        }

        //Check that we have to reset to Call2Action
        if (ArduinoManager.Instance.gameObject.activeSelf)
        {
            if (!ArduinoManager.Instance.IsSensorAOn() && !ArduinoManager.Instance.IsSensorBOn())
                StartCoroutine(WaitBeforeResetExperience());
        }
    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: OneUserReady");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        //ArduinoManager.Instance.StoptReceivingData();
        StateMachine.Instance.ChangeState(nextState);
    }

    private IEnumerator WaitBeforeResetExperience()
    {

        Debug.Log("[OneUserReady] Pulseras detectadas en caja, waiting before going to screensaver...");
        yield return new WaitForSeconds(GameManager.Instance.waitTimeResetExperience);

        if (!goingToNext)
        {
            Debug.Log("[OneUserReady] Back to screensaver...");
            goingToNext = true;
            StateMachine.Instance.ChangeState(StatesEnum.CallToAction);

        }
           
    }

}