using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    private PlayerManager player;
    [SerializeField] private TextMeshProUGUI instructionsText;

    /*
        WASD To Move.
        Space to Jump.
        Left Shift to Dash.
    */
    IEnumerator Start()
    {
        player = PlayerManager.Instance;

        player.CanAttack = true;
        instructionsText.text = "WASD to Move";
        player.CanMove = true;
        yield return new WaitForSeconds(2f);
        player.CanJump = true;
        instructionsText.text = "Space to Jump";
        yield return new WaitForSeconds(2f);
        player.CanDash = true;
        instructionsText.text = "Left Shift to Dash";
        yield return new WaitForSeconds(2f);
        instructionsText.text = "";
    }
    public void TutorialCompleted()
    {
        Debug.Log("Prologue Finished");
    }
}