using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class Player : NetworkBehaviour
{   
    //singleton player to connect between UI Client <-> Server
    public static Player localPlayer; 
    NetworkMatchChecker networkMatchChecker;
    [SyncVar] public string matchID;
    [SyncVar] public int playerIndex;
    private void Start()
    {
        networkMatchChecker=GetComponent<NetworkMatchChecker>();
        if(isLocalPlayer)
        {
            localPlayer=this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            UILobby.instance.SpawnPlayerUIPrefabs(this);
        }
        
        
    }
    public void HostGame()
    {
        string matchID=MatchMaker.GetRandomMatchID();
        CmdHostGame(matchID);
    }

    [Command]
    void CmdHostGame(string _matchID)
    {
        matchID=_matchID;
        if(MatchMaker.instance.HostGame(_matchID,gameObject, out playerIndex))
        {
          Debug.Log($"<color=green> Game hosted successfully</color>");
          //encrypt MD5 _match ID with 5 digits to GuiID
          networkMatchChecker.matchId=_matchID.ToGuid();
          Debug.Log($"Match GuId: "+networkMatchChecker.matchId.ToString());
          TargetHostGame(true,_matchID, playerIndex);
        }
        else
        {
            Debug.Log($"<color=red> Game hosted failed</color>");
            TargetHostGame(false,_matchID,playerIndex);
        }

    }
    
   [TargetRpc]
   public void TargetHostGame(bool success,string _matchID, int _playerIndex)
   {
       playerIndex=_playerIndex;
       matchID=_matchID;
       Debug.Log($"MatchId: {matchID}=={_matchID}");
       UILobby.instance.HostSuccess(success, _matchID);

   }

   public void JoinGame(string inputmatchID )
    {       
        CmdJoinGame(inputmatchID);
    }

  public void BeginGame(string _matchID)
    {  
        Debug.Log("_matchID "+_matchID);    
        CmdStartGame(_matchID);
    }

 [Command]
    void CmdJoinGame(string _matchID)
    {
        matchID=_matchID;
        if(MatchMaker.instance.JoinGame(_matchID,gameObject, out playerIndex))
        {
          Debug.Log($"<color=green> Game Joined successfully</color>");
          //encrypt MD5 _match ID with 5 digits to GuiID
          networkMatchChecker.matchId=_matchID.ToGuid();
          Debug.Log($"Match GuId: "+networkMatchChecker.matchId.ToString());
          TargetJoinGame(true,_matchID, playerIndex);
        }
        else
        {
            Debug.Log($"<color=red> Game joined failed</color>");
            TargetJoinGame(false,_matchID, playerIndex);
        }

    }
    
   [TargetRpc]
   public void TargetJoinGame(bool success,string _matchID, int _playerIndex)
   {   
       playerIndex=_playerIndex;
       matchID=_matchID;
       Debug.Log($"MatchId: {matchID}=={_matchID}");
       UILobby.instance.JoinSuccess(success, _matchID);

   }

   [Command]
    void CmdStartGame(string _matchID)
    {
        matchID=_matchID;
        MatchMaker.instance.BeginGame(matchID);
        Debug.Log($"<color=green> Game Beginning </color>");
        
        
    }

    public void StartGame(string _matchID)
    {
        TargetStartGame(_matchID);

    }
    
   [TargetRpc]
  public void TargetStartGame(string _matchID)
   {
       UILobby.instance.UpdateUI();
       Debug.Log($"MatchId: {_matchID} | Beginning");
       //Addictive load scene game
       //dont destroy Player on loadscene
       SceneManager.LoadScene(2,LoadSceneMode.Additive);
      
   }

}
