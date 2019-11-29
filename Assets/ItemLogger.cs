using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLogger : MonoBehaviour
{
    public Text text;

    public ScrollRect scrollbar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            string currenttext = text.text;
            text.text = currenttext + "\nNew Line";
            Debug.Log(text.text);
            RectTransform rect = text.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + 100f);
            scrollbar.normalizedPosition = new Vector2(0, 0);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            string currenttext = text.text;
            text.text = currenttext + "\nasdfkajshdfkljhasdfkljsadfsadfadsfsdafadsfsadfsadfadsfsadfsadf";
            Debug.Log(text.text);
            RectTransform rect = text.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + 100f);
            scrollbar.normalizedPosition = new Vector2(0, 0);
        }
    }


    public void NewLog(String newItem)
    {
        string currenttext = text.text;
        text.text = currenttext + "\n" + newItem;
        Debug.Log(text.text);
        RectTransform rect = text.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + 100f);
        scrollbar.normalizedPosition = new Vector2(0, 0);
    }
    
}
