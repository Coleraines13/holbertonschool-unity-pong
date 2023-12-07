using System.Collections;
using UnityEngine;

public class PaddleShake : MonoBehaviour
{
    // Adjust these variables based on your desired shaking effect
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.1f;

    // Call this method to start the shaking effect
    public void StartShake()
    {
        Debug.Log("SHaking");
        StartCoroutine(Shake());
    }

    // Coroutine for the shaking effect
    private IEnumerator Shake()
    {
        Vector3 originalPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Generate a random offset for the shake
            Vector2 randomOffset = Random.insideUnitCircle * shakeIntensity;

            // Apply the offset to the paddle's position
            transform.position = new Vector3(originalPosition.x + randomOffset.x, originalPosition.y + randomOffset.y, originalPosition.z);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset the position after the shake is complete
        transform.position = originalPosition;
    }
}
