using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Vector2 mouseInput;
    private bool inputKey;
    public int currentBgmIndex;
    public float BgmLength;
    private AudioSource audioSource;
    public AudioClip[] Bgm;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        GlobalInput();
        Music();
    }
    private void GlobalInput()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            inputKey = Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2);

            if(inputKey)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if(mouseInput.magnitude > 0)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            return;
        }
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void LevelFinish(bool completed)
    {
        if(completed == true)
        {
            if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }

            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void Music()
    {
        if(BgmLength <= 0)
        {
            audioSource.clip = Bgm[currentBgmIndex];
            BgmLength = audioSource.clip.length;
            audioSource.Play();
            currentBgmIndex++;
        }

        BgmLength -= Time.deltaTime;
    }
}