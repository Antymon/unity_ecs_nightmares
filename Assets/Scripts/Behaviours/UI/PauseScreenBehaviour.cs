using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
using DG.Tweening;
#endif

public class PauseScreenBehaviour : BindingEntitasBehaviour {
	
	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;

    public GameObject screen;
	
	void Start()
	{
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            
			TogglePause();
		}
	}
	
	public void TogglePause()
	{
        screen.SetActive(!screen.activeSelf);

		Time.timeScale = Time.timeScale == 0 ? 1 : 0;

        DOTween.TogglePauseAll();

		Lowpass();
		
	}
	
	private void Lowpass()
	{
		if (Time.timeScale == 0)
		{
			paused.TransitionTo(.01f);
		}
		
		else
			
		{
			unpaused.TransitionTo(.01f);
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
        TogglePause();
        CreateEntity().isRoundRestart = true;
    }

    public void ResetGame()
    {
        TogglePause();
        CreateEntity().isGameRestart = true;
    }

    private GameEntity CreateEntity()
    {
        var messageEntity = Contexts.sharedInstance.game.CreateEntity();
        messageEntity.isMarkedToPostponedDestroy = true;
        return messageEntity;
    }
}
