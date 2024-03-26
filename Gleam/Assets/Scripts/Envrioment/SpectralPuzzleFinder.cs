using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectralPuzzleFinder : MonoBehaviour
{

    public class puzzleObject
    {
        public Color lightcolor;
        public GameObject hiddenObject;

    }
    public bool lightIsOn;
    public int index;
    public puzzleObject[] puzzleObjects;
    public puzzleObject currentObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentObject = puzzleObjects[index];
       if(lightIsOn)
        {
            Debug.Log("lightison");

        }
    }
}
