
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class GameOverScreenBehaviour : FadingUIBehaviour, IGameOverScreenListener
{
    public Text messageTextField;

    private bool buttonPressed;

    override public void Awake()
    {
        //this is to get rid of screent in editor 
        //but still initialize correctly in runtime
        this.GetComponentInParent<Canvas>().enabled = true;

        base.Awake();
    }

    override public void DeserializeEnitity(GameEntity entity)
    {
        base.DeserializeEnitity(entity);

        entity.AddGameOverScreen(this);
    }

    public void OnShow(string message)
    {
        buttonPressed = false;

        base.OnShow();

        messageTextField.text = message;
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

