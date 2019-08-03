//using System.Collections;
//using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 998f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    static private int sceneIndex = 0;

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
            RespondToThrustInput();
            RespondToRotateInput();
            PlayThrusterAudio();
        }
        else
        {

        }
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
                PlayLevelLoadAudio();
                successParticles.Play();
                Invoke("LoadNextScene", levelLoadDelay);
                break;
            default:
                print("Died!");
                state = State.Dying;
                PlayDeathAudio();
                deathParticles.Play();
                Invoke("LoadFirstScene", levelLoadDelay);
                break;
        }
    }

    private void PlayLevelLoadAudio()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(levelSound);

    }

    private void PlayDeathAudio()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(deathSound);
    }

    private void LoadNextScene()
    {
        var scenes = SceneManager.sceneCountInBuildSettings;
        if (sceneIndex + 1 == scenes)
            SceneManager.LoadScene(scenes - 1);
        else
            SceneManager.LoadScene(++sceneIndex);
    }

    private void LoadFirstScene()
    {

        sceneIndex = 0;
        SceneManager.LoadScene(0);
    }

    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
            mainEngineParticles.Play();

            //audioSource.PlayOneShot(mainEngine);
        }
        else
        {
            mainEngineParticles.Stop();
            //audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thrustPerFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustPerFrame);
    }

    private void PlayThrusterAudio()
    {
        if (Input.GetKey(KeyCode.Space) && state == State.Alive)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 0.8f;
                audioSource.PlayOneShot(mainEngine);
            }
        }
        else
        {
            if (audioSource.isPlaying && state == State.Alive)
            {
                audioSource.volume -= 0.05f;
                if (audioSource.volume == 0.0f)
                    audioSource.Stop();
            }
        }
    }
}
