using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OnCollision : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;


    AudioSource audioSource;

    bool isTranstioning = false;
    bool isCollisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (isTranstioning || isCollisionDisabled)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly Tag":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                Debug.Log("Congrats , yo , you finished ");
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }


    }

    void StartSuccessSequence()
    {
        GetComponent<MoveRocket>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);

    }
    void StartCrashSequence()
    {
        GetComponent<MoveRocket>().enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        Invoke("RespawnLevel", 1f);
    }
    void LoadNextLevel()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void RespawnLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
