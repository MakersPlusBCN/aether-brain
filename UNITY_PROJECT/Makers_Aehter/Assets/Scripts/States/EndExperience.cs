using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class EndExperience : StateBase
{
    public StatesEnum nextState = StatesEnum.CallToAction;


    public override void OnEnterState()
    {
        Debug.Log("ENTER STATE: EndExperience");
        base.OnEnterState();

        //Play base sound
        AudioClip clipEnd = SoundsManager.Instance.endExperience;
        StartCoroutine(SoundsManager.Instance.PlayBaseSoundForPhase(clipEnd));

        StartCoroutine(AllWhite());
    }

    private IEnumerator AllWhite()
    {
        yield return new WaitForSeconds(1f);
        //Turn leds all white!
        ArduinoManager.Instance.TurnOffSymbols();
        yield return new WaitForSeconds(5f);
       ArduinoManager.Instance.SendMessageToArduino("X");
    }


    public override void UpdateState()
    {
        base.UpdateState();

        //Detectar que las pulseras estan en la caja de nuevo
        if (timeInState > 20)
            Next();


    }

    public override void OnExitState()
    {
        Debug.Log("EXIT STATE: EndExperience");
        StopAllCoroutines();
        base.OnExitState();
    }

    private void Next()
    {
        StateMachine.Instance.ChangeState(nextState);
    }

}