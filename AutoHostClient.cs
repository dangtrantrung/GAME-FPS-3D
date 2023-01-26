using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AutoHostClient : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    
    /* Start is called before the first frame update
    [Server]
    private void Start()
    {    
        // Start in headless - 
        // Automatically start mirror server
        networkManager.StartServer();

        
        /*if(!Application.isBatchMode) // headless server
        { 
          Debug.Log("Client Build");
          networkManager.StartClient();
        }
        else
        {
            Debug.Log("Server Build");
        }
        
    
    }
    
    */

    // Update is called once per frame
    public void JoinLocal()
    {
        networkManager.networkAddress="localhost";
        networkManager.StartClient();
    }
}
