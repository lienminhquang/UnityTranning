using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instantiate = null;
    private PlayableDirector playableDirector;

    private void Awake()
    {
        if(instantiate == null)
        {
            instantiate = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayPlaerRespawnCutscene()
    {
        playableDirector.Play();
    }

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        
    }
}
