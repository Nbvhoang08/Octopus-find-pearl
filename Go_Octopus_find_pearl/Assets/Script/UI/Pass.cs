using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Pass : UICanvas
{
    public Sprite OnVolume;
    public Sprite OffVolume;
    [SerializeField] private Image buttonImage;
    // Start is called before the first frame update
    public void NextBtn()
    {
        Time.timeScale = 1;
        UIManager.Instance.CloseUI<Pass>(0.2f);
        LoadNextScene();
          SoundManager.Instance.PlayClickSound();
    }
    public void LoadNextScene() 
    { 
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        int nextSceneIndex = currentSceneIndex + 1; 
        // Kiểm tra xem scene tiếp theo có tồn tại không
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("Home");
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<LevelUI>();
        } 
    }

    IEnumerator NextSence()
    {
        yield return new WaitForSeconds(0.3f);
        LoadNextScene();
    }

    public void HomeBtn()
    {
        UIManager.Instance.CloseAll();
        Time.timeScale = 1; 
        SceneManager.LoadScene("Home");        
        UIManager.Instance.OpenUI<LevelUI>();
        SoundManager.Instance.PlayClickSound();
            
    }
    public void SoundBtn()
    {
        SoundManager.Instance.TurnOn = !SoundManager.Instance.TurnOn;
        SoundManager.Instance.PlayClickSound();
        UpdateButtonImage();  
    }    

    private void UpdateButtonImage()
    {
        if (SoundManager.Instance.TurnOn)
        {
            buttonImage.sprite = OnVolume;
        }
        else
        {
            buttonImage.sprite = OffVolume;
        }
    } 
}
