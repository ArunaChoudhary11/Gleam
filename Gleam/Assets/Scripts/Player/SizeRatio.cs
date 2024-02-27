using UnityEngine;

public class SizeRatio : MonoBehaviour
{
    public float ratio;
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            ratio -= 0.1f;
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            ratio += 0.1f;
        }

        ratio = Mathf.Clamp(ratio, 0.1f, 1f);
        transform.position = Vector3.up * ratio * -10;


        transform.localScale = Vector2.one * ratio * 2;
    }
}