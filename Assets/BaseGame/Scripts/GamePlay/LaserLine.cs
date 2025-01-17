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
        StartPosEffect.SetActive(true);
        //Texture tiling
        Length[0] = MainTextureLength * (Vector3.Distance(transform.position, EndPos));
        Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, EndPos));
        _tween?.Kill();
        if (!isImmeditate)
        {
            _tween = DOVirtual.DelayedCall(0f, () =>
            {
                // Animate the LineRenderer from startPos to endPos
                float time = Vector3.Distance(StartPos, EndPos) / 200f;
                DOTween.To(() => Laser.GetPosition(1), x => Laser.SetPosition(1, x), EndPos, time)
                       .OnUpdate(OnDrawing)
                       .OnComplete(OnDrawCompleted)
                       .SetEase(Ease.Linear);
            });
        }
        else
        {
            Laser.SetPosition(1, EndPos);
            OnDrawCompleted();
        }
        //if (Laser.material.HasProperty("_SpeedMainTexUVNoiseZW")) LaserStartSpeed = Laser.material.GetVector("_SpeedMainTexUVNoiseZW");
        //Save [1] and [3] textures speed
        //{ DISABLED AFTER UPDATE}
        //LaserSpeed = LaserStartSpeed;
    }
    public void OnDrawCompleted()
    {
        _actionCallBack?.Invoke();
        Debug.Log("LaserLine DrawCompleted" + Time.deltaTime);
    }
    public void OnDrawing()
    {

    }
    //void Update()
    //{
    //    //if (Laser.material.HasProperty("_SpeedMainTexUVNoiseZW")) Laser.material.SetVector("_SpeedMainTexUVNoiseZW", LaserSpeed);
    //    //SetVector("_TilingMainTexUVNoiseZW", Length); - old code, _TilingMainTexUVNoiseZW no more exist

    //    //To set LineRender position
    //    if (Laser != null && UpdateSaver == false)
    //    {

    //        //ADD THIS IF YOU WANNT TO USE LASERS IN 2D:
    //        RaycastHit2D hit = Physics2D.Raycast(StartPos, transform.up, MaxLength);       
    //        if (hit.collider != null)//CHANGE THIS IF YOU WANT TO USE LASERRS IN 2D: if (hit.collider != null)
    //        {
    //            //End laser position if collides with object

    //            //Texture speed balancer {DISABLED AFTER UPDATE}
    //            //LaserSpeed[0] = (LaserStartSpeed[0] * 4) / (Vector3.Distance(transform.position, hit.point));
    //            //LaserSpeed[2] = (LaserStartSpeed[2] * 4) / (Vector3.Distance(transform.position, hit.point));
    //            //Destroy(hit.transform.gameObject); // destroy the object hit
    //            //hit.collider.SendMessage("SomeMethod"); // example
    //            /*if (hit.collider.tag == "Enemy")
    //            {
    //                hit.collider.GetComponent<HittedObject>().TakeDamage(damageOverTime * Time.deltaTime);
    //            }*/
    //        }
    //        else
    //        {
    //            //End laser position if doesn't collide with object
    //            var EndPos = transform.position + transform.up * MaxLength;
    //            Laser.SetPosition(1, EndPos);
    //            //Texture tiling
    //            Length[0] = MainTextureLength * (Vector3.Distance(transform.position, EndPos));
    //            Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, EndPos));
    //            //LaserSpeed[0] = (LaserStartSpeed[0] * 4) / (Vector3.Distance(transform.position, EndPos)); {DISABLED AFTER UPDATE}
    //            //LaserSpeed[2] = (LaserStartSpeed[2] * 4) / (Vector3.Distance(transform.position, EndPos)); {DISABLED AFTER UPDATE}
    //        }
    //        //Insurance against the appearance of a laser in the center of coordinates!
    //        if (Laser.enabled == false && LaserSaver == false)
    //        {
    //            LaserSaver = true;
    //            Laser.enabled = true;
    //        }
    //    }
    //}

    public void DisablePrepare()
    {
        if (Laser != null)
        {
            Laser.enabled = false;
        }
        //UpdateSaver = true;
        //Effects can = null in multiply shooting
        if (EffectsStartPos != null)
        {
            foreach (var AllPs in EffectsStartPos)
            {
                if (AllPs.isPlaying) AllPs.Stop();
            }
        }
        if (EffectsEndPos != null)
        {
            foreach (var AllPs in EffectsEndPos)
            {
                if (AllPs.isPlaying) AllPs.Stop();
            }
        }
        StartPosEffect.SetActive(false);
        EndPosEffect.SetActive(false);
        _tween?.Kill();
        Debug.Log("LaserLine DisablePrepare" + Time.deltaTime);
    }
}
