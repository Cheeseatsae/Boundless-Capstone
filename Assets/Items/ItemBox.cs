using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : Interactable
{
    public GameObject pickup;
    public int cost;
    private bool activated;
    public bool activeChest;
    public GameObject floatingCost;
    public float minPlayerDistance;

    public GameObject lid;
    public Transform spawnPoint;
    
    private void Start()
    {
        active = true;
        TextMesh text = floatingCost.GetComponent<TextMesh>();
        text.text = "$" + cost;
    }

    public override void Interact(int currency)
    {
        if (activated) return;
        OpenBox(currency);
    }

    public void OpenBox(int currency)
    {
        if (currency < cost) return;
        
        activated = true;
        active = false; 
        PlayerInteraction.ChangeMoney(-cost);
        GameObject p = Instantiate(pickup, spawnPoint.position, Quaternion.identity);
        p.GetComponent<Pickup>().PickItem();
        floatingCost.SetActive(false);
        lid.GetComponent<Rigidbody>().isKinematic = false;
        lid.GetComponent<Rigidbody>().AddForce((Vector3.forward + Vector3.up) * 600);
        
        Destroy(this);
    }

    private void Update()
    {
        if (LevelManager.instance.player != null)
        {
            float playerDistance = Vector3.Distance(LevelManager.instance.player.transform.position, transform.position);
            if (playerDistance <= minPlayerDistance && !activated)
            {
                floatingCost.SetActive(true);
                Vector3 lookDirection = LevelManager.instance.player.transform.position - transform.position;;
                lookDirection.y = 0;
                floatingCost.transform.rotation = Quaternion.LookRotation(-lookDirection);
            }else floatingCost.SetActive(false);
        }

    }
}
