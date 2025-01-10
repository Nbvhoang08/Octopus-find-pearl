using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Pause : UICanvas
{

    public Sprite OnVolume;
    public Sprite OffVolume;
    [SerializeField] private Image buttonImage;
    void Start()
    {  
        UpdateButtonImage();
    }
    public void Resume()
    {
        Time.timeScale = 1;
        UIManager.Instance.CloseUI<Pause>(0.2f);
        SoundManager.Instance.PlayClickSound();
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

