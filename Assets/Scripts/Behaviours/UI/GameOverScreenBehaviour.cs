using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

/*
 * general UI Fading implemented
 * fetches all images and text within
 * and then tweens alpha values on their colors
 */
public class GameOverScreenBehaviour : BindingEntitasBehaviour, IGameOverScreenListener
{
    public Text messageTextField;

    private bool buttonPressed;

    private Image[] imagesUsed;
    private Text[] textFieldsUsed;
    private Tween tween;

    private Dictionary<Image, Color> originalImageColors;
    private Dictionary<Text, Color> originalTextColors;

    public void Awake()
    {
        //this is to get rid of screent in editor 
        //but still initialize correctly in runtime
        this.GetComponentInParent<Canvas>().enabled = true;

        originalImageColors = new Dictionary<Image, Color>();
        originalTextColors = new Dictionary<Text, Color>();

        imagesUsed = this.GetComponentsInChildren<Image>();
        textFieldsUsed = this.GetComponentsInChildren<Text>();

        foreach (var image in imagesUsed)
        {
            originalImageColors[image] = image.color;
        }
        foreach (var text in textFieldsUsed)
        {
            originalTextColors[text] = text.color;
        }

        OnAlphaUpdate(0);
    }

    override public void DeserializeEnitity(GameEntity entity)
    {
        base.DeserializeEnitity(entity);

        entity.AddGameOverScreen(this);
    }

    public void OnShow(string message)
    {
        buttonPressed = false;

        tween = DOVirtual.Float(0, 1, .5f, OnAlphaUpdate);
        messageTextField.text = message;
    }

    private void OnAlphaUpdate(float value)
    {
        bool enabled = value > 0;

        Color color;

        foreach(var image in imagesUsed)
        {
            color = originalImageColors[image];
            color.a *= value;
            image.color = color;
            image.enabled = enabled;
        }
        foreach (var text in textFieldsUsed)
        {
            color = originalTextColors[text];
            color.a *= value;
            text.color = color;
            text.enabled = enabled;
        }
    }

    public void OnHide()
    {
        tween.Kill();
        tween = DOVirtual.Float(1, 0, .25f, OnAlphaUpdate);
    }

    public void OnUpdate(float value)
    {
    }

    public void OnTryAgain()
    {
        if (!buttonPressed)
        {
            buttonPressed = true;
            entity.isGameStart = true;
            OnHide();
        }
    }
}

