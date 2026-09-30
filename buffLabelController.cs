using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;




public class buffLabelController : MonoBehaviour
{
    public List<Sprite> labels;


    #region reactiveproperty
    public CompositeDisposable disposable = new CompositeDisposable();
    public BoolReactiveProperty isPulemet = new BoolReactiveProperty();
    public BoolReactiveProperty isLazer = new BoolReactiveProperty();
    public BoolReactiveProperty isRocket = new BoolReactiveProperty();
    #endregion

    private void Awake()
    {
        isPulemet.Subscribe(value => { PulemetImageFilling(value); }).AddTo(disposable);
        isLazer.Subscribe(value => { if (value) { transform.GetComponent<Image>().sprite = labels[1]; AlphaToOne(); } else { transform.GetComponent<Image>().sprite = null; AlphaToZero(); } }).AddTo(disposable);
        isRocket.Subscribe(value => { if (value) { transform.GetComponent<Image>().sprite = labels[2]; AlphaToOne(); } else { transform.GetComponent<Image>().sprite = null; AlphaToZero(); } }).AddTo(disposable);


    }

    private void PulemetImageFilling(bool value)
    {
        Image image = transform.GetComponent<Image>();
        if (value) 
        { 
            image.sprite = labels[0];
            image.type = Image.Type.Filled;
            image.fillAmount = 1f;
            AlphaToOne();
        }
        else
        {
            AlphaToZero();
            transform.GetComponent<Image>().sprite = null;
            image.type = Image.Type.Simple;


        }
    }
    private void AlphaToZero() 
    {
        Image image = transform.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }
    private void AlphaToOne()
    {
        Image image = transform.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }



    private void OnDestroy()
    {
        disposable.Clear();
    }
    private void OnDisable()
    {
        disposable.Clear();
    }

    private void Update()
    {
        if (isPulemet.Value) 
        {

        }
    }

}
