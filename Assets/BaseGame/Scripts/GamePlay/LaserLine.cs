using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TW.Utility.CustomComponent;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LaserLine : ACachedMonoBehaviour
{
    public Vector3 StartPos;
    public Vector3 EndPos;

    public float HitOffset = 0;
    public bool useLaserRotation = false;

    public float MaxLength;
    private LineRenderer Laser;

    public float MainTextureLength = 1f;
    public float NoiseTextureLength = 1f;
    private Vector4 Length = new Vector4(1, 1, 1, 1);
    //private Vector4 LaserSpeed = new Vector4(0, 0, 0, 0); {DISABLED AFTER UPDATE}
    //private Vector4 LaserStartSpeed; {DISABLED AFTER UPDATE}
    //One activation per shoot
    //private bool LaserSaver = false;
    //private bool UpdateSaver = false;
    private Action _actionCallBack;
    public GameObject StartPosEffect;
    public ParticleSystem[] EffectsStartPos;
    public GameObject EndPosEffect;
    public ParticleSystem[] EffectsEndPos;
    private Tween _tween;

    private void Awake()
    {
        Laser = GetComponent<LineRenderer>();
        EffectsStartPos = StartPosEffect.GetComponentsInChildren<ParticleSystem>();
        EffectsEndPos = EndPosEffect.GetComponentsInChildren<ParticleSystem>();
        Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
        Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
    }
    public void Setup(Vector3 startPos, Vector3 endPos,bool isImmeditate, Action actionCallBack = null)
    {
        StartPosEffect.SetActive(true);
        EndPosEffect.SetActive(false);
        _tween?.Kill();
        //Get LineRender and ParticleSystem components from current prefab;  
        _actionCallBack = actionCallBack;
        StartPos = startPos;
        EndPos = endPos;
        //EndPos = endPos.position;
        Laser.SetPosition(0, StartPos);
        Laser.SetPosition(1, StartPos);
        EndPosEffect.transform.position = EndPos;
        Laser.enabled = true;
        foreach (var AllPs in EffectsStartPos)
        {
            if (!AllPs.isPlaying) AllPs.Play();
        }
        foreach (var AllPs in EffectsEndPos)
        {
            if (!AllPs.isPlaying) AllPs.Play();
        }
        //Texture tiling
        Length[0] = MainTextureLength * (Vector3.Distance(transform.position, EndPos));
        Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, EndPos));
        if (!isImmeditate)
        {
            //DOVirtual.DelayedCall(0f, () =>
            //{
                // Animate the LineRenderer from startPos to endPos
                float time = Vector3.Distance(StartPos, EndPos) / 100f;
                _tween = DOTween.To(() => Laser.GetPosition(1), x => Laser.SetPosition(1, x), EndPos, time)
                       .OnUpdate(OnDrawing)
                       .OnComplete(OnDrawCompleted)
                       .SetEase(Ease.Linear);
            //});
        }
        else
        {
            Laser.SetPosition(1, EndPos);
            OnDrawCompleted();
        }
    }
    public void OnDrawCompleted()
    {
        EndPosEffect.SetActive(true);
        _actionCallBack?.Invoke();
        Debug.Log("LaserLine DrawCompleted" + Time.deltaTime);
    }
    public void OnDrawing()
    {
        if (!Laser.enabled)
        {
            _tween?.Kill();
            StartPosEffect.SetActive(false);
            EndPosEffect.SetActive(false);
        }
    }
    public void DisablePrepare()
    {
        if (Laser != null)
        {
            Laser.enabled = false;
        }
        _tween?.Kill();
        StartPosEffect.SetActive(false);
        EndPosEffect.SetActive(false);
        EndPosEffect.transform.position = StartPos;
        Debug.Log("LaserLine DisablePrepare" + Time.deltaTime);
    }
}
