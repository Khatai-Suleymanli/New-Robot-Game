using UnityEngine;

public class LightOffOn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Light targetLight;
    public float maxIntensity = 10f;
    public float speed = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetLight != null) {
            targetLight.intensity = Mathf.PingPong(Time.time * speed, maxIntensity);
        }
    }
}
