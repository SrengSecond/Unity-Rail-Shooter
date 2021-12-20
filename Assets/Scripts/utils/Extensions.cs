using System.Collections;
using UnityEngine;

public static class Extensions
{
    public static void DelayAction(this MonoBehaviour mb, System.Action action, float delay)
    {
        mb.StartCoroutine(DelayedCoroutine(action, delay));
    }

    static IEnumerator DelayedCoroutine(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public static Vector3 GetPositionInsideScreen(Vector2 baseRes, RectTransform rect, float offset)
    {
        float widthBonds = baseRes.x - rect.rect.width - offset;
        float heightBounds = baseRes.y - rect.rect.height - offset;

        Vector2 adjustPos = rect.anchoredPosition;
        adjustPos.x = Mathf.Clamp(adjustPos.x, widthBonds * -0.5f, widthBonds * 0.5f);
        adjustPos.y = Mathf.Clamp(adjustPos.y, heightBounds * -0.5f, heightBounds * 0.5f);
        return adjustPos;
    }

    public static void PlayAudioData(this AudioSource aSource, AudioData audioData)
    {
        aSource.pitch = audioData.GetPitch;
        aSource.clip = audioData.GetAudioClip;
        aSource.Play();
    }
}