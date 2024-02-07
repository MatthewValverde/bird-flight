using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public bool showDebugUI;
    public BPPathController pathController;
    [Space]
    public IntRange randomClipsAmount;
    [Space]
    public AnimationClip takeOff;
    public AnimationClip fly;
    public AnimationClip land;
    public AnimationClip jumpForward;
    public AnimationClip nudge;
    public string jumpForwardTrigger = "JumpForward";
    public string nudgeTrigger = "NudgeItem";
    [Space]
    public BcAnimationSet[] animationSets;
    public GameObject birdAmature;
    public GameObject debugUIButton;
    Animator animator;
    float currentLength;
    float clipTimeCounter;
    int currentPose = 0;
    int currentRandomClip = 0;
    int amountOfClips = 0;
    int clipCounter = 0;
    bool poseReady = false;
    bool isFlying = false;
    Dictionary<string, AnimationClip> customClipDictonary;
    void Start()
    {
        animator = GetComponent<Animator>();
        customClipDictonary = new Dictionary<string, AnimationClip>();
        customClipDictonary.Add(jumpForwardTrigger, jumpForward);
        customClipDictonary.Add(nudgeTrigger, nudge);

        if (debugUIButton)
        {
            debugUIButton.SetActive(showDebugUI);
        }

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject");
            return;
        }

        if (pathController)
        {
            transform.position = pathController.sequence[0].path[0].transform.position;
            pathController.OnReadyToLand += OnReadyToLand;
            pathController.OnPathFinished += OnPathFinished;
        }

        isFlying = false;
        Pose();
    }

    // this is where the Flying Path is being started. You must have a pathController added for it to fly
    public void SetTrigger(string trigger)
    {
        if (customClipDictonary.TryGetValue(trigger, out AnimationClip clip))
        {
            isFlying = true;
            PlayClip(clip);
            StartCoroutine(SetTimeout(4f, () =>
            {
                isFlying = false;
                Pose();
            }));
            return;
        }

        if (!pathController) return;
        isFlying = true;
        clipTimeCounter = 0;

        PlayClip(fly);
        pathController.StartPath(trigger);
    }

    private void OnReadyToLand()
    {
    }

    private void OnPathFinished()
    {
        PlayClip(land);
        StartCoroutine(SetTimeout(2, () =>
        {
            isFlying = false;
            Pose();
        }));
    }

    void Pose()
    {
        clipCounter = 0;
        amountOfClips = Random(randomClipsAmount.min, randomClipsAmount.max);
        int tempRandomPose = Random(0, animationSets.Length);
        while (tempRandomPose == currentPose)
        {
            tempRandomPose = Random(0, animationSets.Length);
        }
        currentPose = tempRandomPose;
        poseReady = true;
        clipTimeCounter = 0;
        PlayClip(animationSets[currentPose].idle);
    }

    void Update()
    {
        if (!poseReady || isFlying) return;

        if (clipTimeCounter > currentLength)
        {
            clipTimeCounter = 0;
            currentLength = 0;
            int tempRandomClip = Random(0, animationSets[currentPose].clips.Length);
            while (tempRandomClip == currentRandomClip)
            {
                tempRandomClip = Random(0, animationSets[currentPose].clips.Length);
            }
            currentRandomClip = tempRandomClip;
            AnimationClip clip = animationSets[currentPose].clips[currentRandomClip];
            if (clipCounter == amountOfClips)
            {
                poseReady = false;
                Pose();
            }
            else
            {
                PlayClip(clip);
                clipCounter++;
            }
        }
        else
        {
            clipTimeCounter += Time.deltaTime;
        }
    }

    void PlayClip(AnimationClip clip)
    {
        //print(clip.name);
        currentLength = clip.length;
        animator.CrossFade(clip.name, 0.2f, -1, 0);
    }

    private int Random(int min, int max)
    {
        return UnityEngine.Random.Range(min, max); ;
    }

    IEnumerator SetTimeout(float delayInSeconds, System.Action action)
    {
        yield return new WaitForSeconds(delayInSeconds);
        action();
    }
}

[Serializable]
public class BcAnimationSet
{
    public string name;
    public AnimationClip idle;
    public AnimationClip takeOff;
    public AnimationClip[] clips;
}

[System.Serializable]
public struct IntRange
{
    public int min;
    public int max;

    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
}
