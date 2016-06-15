using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GoToPlay : MonoBehaviour
{
    public AudioClip startBTNsound=null;
    
    public void Awake()
    {
       // startBTNsound = GetComponent<AudioClip>();
    }

    public void StartBTN()
    {
        GetComponent<AudioSource>().clip = startBTNsound;
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("PlayScene01");
    }

    public void QuitBTN()
    {
        QuitWindow();
    }

    public void Update()
    {


        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {

                Application.Quit();
            }

        }
    }

    void QuitWindow()
    {
        Application.Quit();
    }

    //버튼 사운드
    void SoundPlayStartBTN()
    {
        GetComponent<AudioSource>().clip = startBTNsound;
        GetComponent<AudioSource>().Play();

    }
}
