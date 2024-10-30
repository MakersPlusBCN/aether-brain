using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SyncGesture : StateBase
{
    public StatesEnum nextState = StatesEnum.GestureCompleted;
    float durationGesture;

    public TMP_Text textFeedback;
    public TMP_Text textFeedbackElement;
    public TMP_Text feedbackSynxGesture_A;
    public TMP_Text feedbackSynxGesture_B;

    //List<string> gesturesPlayerA;
    bool finishGesture;
    bool isSync;


    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: SyncGesture");
        base.OnEnterState();


        GameManager.Instance.ResetGesturesListPulseraA();
        GameManager.Instance.ResetGesturesListPulseraB();


        feedbackSynxGesture_A.text = "Player 0 - Frequent Gesture = ";
        feedbackSynxGesture_B.text = "Player 1 - Frequent Gesture = ";

        isSync = false;
        

        //Show gesture (leds gestos)
        if (ArduinoManager.Instance.gameObject.activeSelf)
        {
            ArduinoManager.Instance.SendMessageToArduino("Q");
            ArduinoManager.Instance.SendMessageToArduino(GameManager.Instance.currentGestureToSync.type.ToString());
        }

        //Receive mqtt messages from both sensors
        if(GameManager.Instance.isMQTTActive)
            if(!GameManager.Instance.mqttManager.gameObject.activeSelf)
                GameManager.Instance.mqttManager.gameObject.SetActive(true);

        //Check for gesture duration and check sync level
        durationGesture = GameManager.Instance.currentGestureToSync.duration;
        finishGesture = false;

        textFeedback.text = "["  + GameManager.Instance.currentGestureIndex + "/" + (GameManager.Instance.currentPhaseElement.gestures.Count-1) +  "] Gesture to sync = " + GameManager.Instance.currentGestureToSync.type + " in " + durationGesture + "seconds";
        textFeedbackElement.text = "Current phase = " + GameManager.Instance.currentPhaseElement.elementName;

        UIElements[0].GetComponent<Image>().color = GameManager.Instance.currentPhaseElement.color;

    }

 

    public override void UpdateState()
    {
        base.UpdateState();
        if (Input.GetKeyDown(KeyCode.Space))
            Next();

        if (!GameManager.Instance.isMQTTActive)
        {
            //Debug Player A
            if (Input.GetKeyDown(KeyCode.D))
                GameManager.Instance.gesturesPlayerA.Add(GestureType.D);
            
            if (Input.GetKeyDown(KeyCode.H))
                GameManager.Instance.gesturesPlayerA.Add(GestureType.H);

            if (Input.GetKeyDown(KeyCode.V))
                GameManager.Instance.gesturesPlayerA.Add(GestureType.V);


            //Debug Player B
            if (Input.GetKeyDown(KeyCode.Alpha2))
                GameManager.Instance.gesturesPlayerB.Add(GestureType.D);

            if (Input.GetKeyDown(KeyCode.Alpha1))
                GameManager.Instance.gesturesPlayerB.Add(GestureType.H);

            if (Input.GetKeyDown(KeyCode.Alpha0))
                GameManager.Instance.gesturesPlayerB.Add(GestureType.V);

        }


        feedbackSynxGesture_A.text = "Player 0 - Frequent Gesture:\n" + GetMostFrequentGesture(GameManager.Instance.gesturesPlayerA);
        feedbackSynxGesture_B.text = "Player 1 - Frequent Gesture:\n" + GetMostFrequentGesture(GameManager.Instance.gesturesPlayerB);



        //If not sync happened during durationGesture --> Error -> tryagain! --> Jump to GestureFailed state!
        //If sync happened -> go to GestureCompleted state

        if (!finishGesture)
        {
            if (timeInState > durationGesture)
            {
                finishGesture = true;
                //GameManager.Instance.mqttManager.gameObject.SetActive(false);

       
                isSync = CheckSync();
 

                if (isSync)
                {
                    StartCoroutine(GoToNext());
                }
                else
                {
                    StateMachine.Instance.ChangeState(StatesEnum.GestureFailed);
                }
            }
        }

    }


    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: SyncGesture");
        StopAllCoroutines();
        base.OnExitState();
    }


    private IEnumerator GoToNext()
    {
        yield return new WaitForSeconds(2f);
        Next();

    }

    private void Next()
    {
        //Turn off gesture leds
        ArduinoManager.Instance.SendMessageToArduino("Q");

        StateMachine.Instance.ChangeState(nextState);
    }

    private bool CheckSync()
    {

        GestureType frequentGestureA = GetMostFrequentGesture(GameManager.Instance.gesturesPlayerA);
        GestureType frequentGestureB = GetMostFrequentGesture(GameManager.Instance.gesturesPlayerB);



        if (frequentGestureA == GameManager.Instance.currentGestureToSync.type && frequentGestureB == GameManager.Instance.currentGestureToSync.type)
        {
            Debug.Log("SYNC IS CORRECT");
            return true;
        }
        else
        {
            return false;
        }
    }


    private GestureType GetMostFrequentGesture(List<GestureType> _gestures)
    {

        GestureType _gesture = GestureType.None;

        if (_gestures.Count == 0)
            return _gesture;


        int[] auxMax = new int[3];
        auxMax[0] = 0;//Vertical
        auxMax[1] = 0;//Horizontal
        auxMax[2] = 0;//Diagonal

        for (int i = 0; i < _gestures.Count; i++)
        {
            if (_gestures[i] == GestureType.V)
            {
                auxMax[0]++;
            }
            else if (_gestures[i] == GestureType.H)
            {
                auxMax[1]++;
            }
            else
            {
                auxMax[2]++;
            }
        }

       /* Debug.Log("-----------------------");
        Debug.Log("TOTAL VERTICAL = " + auxMax[0]);
        Debug.Log("TOTAL HORIZONTAL = " + auxMax[1]);
        Debug.Log("TOTAL DIAGONAL = " + auxMax[2]);
        Debug.Log("-----------------------");*/


        int maxValue = auxMax.Max();
      
        if (maxValue == auxMax[0])
        {
            _gesture = GestureType.V;
        }
        else if (maxValue == auxMax[1])
        {
            _gesture = GestureType.H;
        }else if (maxValue == auxMax[2])
        {
            _gesture = GestureType.D;
        }

        return _gesture;
     
    }

}