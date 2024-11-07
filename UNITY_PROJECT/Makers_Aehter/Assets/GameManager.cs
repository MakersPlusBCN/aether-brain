using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element
{
    Air,
    Fire,
    Water,
    Earth
}

public enum GestureType
{
    V,//0
    H,//1
    D,//2
    None
}


public class GameManager : Singleton<GameManager>
{
    private int currentPhaseIndex;
    public bool appIsReady;
    public BaseElement currentPhaseElement;
    public List<BaseElement> experiencePhases;

    public MQTTManager mqttManager;

    public Gesture currentGestureToSync;
    public int currentGestureIndex;

    public List<GestureType> gesturesPlayerA;
    public List<GestureType> gesturesPlayerB;

    public bool isMQTTActive;

    public float waitTimeResetExperience;
    public float waitTimeResetSensors; //mensaje reset pulseras, durante la experiencia
        

    private bool pulseraAPlaying;
    private bool pulseraBPlaying;


    private void Start()
    {

        ResetGame();

    }

    public void ResetGame()
    {
        currentPhaseIndex = -1;
        currentGestureIndex = -1;
        pulseraAPlaying = false;
        pulseraBPlaying = false;
    }


    public void StartPlayingPulseraA()
    {
        pulseraAPlaying = true;

        //Active lights for active user

        //update sound effect


        gesturesPlayerA = new List<GestureType>();
        gesturesPlayerA.Clear();

    }

    public void StartPlayingPulseraB()
    {
        pulseraBPlaying = true;

        gesturesPlayerB = new List<GestureType>();
        gesturesPlayerB.Clear();
    }

    public bool IsPulseraAPlaying()
    {
        return pulseraAPlaying;
    }

    public bool IsPulseraBPlaying()
    {
        return pulseraBPlaying;
    }


    public void PlayerAReady()
    {
        //Feedback luces

        //Feedback sonido
    }

    public void PlayerBReady()
    {
        //Feedback luces

        //Feedback sonido
    }


    public bool StartNextPhase()
    {

        bool phaseAvailable = false;
        currentPhaseIndex++;

        if(currentPhaseIndex < experiencePhases.Count)
        {
            currentPhaseElement = experiencePhases[currentPhaseIndex];

            currentGestureIndex = 0;
            currentGestureToSync = currentPhaseElement.gestures[currentGestureIndex];


            //Turn on symbol
            Debug.Log("SET LEDS --- RESET --> 0/"+ currentPhaseElement.messageArduino+"/"+ currentPhaseElement.gestures.Count.ToString());
            ArduinoManager.Instance.SendMessageToArduino("0");//Reset LEDs

            ArduinoManager.Instance.SendMessageToArduino(currentPhaseElement.messageArduino);//Leds simbolo caja
            ArduinoManager.Instance.SendMessageToArduino(currentPhaseElement.gestures.Count.ToString());//Segments feedback leds

            //Play base sound
            StartCoroutine( SoundsManager.Instance.PlayBaseSoundForPhase(currentPhaseElement.baseSound));
            phaseAvailable = true;
        }
        else
        {
            Debug.Log("End all phases!!! ");
            phaseAvailable = false;
        }
        return phaseAvailable;
    }

    public bool SetNextGesture()
    {
        currentGestureIndex++;
        bool setGesture = false;

        if (currentGestureIndex < currentPhaseElement.gestures.Count)
        {

            currentGestureToSync = currentPhaseElement.gestures[currentGestureIndex];
            setGesture = true;
        }
        else
        {
            Debug.Log("End all gestures from phase " + currentPhaseElement.elementName + "!!");
        }

        return setGesture;
    }

    public void FailureGesture()
    {
        if (currentGestureIndex > 1)
        {
            currentGestureIndex--;
        }
    }


    public void EndGame()
    {


    }

    public void ResetPhase()
    {
        currentPhaseElement = experiencePhases[currentPhaseIndex];


        //currentGestureIndex = 0;
        Debug.Log("--->>> currentGestureIndex ---> " + currentGestureIndex);
        currentGestureToSync = currentPhaseElement.gestures[currentGestureIndex];

        //Turn on symbol
        //ArduinoManager.Instance.SendMessageToArduino("0");//Reset LEDs

        //ArduinoManager.Instance.SendMessageToArduino(currentPhaseElement.messageArduino);//Leds simbolo caja
        //ArduinoManager.Instance.SendMessageToArduino(currentPhaseElement.gestures.Count.ToString());//Segments feedback leds

    }


    public void ResetGesturesListPulseraA()
    {
        gesturesPlayerA = new List<GestureType>();
        gesturesPlayerA.Clear();
    }


    public void ResetGesturesListPulseraB()
    {
        gesturesPlayerB = new List<GestureType>();
        gesturesPlayerB.Clear();
    }



}
