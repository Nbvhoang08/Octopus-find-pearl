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
            // if (_gameManager == null)
            // {
            //     _gameManager = FindObjectOfType<GameManager>();
            // }
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
        }

    private void Update()
    {
        UpdateLevelText();
            // if (_gameManager == null)
            // {
            //     _gameManager = FindObjectOfType<GameManager>();
            // }
            // if (_gameManager.hasWon)
            // {
            //     WinMess.SetActive(true);
                
            // }
            // else
            // {
            //     WinMess.SetActive(false);
        // }
    }

        private void UpdateLevelText()
        {
            if (_levelText != null)
            {
                 _levelText.text = "Level: "+ SceneManager.GetActiveScene().name;
            }
        }
}
