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



    public void PlayBaseWelcome()
    {
        audioSourceBase.Stop();

        audioSourceBase.clip = baseWelcome;
        audioSourceBase.loop = true;

        audioSourceBase.Play();

        audioSourceBase.volume = 1f;

    }

    public void FeedbackPulseraActiva()
    {
        audioSourceBase.volume = 0.3f;

        audioSourceFeedback.loop = false;
        audioSourceFeedback.volume = 1f;
        audioSourceFeedback.clip = feedbackPositivo;
        audioSourceFeedback.Play();
    }


    public void FeedbackAllUsersReady()
    {
        //audioSourceFeedback.Stop();

        audioSourceBase.Stop();
        audioSourceBase.volume = 1f;
    }

    public void FeedbackCompleteGesture()
    {
        audioSourceFeedback.volume = 1f;
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

    public void PlayBaseSoundForPhase(AudioClip _aC)
    {
        audioSourceFeedback.Stop();

        audioSourceBase.volume = 1f;
        audioSourceBase.loop = true;

        audioSourceBase.clip = _aC;
        audioSourceBase.Play();

    }

    public void PlayEndExperienceSound()
    {
        audioSourceFeedback.Stop();
        audioSourceFeedback.loop = false;

        audioSourceFeedback.clip = endExperience;

        audioSourceFeedback.Play();
    }

}
