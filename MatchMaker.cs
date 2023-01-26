using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Text;
using System.Security.Cryptography;

[System.Serializable]
    public class Match
    {
        public string matchID;
        public SyncListGameObject players = new SyncListGameObject();

        public Match (string matchID, GameObject player)
        {
            this.matchID=matchID;
            players.Add(player);
        }
        public Match()
        {

        }

    }
    [System.Serializable]
    public class SyncListMatch:SyncList<Match>
    {
        
    }


    [System.Serializable]
    public class SyncListGameObject:SyncList<GameObject>
    {
        
    }

public class MatchMaker : NetworkBehaviour
{    
    public static MatchMaker instance;

    public SyncListMatch matches= new SyncListMatch();
    public SyncList<string> matchIDs=new SyncList<string>();
    [SerializeField] GameObject TurnManagerPrefabs;

  private void Start()
  {
      instance=this;
      //DontDestroyOnLoad(this.gameObject);
  }
    public bool HostGame(string _matchID, GameObject _player, out int playerIndex)
    {
        playerIndex=-1;
        if(!matchIDs.Contains(_matchID))
        {
            matchIDs.Add(_matchID);
            matches.Add(new Match(_matchID,_player));
            Debug.Log($"Match generated with MatchID: "+_matchID);
            playerIndex=1;
            return true;
        }
        else
        {
            Debug.Log("MatchID already exits");
            return false;
        }
        
    }
    public bool JoinGame(string _matchID, GameObject _player, out int playerIndex)
    {   playerIndex=-1;
        if(matchIDs.Contains(_matchID))
        {
            for (int i =0; i<matches.Count;i++)
            {
                if(matches[i].matchID==_matchID)
                {
                    matches[i].players.Add(_player);
                    playerIndex=matches[i].players.Count;
                    break;
                }
            }
            Debug.Log("Match joined");
            return true;
        }
        else
        {
            Debug.Log("Match joined unsuccessfully");
            return false;
        }
        
    }
    public void BeginGame(string _matchID)
    {
        Debug.Log("maker _matchID "+_matchID);  
        GameObject _turnManager= Instantiate(TurnManagerPrefabs);
        NetworkServer.Spawn(_turnManager);
        if(_turnManager==null) return;
        else
        {
        _turnManager.GetComponent<NetworkMatchChecker>().matchId = _matchID.ToGuid();
        TurnManager turnManager =_turnManager.GetComponent<TurnManager>();

        for (int i = 0; i < matches.Count; i++)
        {
            if( matches[i].matchID==_matchID)
            {   
                foreach (var player in matches[i].players)
                {
                    Player _player= player.GetComponent<Player>();
                    turnManager.AddPlayer(_player);
                    _player.StartGame(_matchID);
                }
                break;
            }
            
        }
        }

    }
    public static string GetRandomMatchID()
    { 
        string _id=string.Empty;
        for (int i=0; i<5;i++ )
        {
            int random=UnityEngine.Random.Range(0,36);
            if(random<26)
            {
                _id+=(char)(random+65);
            }
            else
            {
                _id+=(random-26).ToString();

            }                
        }
        Debug.Log($"Random Match ID:{_id}");
       return _id;
    }
}

public static class MatchExtensions
{
    public static Guid ToGuid (this string id)
    {
            MD5CryptoServiceProvider provider= new MD5CryptoServiceProvider();
            byte[] inputbytes=Encoding.Default.GetBytes(id);
            byte[] Hasbytes= provider.ComputeHash(inputbytes);
            return new Guid(Hasbytes);
    }
}