using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GamePlay : UICanvas
{
        [SerializeField] private Text _levelText;
   
        [SerializeField] private GameObject WinMess;
        //[SerializeField] private GameManager _gameManager;
        [SerializeField] private PlayerMoveMent player;

        private void Awake()
        {
            if(player == null)
            {
                player = FindObjectOfType<PlayerMoveMent>();
            }
        }

    
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log(scene.name);
           
        }
    public void Move(int dir)
    {
        player.Move(dir);
    }

    public void PauseBtn()
    {
        UIManager.Instance.OpenUI<Pause>();
        Time.timeScale = 0;
        SoundManager.Instance.PlayClickSound();
    }

    private void Update()
    {
        if(player == null)
        {
            player = FindObjectOfType<PlayerMoveMent>();
        }
        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
       if (_levelText != null)
        {   
            int levelNumber = SceneManager.GetActiveScene().buildIndex;
            _levelText.text = $"Level: {levelNumber:D2}"; // Hiển thị với 2 chữ số, ví dụ: 01, 02
        }   
    }
}
