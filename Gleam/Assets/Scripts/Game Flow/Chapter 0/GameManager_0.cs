using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class GameManager_0 : MonoBehaviour
{
    [SerializeField] private VideoClip[] clips;
    [SerializeField] private bool cutscene_Completed;
    [SerializeField] private string[] steps;
    [SerializeField] private int currentIndex;
    private int nextSceneIndex;
    private InputManager input;
    [SerializeField] private Animator enemyAnimator;
    IEnumerator cutsceneCoroutine;
    IEnumerator EnemyAttackCoroutine;

    [SerializeField] private Transform player;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Animator[] enemies;
    [SerializeField] private Transform[] enemiesPos;
    [SerializeField] private GameObject BlackScreen;
    [SerializeField] private int typeSpeed;
    [SerializeField] private TextMeshProUGUI instructionText;
    private bool isAttacking;
    /*
        1. Cutscene
        2. Time Freeze
        3. Player Input
        4. Shield
        5. Cutscene
        6. Logo Flash
        8. Splash Screen
        9. Scene Change to Tutorial
    */

    void Start()
    {
        nextSceneIndex = 2;
        input = InputManager.Instance;
        CutsceneManager.Instance.SetCutscene(clips[0]);
        cutsceneCoroutine = Cutscene_0Completed();
        EnemyAttackCoroutine = EnemyAttack();
    }
    void Update()
    {
       SequenceSetter();
    }
    private void SequenceSetter()
    {
        if(cutscene_Completed == false)
        {
            instructionText.text = "";
            cutscene_Completed = CutsceneManager.Instance.IsCutsceneCompleted;
        
            if(cutscene_Completed == true)
            {
                if(currentIndex == 0) StartCoroutine(cutsceneCoroutine);

                else if(currentIndex == 1)
                {
                    isAttacking = true;
                    StopCoroutine(cutsceneCoroutine);
                    StartCoroutine(EnemyAttackCoroutine);
                }

                else if(currentIndex == 2)
                {
                    currentIndex++;
                }
            }

            return;
        }

        switch(currentIndex)
        {
            case 1:
                if(input.Jump() && isAttacking == false) Cutscene_1(clips[1]);
            break;

            case 2:
                if(input.Attack()) StartCoroutine(Cutscene_2(clips[2]));
            break;
            
            case 3:
                SceneManager.LoadSceneAsync(2);
            break;
        }
    }
    private IEnumerator Cutscene_0Completed()
    {
        yield return new WaitForSeconds(0.5f);
        enemyAnimator.enabled = true;
        yield return new WaitForSeconds(1.25f);
        instructionText.text = steps[0];
        currentIndex++;
        Time.timeScale = 0.002f;
        yield return new WaitForSecondsRealtime(20f);
        Time.timeScale = 0f;
    }
    private void Cutscene_1(VideoClip clip)
    {
        enemyAnimator.enabled = false;
        cutscene_Completed = false;
        CutsceneManager.Instance.SetCutscene(clip);
        Time.timeScale = 1;
    }
    private IEnumerator EnemyAttack()
    {
        player.position = playerPos.position;
        enemies[0].transform.position = enemiesPos[0].position;
        enemies[1].transform.position = enemiesPos[1].position;
        Camera.main.transform.position = new Vector3(-15, 0, -10);

        yield return new WaitForSeconds(.5f);

        foreach(Animator A in enemies)
        {
            A.enabled = true;
            A.Play("Attack");
        }
        
        yield return new WaitForSeconds(0.4f);

        currentIndex++;
        
        instructionText.text = steps[1];

        Time.timeScale = 0.005f;
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 0f;
    }
    private IEnumerator Cutscene_2(VideoClip clip)
    {
        StopCoroutine(EnemyAttackCoroutine);
        CutsceneManager.Instance.SetCutscene(clip);
        
        foreach(Animator A in enemies) A.enabled = true;

        cutscene_Completed = false;
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.2f);
        BlackScreen.SetActive(true);
    }
}