using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using o_Ming;

public class Player : MonoBehaviour
{
    public GameObject textPrefab;

    public int level = 1;
    public int health = 100;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        //string saveTip = "Saved";
        GameObject textClone = Ming.TextUp2D(textPrefab, transform.position);
        StartCoroutine(Ming.TextAnimation(textClone));
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
        //string loadedTip = "Loaded";
        GameObject textClone = Ming.TextUp2D(textPrefab, transform.position);
        StartCoroutine(Ming.TextAnimation(textClone));
    }
}
