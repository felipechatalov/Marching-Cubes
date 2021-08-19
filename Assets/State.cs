using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    
    public bool isOn;
    public float value;
    
    // public int triangNumber;

    void Start()
    {
        
    }

    public void SetValue(float value){
        this.value = value;
        // GetComponentInChildren<Renderer>().material.SetColor("_Color", new Color(value, value, value, 1f));
    }

    public void SetColor(Color color){
        GetComponentInChildren<Renderer>().material.SetColor("_Color", color);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
