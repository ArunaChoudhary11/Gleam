using System.Collections.Generic;
using UnityEngine;

public class Probabilty : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // SimpleProbabilty(10);
            Debug.Log(GetItemByProbability(Items));
            Debug.Log("Print");
        }       
    }
    private bool SimpleProbabilty(int probabilty)
    {
        float random = Random.Range(0, 101);

        if(random <= probabilty)
        {
            Debug.Log("True");
            return true;
        }

        Debug.Log("False");
        return false;
    }
    private List<float> cumulativeProbabilty;
    public List<float> Items;
    private void CumulativeProbabilty(List<float> probability)
    {
        float sum = 0;

        cumulativeProbabilty = new List<float>();

        for(int i = 0; i < probability.Count; i++)
        {
            sum += probability[i];
            cumulativeProbabilty.Add(sum);
        }

        if(sum > 100)
            Debug.Log("Sum > 100");
    }
    private int GetItemByProbability(List<float> probability)
    {
        CumulativeProbabilty(probability);
        // GetItemRarity(probability);
        float rnd = Random.Range(0, 101);
        
        for(int i = 0; i < probability.Count; i++)
        {
            if(rnd <= cumulativeProbabilty[i])
            {
                return i;
            }
        }
        return -1;
    }
    private float ByRarity(List<float> probability)
    {
        float itemRaritySum = 0;

        for(int i = 0; i < probability.Count; i++)
        {
            itemRaritySum += probability[i];
        }

        return 100 / itemRaritySum;
    }
    private void GetItemRarity(List<float> probability)
    {
        float sum = 0;

        cumulativeProbabilty = new List<float>();
        float modifier = ByRarity(probability);

        for(int i = 0; i < probability.Count; i++)
        {
            sum += probability[i] * modifier;
            cumulativeProbabilty.Add(sum);
        }
    }
    private bool ByChangeInProbabilty(int chance, int In)
    {
        int total = Random.Range(0, In + 1);

        if(total <= chance)
            return true;

        return false;
    }
}