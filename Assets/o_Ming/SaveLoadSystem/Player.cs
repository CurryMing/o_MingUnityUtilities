using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using o_Ming;

public class Player : MonoBehaviour
{
    public int level = 1;
    public int health = 100;

    //UI Methods
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData playerData = SaveSystem.LoadPlayer();

        level = playerData.level;
        health = playerData.health;

        Vector3 position = Vector3.one;
        position.x = playerData.position[0];
        position.y = playerData.position[1];
        position.z = playerData.position[2];
        transform.position = position;
    }
    
}
