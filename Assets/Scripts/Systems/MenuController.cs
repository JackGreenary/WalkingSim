using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Awake()
    {

    }

    void PlayScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
