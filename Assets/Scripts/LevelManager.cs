using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static AIManager aiManager;
    
    public GameObject playerBulletPrefab;
    public GameObject playerPrefab;
    
    public List<GameObject> spawnPoints = new List<GameObject>();

    public UiManager uiManager;
    public GameObject player;
    public bool paused = false;

    public GameObject agni;
    public ActivatePortal portal;
    private void Awake()
    {
        instance = this;
        aiManager = GetComponent<AIManager>();
        uiManager = GetComponent<UiManager>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        portal.PortalActivate += ActivateBoss;
        GameObject spawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        player = Instantiate(playerPrefab, spawn.transform.position, spawn.transform.rotation);

        player.GetComponent<Ability1>().bulletPref = SetupPlayer();
        aiManager.player = player;
    }

    public GameObject SetupPlayer()
    {
        Vector3 spawnPos = new Vector3(0,-15000,0);
        GameObject obj = Instantiate(playerBulletPrefab, spawnPos, Quaternion.identity);
        
        return obj;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }
    
    public void Pause()
    {
        if (paused == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //uiManager.settingsAnim.SetBool("Open", true);
            uiManager.settings.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
            
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //uiManager.settingsAnim.SetBool("Open", false);
            uiManager.settings.SetActive(false);
            Time.timeScale = 1f;
            paused = false;
        }
        
    }

    public void LogText(string text)
    {
        uiManager.textLogger.NewLog(text);
    }

    public void ActivateBoss()
    {
        StartCoroutine(Boss());
    }

    public IEnumerator Boss()
    {
        yield return new WaitForSeconds(3);
        agni.SetActive(true);
    }
}
