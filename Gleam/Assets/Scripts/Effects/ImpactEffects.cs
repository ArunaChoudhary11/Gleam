using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ImpactEffects : MonoBehaviour
{
    public static ImpactEffects Instance;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void FlashOnImpact(SpriteRenderer spriteRend, float duration, Color flashColor)
    {
        StartCoroutine(Flash(spriteRend, duration, flashColor));
    }
    private IEnumerator Flash(SpriteRenderer spriteRend, float duration, Color flashColor)
    {
        Color origColor = spriteRend.color;
        spriteRend.color = flashColor;
        yield return new WaitForSeconds(duration);
        if(spriteRend != null) spriteRend.color = origColor;
    }

    public void TimeFreezeOnImpact(float duration)
    {
        StartCoroutine(TimeFreeze(duration));
    }
    private IEnumerator TimeFreeze(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    private float highFreq;
    private float lowFreq;

    public IEnumerator Vibrate(float duration)
    {
        var gamepad = Gamepad.current;
        gamepad.SetMotorSpeeds(lowFreq, highFreq);

        yield return new WaitForSecondsRealtime(duration);

        gamepad.SetMotorSpeeds(0f, 0f);
    }
}