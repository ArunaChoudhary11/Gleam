using UnityEngine;

public class Phirta_Hua : MonoBehaviour
{
    public GameObject[] poses;
    void Start()
    {
        SetCurrentPose();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetCurrentPose();   
        }
    }
    private void SetCurrentPose()
    {
        int index = Random.Range(0, poses.Length);

        for(int i = 0; i < poses.Length; i++)
        {
            if(i == index)
            {
                poses[i].SetActive(true);
            }
            else
            {
                poses[i].SetActive(false);
            }
        }
    }
}