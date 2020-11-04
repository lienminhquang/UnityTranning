using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawEffectController : MonoBehaviour
{
    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;

    public ParticleSystem ps;
    float timer = 0;
    public Renderer _renderer;

    int shaderProperty;
    public bool isPlaying = false;

    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        //_renderer = GetComponent<Renderer>();
        //ps = GetComponentInChildren<ParticleSystem>();

        var main = ps.main;
        main.duration = spawnEffectTime;

        //ps.Play();

    }

    public void Restart()
    {
        isPlaying = true;
        ps.Play();
        timer = 0;
    }

    void Update()
    {
        if (isPlaying == false)
        {
            _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(spawnEffectTime));
            return;
        }

        if (timer < spawnEffectTime + pause)
        {
            timer += Time.deltaTime;
        }
        else
        {
            //timer = 0;
            //ps.Play();
            isPlaying = false;
        }

        //_renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));

    }
}
