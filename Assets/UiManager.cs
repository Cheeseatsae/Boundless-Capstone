using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject settings;
    public Animator settingsAnim;
    public GameObject interaction;
    public ItemLogger textLogger;

    public GameObject youDied;
    // Start is called before the first frame update
    void Start()
    {
        Health.PlayerDeath += Died;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Died()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        youDied.SetActive(true);
    }
}
