using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    [Range(0, 1)]
    public float movementFactor;

    const float tau = Mathf.PI * 2;

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) return;
        float cycles = Time.time / period / 2;

        float rawSineWave = Mathf.Sin(cycles * tau);

        movementFactor = Mathf.Abs(rawSineWave / 1f);

        transform.position = startingPos + movementVector * movementFactor;
    }
}
