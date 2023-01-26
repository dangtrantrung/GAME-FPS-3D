using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPlayer : MonoBehaviour
{
   [SerializeField] Text playerText;
   Player player;

   public void SetPlayer(Player _player)
   {
      this.player=_player;
      playerText.text= "Player "+player.playerIndex;
   }
}
