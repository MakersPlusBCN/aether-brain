using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : Singleton<SoundsManager>
{

    public AudioClip feedbackPositivo;
    public AudioClip feedbackNegativo;

    public AudioClip baseWelcome;
    public AudioClip endExperience;


    public AudioSource audioSourceBase;
    public AudioSource audioSourceFeedback;



    public IEnumerator PlayBaseWelcome()
    {
        //audioSourceBase.Stop();
        Debug.Log("PLAYING welcome sound!!! ");
        float fadeTime = 0.4f;
        StartCoroutine(FadeOut(audioSourceBase, fadeTime));


        //audioSourceBase.Play();

        yield return new WaitForSeconds(1f);

        audioSourceBase.clip = baseWelcome;
        audioSourceBase.loop = true;

        StartCoroutine(FadeIn(audioSourceBase, 0.4f));

        //audioSourceBase.volume = 1f;

    }

    public void FeedbackPulseraActiva()
    {
        //audioSourceBase.volume = 0.3f;

        audioSourceFeedback.loop = false;
        //audioSourceFeedback.volume = 1f;
        audioSourceFeedback.clip = feedbackPositivo;
        audioSourceFeedback.Play();
    }


    public void FeedbackAllUsersReady()
    {
        //audioSourceFeedback.Stop();

        audioSourceBase.Stop();
        //audioSourceBase.volume = 1f;
    }

    public void FeedbackCompleteGesture()
    {
        //saudioSourceFeedback.volume = 1f;
        audioSourceFeedback.Stop();
        audioSourceFeedback.loop = false;

        audioSourceFeedback.clip = feedbackPositivo;

        audioSourceFeedback.Play();

    }

    public void FeedbackFailGesture()
    {
        audioSourceFeedback.Stop();
        audioSourceFeedback.loop = false;

        audioSourceFeedback.clip = feedbackNegativo;

        audioSourceFeedback.Play();
    }

    public IEnumerator PlayBaseSoundForPhase(AudioClip _aC)
    {
        if(_aC != audioSourceBase.clip)
        {
            Debug.Log("PLAYING SOUND FOR CURRENT PHASE!!! ");
            float fadeTime = 0.4f;
            StartCoroutine(FadeOut(audioSourceBase, fadeTime));
            //audioSourceFeedback.Stop();

            //audioSourceBase.volume = 1f;
           

            yield return new WaitForSeconds(1f);
            audioSourceBase.loop = true;
            audioSourceBase.clip = _aC;

            StartCoroutine(FadeIn(audioSourceBase, 0.4f));
            //audioSourceBase.Play();
        }
    }

    /*public void PlayEndExperienceSound()
    {
        audioSourceBase.Stop();
        audioSourceBase.loop = false;

        audioSourceBase.clip = endExperience;

        StartCoroutine(FadeIn(audioSourceFeedback, 0.5f));
        //audioSourceFeedback.Play();
    }*/

    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("---->> start fade out");
        float startVolume = 1f;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        //audioSource.volume = startVolume;
        Debug.Log("---->> end fade out");
    }

    private IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        Debug.Log("---->> start fade in");
        float startVolume = 0.1f;
        audioSource.volume = startVolume;
        audioSource.Play();

        while (audioSource.volume < 1f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        Debug.Log("---->> end fade in");
        //audioSource.volume = 1f;
    }

}
