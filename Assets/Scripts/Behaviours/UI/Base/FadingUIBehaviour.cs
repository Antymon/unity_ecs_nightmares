using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

/*
 * general UI Fading implemented
 * fetches all images and text within
 * and then tweens alpha values on their colors
 * altering 'enabled' values as fit
 */

public class FadingUIBehaviour : BindingEntitasBehaviour
{
    private MaskableGraphic[] graphicsUsed;

    private Dictionary<MaskableGraphic, Color> originalImageColors;

    float value;

    public virtual void Awake()
    {
        RegisterFadingElements(this.gameObject);

        OnAlphaUpdate(0);
    }

    protected void RegisterFadingElements(GameObject root)
    {
        originalImageColors = new Dictionary<MaskableGraphic, Color>();

        graphicsUsed = root.GetComponentsInChildren<MaskableGraphic>();

        foreach (var graphic in graphicsUsed)
        {
            originalImageColors[graphic] = graphic.color;
        }
    }

    public virtual void OnShow(bool ignorePause = false)
    {
        float duration = .25f;

        if (!Contexts.sharedInstance.game.pause.paused || ignorePause)
        {
            DOTween.defaultTimeScaleIndependent = ignorePause;
            DOVirtual.Float(0, 1, duration, OnAlphaUpdate);
            DOTween.defaultTimeScaleIndependent = false;
        }
    }

    protected void OnAlphaUpdate(float scaleValue)
    {
        this.value = scaleValue;

        bool enabled = scaleValue > 0;

        Color color;

        foreach (var image in graphicsUsed)
        {
            color = originalImageColors[image];
            color.a *= scaleValue;
            image.color = color;
            image.enabled = enabled;
        }
    }

    public void OnHide()
    {
        float duration = .1f * value;
        DOVirtual.Float(value, 0, duration, OnAlphaUpdate);
    }
}
