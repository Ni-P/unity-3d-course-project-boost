using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField]
    float rcsThrust = 100f;
    [SerializeField]
    float mainThrust = 998;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
        Audio();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Fuel":
                print("fueled!");
                break;
            default:
                print("Died!");
                break;
        }
    }


    private void Rotate()
    {
        rigidBody.freezeRotation = true; // stop physics

        float rotationPerFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationPerFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(Vector3.back * rotationPerFrame);
        }

        rigidBody.freezeRotation = false;
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustPerFrame = mainThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustPerFrame);
        }
    }

    private void Audio()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 0.8f;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.volume -= 0.05f;
                if (audioSource.volume == 0.0f)
                    audioSource.Stop();
            }
        }
    }
}
