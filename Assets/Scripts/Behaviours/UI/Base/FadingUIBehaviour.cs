using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

/*
 * general UI Fading implemented
 * fetches all images and text within
 * and then tweens alpha values on their colors
 * altering 'enabled' values as fit
 * 
 * ToDo:
 * more proper would be to use tweens
 * and use selective pausing whenever neeeded
 * or at least wrap tween params within an instance
 * and force interpolation on update
 */
public class FadingUIBehaviour : BindingEntitasBehaviour
{
    private Image[] imagesUsed;
    private Text[] textFieldsUsed;

    private Dictionary<Image, Color> originalImageColors;
    private Dictionary<Text, Color> originalTextColors;

    bool tween, ignorePause;
    float from, to, time, duration, value;

    public virtual void Awake()
    {
        RegisterFadingElements(this.gameObject);

        OnAlphaUpdate(0);
    }

    protected void RegisterFadingElements(GameObject root)
    {
        originalImageColors = new Dictionary<Image, Color>();
        originalTextColors = new Dictionary<Text, Color>();

        imagesUsed = root.GetComponentsInChildren<Image>();
        textFieldsUsed = root.GetComponentsInChildren<Text>();

        foreach (var image in imagesUsed)
        {
            originalImageColors[image] = image.color;
        }
        foreach (var text in textFieldsUsed)
        {
            originalTextColors[text] = text.color;
        }
    }

    public virtual void OnShow(bool ignorePause = false)
    {
        this.ignorePause = ignorePause;

        tween = true;
        from = 0;
        to = 1;
        time = 0;
        duration = .25f;
    }

    public virtual void Update()
    {
        if (tween && (!Contexts.sharedInstance.game.pause.paused || ignorePause))
        {
            if (time<duration)
            {
                time += 1f / Application.targetFrameRate;
                value = Mathf.Lerp(from, to, Mathf.Clamp01(time / duration));
                OnAlphaUpdate(value);
            }
            else
            {
                tween = false;
            }
        }
    }

    protected void OnAlphaUpdate(float scaleValue)
    {
        bool enabled = scaleValue > 0;

        Color color;

        foreach (var image in imagesUsed)
        {
            color = originalImageColors[image];
            color.a *= scaleValue;
            image.color = color;
            image.enabled = enabled;
        }
        foreach (var text in textFieldsUsed)
        {
            color = originalTextColors[text];
            color.a *= scaleValue;
            text.color = color;
            text.enabled = enabled;
        }
    }

    public void OnHide(bool ignorePause = false)
    {
        this.ignorePause = ignorePause;

        tween = true;
        from = value;
        to = 0;
        time = 0;
        duration = .1f * value;
    }


}
