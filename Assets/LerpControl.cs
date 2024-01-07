using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpControl : MonoBehaviour
{
    public BoundsControl toolBounds;
    public ObjectManipulator toolManipulator;

    public void OnSliderUpdated(SliderEventData time)
    {
        toolBounds.RotateLerpTime = time.NewValue;
        toolBounds.TranslateLerpTime = time.NewValue;
        toolManipulator.RotateLerpTime = time.NewValue;
        toolManipulator.MoveLerpTime = time.NewValue;
    }
}
