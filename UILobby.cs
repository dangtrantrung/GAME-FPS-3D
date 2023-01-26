using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILobby : MonoBehaviour
{
    public static UILobby instance;
    [Header("Host - Join")]
    [SerializeField] TMP_InputField inputMatchID;
    [SerializeField] Button JoinBtn;
    [SerializeField] Button HostBtn;
    [SerializeField] Canvas Lobby;

    [Header("Lobby")]
    [SerializeField] Transform UIPlayerParent;    
    [SerializeField] GameObject UIPlayerPrefab;
    [SerializeField] TMP_Text MatchIDtxt;
    [SerializeField] GameObject sTARTbTN;
    [SerializeField] GameObject QUITbTN;
    
    // Start is called before the first frame update
    private void Start()
    {
            instance=this;
            
    }
    public void Host()
    {
            inputMatchID.interactable=false;
            JoinBtn.interactable=false;
            HostBtn.interactable=false;
            Player.localPlayer.HostGame();
    }
    public void Join()
    {
            inputMatchID.interactable=false;
            JoinBtn.interactable=false;
            HostBtn.interactable=false;
            Player.localPlayer.JoinGame(inputMatchID.text.ToUpper());
    }
    public void StartGame()
    {
            inputMatchID.interactable=false;
            JoinBtn.interactable=false;
            HostBtn.interactable=false;
            Player.localPlayer.BeginGame(MatchIDtxt.text);
            
    }

        public void UpdateUI()
        {
            sTARTbTN.SetActive(false);
            QUITbTN.SetActive(true);
        }
        public void Quit()
        {
            Application.Quit();
        }
    public void HostSuccess(bool success,string matchID)
    {
       if(success)
       {
        Lobby.enabled=true;
        SpawnPlayerUIPrefabs(Player.localPlayer);
        MatchIDtxt.text=matchID;
       }
       else
       {
            inputMatchID.interactable=true;
            JoinBtn.interactable=true;
            HostBtn.interactable=true;   
       }
       
    }
    public void JoinSuccess(bool success, string matchID)
    {
       if(success)
       {
        Lobby.enabled=true;
        SpawnPlayerUIPrefabs(Player.localPlayer);
        MatchIDtxt.text=matchID;
       }
       else
       {
            inputMatchID.interactable=true;
            JoinBtn.interactable=true;
            HostBtn.interactable=true;   
       }
       
    }

   

    public void SpawnPlayerUIPrefabs(Player player)
    {
            GameObject newUiPlayer=Instantiate(UIPlayerPrefab,UIPlayerParent);
            newUiPlayer.GetComponent<UIPlayer>().SetPlayer(player);
            newUiPlayer.transform.SetSiblingIndex(player.playerIndex-1);
    }
    
}
