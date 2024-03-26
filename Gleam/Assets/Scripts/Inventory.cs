using UnityEngine;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class Collectables
    {
        public int greenCollectable;
        public int blueCollectable;     
        public int RedCollectable;
        public int maxBlueCollect;
        public int maxRedCollect;   
        public int maxGreenCollect;

    }
    public Collectables collectables = new Collectables();
}








/*
  collectable grren 
class bnayege collectables ki

*/