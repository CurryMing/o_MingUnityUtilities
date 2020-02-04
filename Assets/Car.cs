using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    private Transform player;

    public GameObject driveUI;
    public GameObject sitUI;
    public GameObject sitPanel;

    public bool isSitting;
    public bool hasDriver;

    private Image[] sitImage = new Image[4];

    private void Awake()
    {
        player = FindObjectOfType<o_Dash>().transform;
        for (int i = 0; i < sitImage.Length; i++)
        {
            sitImage[i] = sitPanel.transform.GetChild(i).GetComponent<Image>();
        }
    }

    private void Update()
    {
        GetUI();
        SetDriveImage();
    }

    private void SetDriveImage()
    {
        if (hasDriver)
        {
            sitImage[0].color = Color.yellow;
        }
        else
        {
            sitImage[0].color = Color.white;
        }
    }

    private void GetUI()
    {
        bool hasSit = true;

        for (int i = 1; i < sitImage.Length; i++)
        {
            if (sitImage[i].color == Color.white)
            {
                hasSit = true;
                break;
            }
        }

        if (hasDriver == true || isSitting == true)
        {
            //player.GetComponent<o_Player>().enabled = false;
            //return;
        }

        if (Vector2.Distance(transform.position, player.position) < 2f)
        {
            if (hasDriver)
                driveUI.SetActive(false);
            else
                driveUI.SetActive(true);

            if (hasSit)
                sitUI.SetActive(true);
            else
                sitUI.SetActive(false);
        }
        else
        {
            driveUI.SetActive(false);
            sitUI.SetActive(false);
        }
    }
    //UIMethod
    public void Drive()
    {
        sitPanel.SetActive(true);
        driveUI.SetActive(false);
        hasDriver = true;
        Debug.Log("驾驶");
    }
    public void Sit()
    {
        sitPanel.SetActive(true);
        driveUI.SetActive(false);
        isSitting = true;
        int randomNum = Random.Range(1, 3);
        sitImage[randomNum].color = Color.yellow;

        Debug.Log("乘坐");
    }
    public void GetOffCar()
    {
        hasDriver = false;
        isSitting = false;
        sitPanel.SetActive(false);
        ResetImage();
        player.GetComponent<o_Dash>().enabled = true;
        Debug.Log("下车");
    }
    public void ChangeSit()
    {
        int i = 0;
        int j = 0;

        //判断是否处于驾驶状态
        if (hasDriver)
        {
            for (j = 1; j < sitImage.Length; j++)
            {
                //如果找到空位置
                if (sitImage[j].color == Color.white)
                {
                    sitImage[j].color = Color.yellow;
                    isSitting = true;

                    sitImage[0].color = Color.white;
                    hasDriver = false;
                    break;
                }
            }
            return;
        }
        //如果处于乘坐状态
        if (isSitting)
        {
            for (i = 0; i < sitImage.Length; i++)
            {
                //找到空座位
                if (sitImage[i].color != Color.yellow)
                {
                    ResetImage();

                    //如果驾驶位置没有人
                    if (i == 0)
                    {
                        hasDriver = true;

                        break;
                    }

                    sitImage[i].color = Color.yellow;
                    break;
                }
            }
        }

        if (i == sitImage.Length)
        {
            Debug.Log("此座位已经有人了");
        }
        else
        {
            Debug.Log("换座位");
        }

    }

    public void ResetImage()
    {
        for (int i = 0; i < sitImage.Length; i++)
        {
            if (sitImage[i].color != Color.white)
                sitImage[i].color = Color.white;
        }
    }

}
