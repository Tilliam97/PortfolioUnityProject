using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Animator CameraObject;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject firstMenu;
    public GameObject playMenu;
    public GameObject exitMenu;
    public GameObject leaderboards;

    [Header("PANELS")]
    public GameObject mainCanvas;
    public GameObject PanelControls;
    public GameObject PanelVideo;
    public GameObject PanelGame;
    public GameObject PanelKeyBindings;
    public GameObject PanelMovement;
    public GameObject PanelCombat;
    public GameObject PanelGeneral;

    [Header("SETTINGS SCREEN")]
    public GameObject lineGame;
    public GameObject lineVideo;
    public GameObject lineControls;
    public GameObject lineKeyBindings;
    public GameObject lineMovement;
    public GameObject lineCombat;
    public GameObject lineGeneral;

    [Header("SFX")]
    public AudioSource hoverSound;
    public AudioSource sliderSound;
    public AudioSource swooshSound;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
