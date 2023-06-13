﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class HandAnimator : MonoBehaviour
{
    public float speed = 5.0f;
    public XRController controller = null;
    [SerializeField] bool leftHand;
    PhotonView pv;

    private Animator animator = null;

    private readonly List<Finger> gripfingers = new List<Finger>()
    {
        new Finger(FingerType.Middle),
        new Finger(FingerType.Ring),
        new Finger(FingerType.Pinky)
    };

    private readonly List<Finger> pointFingers = new List<Finger>
    {
        new Finger(FingerType.Index),
        new Finger(FingerType.Thumb)
    };

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

        Debug.Log(pv.IsMine);

        if (!pv.IsMine) return;

    }


    bool FoundHands;

    private void Update()
    {
        if (!pv.IsMine) return;
        // Store input

        if (!PlatformManager.instance && !FoundHands)
        {

            FoundHands = true;

            if (leftHand)
            {
                controller = PlatformManager.instance.vrRigParts[1].GetComponent<XRController>();
            }
            else
            {
                controller = PlatformManager.instance.vrRigParts[2].GetComponent<XRController>();
            }
        }

        if (!controller) return;

        CheckGrip();
        CheckPointer();

        // Smooth input values
        SmoothFinger(pointFingers);
        SmoothFinger(gripfingers);

        // Apply smoothed values
        AnimateFinger(pointFingers);
        AnimateFinger(gripfingers);
    }

    private void CheckGrip()
    {

        Debug.Log(controller);
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            SetFingerTargets(gripfingers, gripValue);
    }

    private void CheckPointer()
    {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float pointerValue))
            SetFingerTargets(pointFingers, pointerValue);
    }

    private void SetFingerTargets(List<Finger> fingers, float value)
    {
        foreach (Finger finger in fingers)
            finger.target = value;
    }

    private void SmoothFinger(List<Finger> fingers)
    {
        foreach(Finger finger in fingers)
        {
            float time = speed * Time.unscaledDeltaTime;
            finger.current = Mathf.MoveTowards(finger.current, finger.target, time);
        }
    }

    private void AnimateFinger(List<Finger> fingers)
    {
        foreach (Finger finger in fingers)
            AnimateFinger(finger.type.ToString(), finger.current);
    }

    private void AnimateFinger(string finger, float blend)
    {
        animator.SetFloat(finger, blend);
    }
}