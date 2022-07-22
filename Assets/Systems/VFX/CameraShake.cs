using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static void Do(float strength, float duration)
    {
        instance?.StartCoroutine(strength, duration);
    }
    private static CameraShake instance;
    private Coroutine Shake;
    private const float FREQUENCY = 8f;
    private const float SQRT3 = 1.73205080757f;
    private const float AMPLITUDE = 0.769800359f;

    private void OnEnable() => enabled = ((instance ??= this) == this);
    private void OnDisable() => instance = ((this == instance) ? null : instance);

    private void StartCoroutine(float strength, float duration)
    {
        if (Shake != null)
            StopCoroutine(Shake);
        Shake = StartCoroutine(DoCameraShake(strength, duration));
    }

    private IEnumerator DoCameraShake(float strength, float duration)
    {
        var source = Vector3.zero;
        var target = Random.insideUnitSphere;
        var t = 0f;
        var remainder = ((duration * FREQUENCY) % 1) + 0.000000001f;
        var cycleCount = Mathf.FloorToInt(duration * FREQUENCY);
        var cycle = 0;
        while (t <= duration)
        {
            t += Time.deltaTime;
            var currentCycle = Mathf.FloorToInt(t * FREQUENCY);
            var isLastCycle = currentCycle == cycleCount;
            if (cycle < currentCycle)
            {
                cycle++;
                source = target;
                target = isLastCycle ? Vector3.zero : Random.insideUnitSphere;
            }
            var x = (t * FREQUENCY) % 1;
            x /= isLastCycle ? remainder : 1f;
            var rotation = SmoothLerp(source, target, x);
            transform.localRotation = Quaternion.Euler(rotation * strength);
            yield return null;
        }
        transform.localRotation = Quaternion.identity;
        Shake = null;
    }

    private Vector3 SmoothLerp(Vector3 a, Vector3 b, float t)
    {
        return Vector3.Lerp(a, b, SmoothLerp(0, 1, t));
    }

    private float SmoothLerp(float a, float b, float t)
    {
        t -= 0.5f;
        t /= (SQRT3) / 2;
        t = (t - t * t * t) / 0.769800359f + 0.5f;
        t = Mathf.Clamp01(t);
        return a * (1 - t) + b * t;
    }
}
