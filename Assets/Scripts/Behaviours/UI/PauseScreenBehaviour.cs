﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using DG.Tweening;


public class PauseScreenBehaviour : FadingUIBehaviour 
{
    public GameObject screen;

    private bool pauseAllowed = true;

    private ISignalEntityFactory signalFactory;

    public override void Awake()
    {
        screen.SetActive(true); //just to not show screen in editor mode
        RegisterFadingElements(screen);
        OnAlphaUpdate(scaleValue:0);

        signalFactory = new SignalEntityFactory();
    }
	
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            PauseOn();
		}
	}
	

    public void PauseOn()
    {
        if (pauseAllowed)
        {
            Contexts.sharedInstance.game.isPause = pauseAllowed;

            pauseAllowed = false;

            Time.timeScale = 0;
            DOTween.TogglePauseAll();

            OnShow(ignorePause: true);
        }
    }

    public void PauseOff()
    {
        if (!pauseAllowed)
        {
            Contexts.sharedInstance.game.isPause = pauseAllowed;

            pauseAllowed = true;

            Time.timeScale = 1;
            DOTween.TogglePauseAll();

            OnHide();
        }
    }
	
	public void Quit()
	{
		#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
		#else 
		Application.Quit();
		#endif
	}

    public void ResetRound()
    {
        PauseOff();
        signalFactory.Create().isRoundRestart = true;
    }

    public void ResetGame()
    {
        PauseOff();
        signalFactory.Create().isGameRestart = true;
    }
}
