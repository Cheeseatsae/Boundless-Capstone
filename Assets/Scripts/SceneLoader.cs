using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene(1);
        Jukebox.instance.SetProgressionVariable(4);
        
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);


    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
        Jukebox.instance.RestartSoundtrack();
        Jukebox.instance.SetProgressionVariable(4);
    }
}
