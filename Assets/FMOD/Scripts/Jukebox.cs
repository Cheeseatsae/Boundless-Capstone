using System;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    public static float MusicVolume = 1;
    public static float SFXVolume = 1;
    public float GameProgression;
    private FMOD.Studio.PARAMETER_ID progressionID;
    
    [FMODUnity.EventRef] public string soundtrackEvent = "Boundless Score";
    FMOD.Studio.EventInstance soundtrack;

    public static Jukebox instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameProgression = 0;
        
        soundtrack = FMODUnity.RuntimeManager.CreateInstance(soundtrackEvent);
        soundtrack.start();
        
        FMOD.Studio.EventDescription eventDescription;
        soundtrack.getDescription(out eventDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION eventParameterDescription;
        eventDescription.getParameterDescriptionByName("TrackPoint", out eventParameterDescription);
        progressionID = eventParameterDescription.id;

        soundtrack.start();
    }

    [ContextMenu("RestartSoundtrack")]
    private void RestartSoundtrack()
    {
        StopSoundtrack();
        soundtrack.start();
    }
    
    private void StopSoundtrack()
    {
        soundtrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void SetProgressionVariable(float i)
    {
        GameProgression = i;
    }
    
    public void IncrementProgressionVariable()
    {
        GameProgression += 1;
    }
    
    void OnDestroy()
    {
        StopSoundtrack();
        soundtrack.release();
    }
    
    // Update is called once per frame
    void Update()
    {
        soundtrack.setVolume(MusicVolume);
        soundtrack.setParameterByID(progressionID, GameProgression);
    }
}
