using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    //static Singleton instance;

    // Start is called before the first frame update
    void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        //DontDestroyOnLoad(gameObject);

        if(FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
