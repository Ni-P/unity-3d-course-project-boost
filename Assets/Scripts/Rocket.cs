//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    [SerializeField]
    float rcsThrust = 100f;
    [SerializeField]
    float mainThrust = 998f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        Audio();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return;
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                print("Finish");
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                print("Died!");
                state = State.Dying;
                Invoke("LoadFirstScene", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {

        SceneManager.LoadScene(1);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
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
        if (Input.GetKey(KeyCode.Space) && state == State.Alive)
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
