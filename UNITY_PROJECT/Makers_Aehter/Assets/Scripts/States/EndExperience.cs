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

        //TODO :: Turn leds all white!
    }



    public override void UpdateState()
    {
        base.UpdateState();

        //TODO :: Detectar que las pulseras estan en la caja de nuevo
        if (timeInState > 5)
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