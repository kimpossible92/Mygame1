using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenOrient : MonoBehaviour
{
    public DeviceOrientation deviceOrientation;//в инспекторе показвыает текущую ориентацию

    //анимация панелей, размеры и позиции при изменении ориентации экрана
    public GameObject panelUp;
    public GameObject UpPanelAnim;
    public GameObject DownPanelAnim;
    public GameObject RightPanelAnim;
    public GameObject LeftPanelAnim;
    public GameObject panelDown;
    public GameObject panelLeft;
    public GameObject panelRight;
    public GameObject UIparent;
    public GameObject UIparentLeft;
    public GameObject parentTargets;
    public GameObject starsUp;
    public GameObject upBar;
    public GameObject leftBar;
    public GameObject GemsShop;
    public GameObject otherTarget;             //доп цель, для квалификации в турнире
    public GameObject ingr1;

    public GameObject[] rotateOff;             //кнопка отключения автоповорота в меню паузы и в меню settings на карте
    public GameObject[] menuScale;             //размер меню при повороте экрана
    public GameObject[] imagesMenuLose;        //другие элементы меню, которые не совпадают с меню выше, имеют другие размеры.
    public GameObject[] locks;

    public Transform scaleBoard;               //размер и позиция игровой доски, изменяется при повороте экрана

    public static int lvl;                     //исходя из количества колонок и строк, присватвается что-то вроде вида уровня. LevelManager принимает в case.
    int rotationIndex = 0;                     //0-portrait, 1 - landscapeLeft, 2 -landscapeRight;

    public static bool landscape;              //поворот экрана
    public bool autorotate;                    //автоповорот
    [SerializeField] CanvasScaler canvasScalerGlobal;
    [SerializeField]
    CanvasScaler canvasScalerGlobal2;
    Animator anim, anim2, animRight, animLeft; //аниматоры для панелек в игре при повороте экрана.
    Coroutine coroutine = null;                //задержка при повороте экрана.
    //[SerializeField] AnimationManager[] animationManagers;
    //[SerializeField] Image MenuPlayImage;
    IEnumerator Rotator()                      //задержка при повороте экрана.
    {
        yield return new WaitForSeconds(0.8f);
        coroutine = null;
    }

    void Start()
    {
    }

    void Update()
    {
        Menu1();
    }
    void Menu1()
    {
        LoadMenu123();
    }
    #region Menu2
    void Menu2()
	{        //ingr1.transform.localPosition = new Vector2(-50f, transform.localPosition.y);

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (landscape)
                landscape = false;

            else
                landscape = true;
        }

        if (coroutine == null)
        {
            coroutine = StartCoroutine(Rotator());
            ChangeOrientation(); //задержка при повороте экрана.
        }
        Scaler();
        PanelPos();
        CameraSize();
        LoadMenu123();
	}
    void LoadMenu123()
    {
        //animationManagers = GameObject.FindObjectsOfType<AnimationManager>();
        float aspect = (float)Screen.height / (float)Screen.width;//2.1.4
        aspect = (float)Math.Round(aspect, 2);
        if (landscape)
        {
            //GetComponent<Camera>().orthographicSize = 7.5f;
        }

        else //Portrait
        {
            //print(aspect);
            if (aspect == 1.6f)
            {
                canvasScalerGlobal.referenceResolution = new Vector2(600, 800);
                canvasScalerGlobal2.referenceResolution = new Vector2(600, 800);
                GetComponent<Camera>().orthographicSize = 6;    //16:10
            }
            else if (aspect == 1.78f)
            {
                canvasScalerGlobal.referenceResolution = new Vector2(600, 800);
                canvasScalerGlobal2.referenceResolution = new Vector2(600, 800);
                GetComponent<Camera>().orthographicSize = 6;    //16:9
            }
            else if (aspect == 1.5f)
            {
                canvasScalerGlobal.referenceResolution = new Vector2(600, 800);
                canvasScalerGlobal2.referenceResolution = new Vector2(600, 800);
                GetComponent<Camera>().orthographicSize = 5.8f;    //3:2
            }
            else if (aspect == 1.33f)
            {
                canvasScalerGlobal.referenceResolution = new Vector2(600, 800);
                canvasScalerGlobal2.referenceResolution = new Vector2(600, 800);
                GetComponent<Camera>().orthographicSize = 6f;       //4:3
            }
            else if (aspect == 1.67f)
            {
                canvasScalerGlobal.referenceResolution = new Vector2(600, 800);
                canvasScalerGlobal2.referenceResolution = new Vector2(600, 800);
                GetComponent<Camera>().orthographicSize = 5.65f;        //5:3
            }
            else if (aspect == 1.25f)
            {
                canvasScalerGlobal.referenceResolution = new Vector2(600, 800);
                canvasScalerGlobal2.referenceResolution = new Vector2(600, 800);
                GetComponent<Camera>().orthographicSize = 5.65f;      //5:4
            }
            else if (aspect == 2f)
            {
                canvasScalerGlobal.referenceResolution = new Vector2(600, 800);
                canvasScalerGlobal2.referenceResolution = new Vector2(600, 800);
                GetComponent<Camera>().orthographicSize = 5.65f;    //18:9
            }
            else if (aspect == 2.06f)
            {
                canvasScalerGlobal.referenceResolution = new Vector2(600, 800);//1440x2960
                canvasScalerGlobal2.referenceResolution = new Vector2(600, 800);//1440x2960
            }
            else if (aspect == 2.17f)//|| aspect == 2.1666f)
            {
                canvasScalerGlobal.referenceResolution = new Vector2(720, 1200);//720x1560
                canvasScalerGlobal2.referenceResolution = new Vector2(720, 1200);//720x1560
            }
            else
            {
                GetComponent<Camera>().orthographicSize = 6f;
            }
            //GetComponent<Camera>().GetComponent<MapCamera>().SetPosition(new Vector2(0, GetComponent<Camera>().transform.position.y));
        }
    }
    void CameraSize() //Orhtographic Size of Main Camera
    {
        float aspect = (float)Screen.height / (float)Screen.width;//2.1.4
        aspect = (float)Math.Round(aspect, 2); //окгругленное до двух чисел экранное соотношение
        if (GameObject.Find("isPlay"))  //Если игра идет
        {
            if (landscape)
            {
                GetComponent<Camera>().orthographicSize = 7.5f;
            }

            else //Portrait
            {
                if (aspect == 1.6f)
                    GetComponent<Camera>().orthographicSize = 7.22f;    //16:10
                else if (aspect == 1.78f)
                    GetComponent<Camera>().orthographicSize = 6.92f;    //16:9
                else if (aspect == 1.5f)
                    GetComponent<Camera>().orthographicSize = 7.54f;    //3:2
                else if (aspect == 1.33f)
                    GetComponent<Camera>().orthographicSize = 8f;       //4:3
                else if (aspect == 1.67f)
                    GetComponent<Camera>().orthographicSize = 7f;       //5:3
                else if (aspect == 1.25f)
                    GetComponent<Camera>().orthographicSize = 8.5f;     //5:4
                else if (aspect == 2f)
                    GetComponent<Camera>().orthographicSize = 7.42f;    //18:9
                else if (aspect == 2.06f)
                    GetComponent<Camera>().orthographicSize = 7.6f;     //2960:1440
                else if (aspect == 2.17f)
                    GetComponent<Camera>().orthographicSize = 8f;       //iphone x
                else GetComponent<Camera>().orthographicSize = 6.92f;

                //GetComponent<Camera>().GetComponent<MapCamera>().SetPosition(new Vector2(0, GetComponent<Camera>().transform.position.y));
            }
        }
        if (GameObject.Find("isMap"))   //карта мира, камера меняется
        {
            if (landscape)
            {
                GameObject.Find("LevelsMap").transform.position = new Vector3(-5f, 0f); //перемещаем карту в центр
                if (aspect == 0.56f)
                    GetComponent<Camera>().orthographicSize = 5f;       //16:9
                else if (aspect == 0.49f)
                    GetComponent<Camera>().orthographicSize = 4.35f;    //2960:1440
                else if (aspect == 0.5f)
                    GetComponent<Camera>().orthographicSize = 4.5f;     //18:9
                else if (aspect == 0.6f)
                    GetComponent<Camera>().orthographicSize = 5.38f;    //5:3
                else if (aspect == 0.63f || aspect == 0.62f)
                    GetComponent<Camera>().orthographicSize = 5.6f;     //16:10
                else if (aspect == 0.67f)
                    GetComponent<Camera>().orthographicSize = 5.97f;    //2:3
                else if (aspect == 0.75f)
                    GetComponent<Camera>().orthographicSize = 6.73f;    //3:4
                else if (aspect == 0.8f)
                    GetComponent<Camera>().orthographicSize = 7.18f;    //4:5
                else if (aspect == 0.46f)
                    GetComponent<Camera>().orthographicSize = 4.14f;    //IphoneX
                else GetComponent<Camera>().orthographicSize = 5f;
            }

            else //portrait
            {
                GameObject.Find("LevelsMap").transform.position = new Vector3(0f, 0f); //обратно на место
                if (aspect == 1.6f)
                    GetComponent<Camera>().orthographicSize = 6.25f;                 //16:10
                else if (aspect == 1.78f)
                    GetComponent<Camera>().orthographicSize = 7.12f;                 //16:9
                else if (aspect == 1.5f)
                    GetComponent<Camera>().orthographicSize = 5.9f;                  //3:2
                else if (aspect == 1.33f)
                    GetComponent<Camera>().orthographicSize = 5.25f;                 //4:3
                else if (aspect == 1.67f)
                    GetComponent<Camera>().orthographicSize = 6.6f;                  //5:3
                else if (aspect == 1.25f)
                    GetComponent<Camera>().orthographicSize = 4.9f;                  //5:4
                else if (aspect == 2f)
                    GetComponent<Camera>().orthographicSize = 8f;                    //18:9
                else if (aspect == 2.06f)
                    GetComponent<Camera>().orthographicSize = 8.2f;                  //2960:1440
                else if (aspect == 2.17f)
                    GetComponent<Camera>().orthographicSize = 8.7f;                  //iphone x
                else GetComponent<Camera>().orthographicSize = 7.12f;

                //GetComponent<Camera>().GetComponent<MapCamera>().SetPosition(new Vector2(0, GetComponent<Camera>().transform.position.y));
            }
        }
    }
    public void Autorotate()            //вкл/выкл автоповорот
    {
        if (autorotate)
        {
            autorotate = false;
            foreach (GameObject obj in rotateOff)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            autorotate = true;
            foreach (GameObject obj in rotateOff)
            {
                obj.SetActive(false);
            }
        }
    }
    #endregion
    #region changeOrientation
    void ChangeOrientation()//если экран лицом вверх или вниз
    {
        if (autorotate)
        {
            //если экран лицом вверх или вниз
            if ((Input.deviceOrientation == DeviceOrientation.FaceUp ||
                 Input.deviceOrientation == DeviceOrientation.FaceDown) && !landscape ||
                (Input.deviceOrientation == DeviceOrientation.Portrait ||
                 Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown))
            {
                Screen.orientation = UnityEngine.ScreenOrientation.Portrait;//всегда портретный
                landscape = false;
                rotationIndex = 0;
            }

            else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft ||
                    (Input.deviceOrientation == DeviceOrientation.FaceUp && landscape && Input.deviceOrientation == DeviceOrientation.LandscapeLeft))
            {
                Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
                landscape = true;
                rotationIndex = 1;
            }

            else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight || (Input.deviceOrientation == DeviceOrientation.FaceUp && landscape && Input.deviceOrientation == DeviceOrientation.LandscapeRight))
            {
                Screen.orientation = UnityEngine.ScreenOrientation.LandscapeRight;
                landscape = true;
                rotationIndex = 2;
            }
        }
        else
        {
            if (rotationIndex == 0)
            {
                Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
                landscape = false;
            }
            else if (rotationIndex == 1)
            {
                Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
                landscape = true;
            }
            else if (rotationIndex == 2)
            {
                Screen.orientation = UnityEngine.ScreenOrientation.LandscapeRight;
                landscape = true;
            }
            else
            {
                Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
                landscape = false;
            }
        }
    }
    #endregion
    #region scaler1
    void Scaler() //подстраивает позицию игровой доски, исходя из размеров cols rows
    {
        //ГОРИЗОНТАЛЬНАЯ (альбомная) ОРИЕНТАЦИЯ
        if (landscape)
        {
            
        }

        //ВЕРТИКАЛЬНАЯ (портретная) ОРИЕНТАЦИЯ
        else
        {
            
        }
    }
    #endregion
    #region panel1
    void PanelPos() //верхние, нижние и 2 боковые панели во время игры при повороте экрана. Размеры, анимации, позиции.
    {
        if (landscape)
        {
            GemsShop.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
            panelUp.SetActive(false);          //
            panelDown.SetActive(false);       //
            panelLeft.SetActive(true);       //  ПАНЕЛИ, которые переключаются при повороте экрана
            panelRight.SetActive(true);     //
            UIparent.SetActive(false);     //
            UIparentLeft.SetActive(true); //

            foreach (GameObject menu in menuScale)
            {
                menu.transform.localScale = new Vector3(1.5f, 1.5f, transform.localScale.z);
            }

            if (GameObject.Find("isPlay"))
            {
                otherTarget.transform.localPosition = new Vector3(-1.33f, -538.5f);
                otherTarget.transform.localScale = new Vector3(1.5f, 1.5f);
                anim.SetInteger("UpDown", 0);      //анимация верхней панели сверху внизззз    
                if (GameObject.FindGameObjectWithTag("Panel"))
                    anim2.SetInteger("Down", 0);   //анимация нижней  панели снизу  вверххх
                animLeft.SetInteger("Left", 1);    //анимация левой   панели слева  направо
                animRight.SetInteger("Right", 1);  //анимация правой  панели справа налевоо
            }
            upBar.transform.position = leftBar.transform.position;
            upBar.transform.localScale = new Vector3(0.51f, 0.51f, transform.localScale.z);

            if (GameObject.Find("ParentTarget"))
            {
                parentTargets.transform.position = GameObject.Find("ParentTarget").transform.position;
                parentTargets.transform.localScale = new Vector3(0.35f, 0.35f, transform.localScale.z);
            }

            if (GameObject.Find("StarsLeftPos"))
            {
                starsUp.transform.position = GameObject.Find("StarsLeftPos").transform.position;
                starsUp.transform.localScale = new Vector3(1f, 1f, transform.localScale.z);
            }
        }

        else
        {
            panelUp.SetActive(true);
            panelDown.SetActive(true);
            panelLeft.SetActive(false);
            panelRight.SetActive(false);
            UIparent.SetActive(true);
            UIparentLeft.SetActive(false);

            foreach (GameObject menu in menuScale) menu.transform.localScale = new Vector3(1f, 1f, transform.localScale.z);
            GemsShop.transform.localScale = new Vector3(1f, 1f, 1f);

            if (GameObject.Find("isPlay"))
            { 
                otherTarget.transform.localPosition = new Vector3(193f, -143f);
                otherTarget.transform.localScale = new Vector3(1f, 1f);
                anim.SetInteger("UpDown", 1);
                anim2.SetInteger("Down", 1);
                if (GameObject.FindGameObjectWithTag("Panel"))
                {
                    animLeft.SetInteger("Left", 0);
                }

                if (GameObject.FindGameObjectWithTag("PanelLand")) animRight.SetInteger("Right", 0);
            }


            if (GameObject.Find("TempPos"))
            {
                upBar.transform.position = GameObject.Find("TempPos").transform.position;
                upBar.transform.localScale = GameObject.Find("TempPos").transform.localScale;
            }

            if (GameObject.Find("TargTempPos"))
            {
                parentTargets.transform.position = GameObject.Find("TargTempPos").transform.position;
                parentTargets.transform.localScale = new Vector3(1f, 1f, transform.localScale.z);
            }

            if (GameObject.Find("starsUpPos"))
            {
                starsUp.transform.position = GameObject.Find("starsUpPos").transform.position;
                starsUp.transform.localScale = new Vector3(2f, 2f, transform.localScale.z);
            }
        }
    }
    #endregion
}