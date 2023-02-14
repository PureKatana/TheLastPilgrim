using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Renderer _renderer;
    [SerializeField] AnimationCurve _DisplacementCurve;
    [SerializeField] float _DisplacementMagnitude;
    [SerializeField] float _LerpSpeed;
    [SerializeField] float _DisolveSpeed;
    Coroutine _disolveCoroutine;
    [SerializeField] private AudioSource shieldAudio;
    [SerializeField] private AudioClip turnOn;
    [SerializeField] private AudioClip turnOff;

    Ray ray;
    RaycastHit hit;
    Vector3 hitNormal;
    Vector3 hitPos;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetFloat("_Disolve", 1);
        shieldAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                HitShield(hit.point);
            }
        }*/
    }


    public void HitShield(Vector3 hitPos)
    {
        _renderer.material.SetVector("_HitPos", hitPos);
        StopAllCoroutines();
        StartCoroutine(Coroutine_HitDisplacement());
    }

    public void TriggerShield()
    {
        StartCoroutine(ActivateShield());
    }

    private IEnumerator ActivateShield()
    {
        yield return new WaitForSeconds(0.3f);
        OpenCloseShield(0);
        GameManager.instance.PlaySoundEffect(shieldAudio, turnOn, 0.3f, 1f);
        Player.instance.shieldOn = true;
        yield return new WaitForSeconds(GameObject.Find("Player").GetComponentInChildren<AbilityHolder>().ability[3].activeTime);
        OpenCloseShield(1);
        GameManager.instance.PlaySoundEffect(shieldAudio, turnOff, 0.3f, 1f);
        Player.instance.shieldOn = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    public void OpenCloseShield(float target)
    {
        if (_disolveCoroutine != null)
        {
            StopCoroutine(_disolveCoroutine);
        }
        _disolveCoroutine = StartCoroutine(Coroutine_DisolveShield(target));
    }

    IEnumerator Coroutine_HitDisplacement()
    {
        float lerp = 0;
        while (lerp < 1)
        {
            _renderer.material.SetFloat("_DisplacementStrength", _DisplacementCurve.Evaluate(lerp) * _DisplacementMagnitude);
            lerp += Time.deltaTime*_LerpSpeed;
            yield return null;
        }
    }

    IEnumerator Coroutine_DisolveShield(float target)
    {
        float start = _renderer.material.GetFloat("_Disolve");
        float lerp = 0;
        while (lerp < 1)
        {
            _renderer.material.SetFloat("_Disolve", Mathf.Lerp(start,target,lerp));
            lerp += Time.deltaTime * _DisolveSpeed;
            yield return null;
        }
    }


}
