using UnityEngine;

public class BlindingFlashbang : MonoBehaviour
{
    private FieldOfView View;
    void Start()
    {
        View  = GetComponent<FieldOfView>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            FlashBangs();
        }
    }
    public void FlashBangs()
    {
        View.FindVisibleTargets(transform.position);
        for (int i = 0; i < View.visibleTargets.Count; i++)
        {
            Enemytest enemy = View.visibleTargets[i].GetComponent<Enemytest>();
            if(enemy.isFlashed == false) StartCoroutine (enemy.Flashbang(2));
        }
    }    
}