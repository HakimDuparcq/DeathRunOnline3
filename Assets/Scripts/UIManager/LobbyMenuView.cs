using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LobbyMenuView : View
{
    [SerializeField] private Button _startGame;
    public override void Initialize()
    {
        _startGame.onClick.AddListener(() => MainGame.instance.CmdStartGame());
        _startGame.onClick.AddListener(() => ViewManager.Show<NoUIView>());
        _startGame.onClick.AddListener(() => Cursor.lockState = CursorLockMode.Locked);


    }
   
    public void OnEnable()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }



    public void Update()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            ViewManager.Show<EscapeMenuView>();
        }
    }

}
