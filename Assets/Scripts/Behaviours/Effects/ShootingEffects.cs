using System;
using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;


public class ShootingEffects : MonoBehaviour
{
    public Light faceLight;
    [HideInInspector]
    public int effectsLastTicks; //how long will the effects last

    int effectsEnabledTicks; //0 - effects disabled, + enabled

    ParticleSystem gunParticles;                    // Reference to the particle system.
    LineRenderer gunLine;                           // Reference to the line renderer.
    AudioSource gunAudio;                           // Reference to the audio source.
    Light gunLight;                                 // Reference to the light component.
		
    void Awake ()
    {
        // Set up the references.
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }

    void Update()
    {
        effectsEnabledTicks = Math.Max(0, effectsEnabledTicks - 1);
        if(effectsEnabledTicks == 0)
        {
            DisableEffects();
        }
    }

    public void DisableEffects ()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
		faceLight.enabled = false;
        gunLight.enabled = false;
    }

    public void Shoot (Vector3 endRay)
    {
        effectsEnabledTicks = effectsLastTicks;

        // Play the gun shot audioclip.
        gunAudio.Play ();

        // Enable the lights.
        gunLight.enabled = true;
		faceLight.enabled = true;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop ();
        gunParticles.Play ();

        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);
        gunLine.SetPosition(1, endRay);
    }
}
