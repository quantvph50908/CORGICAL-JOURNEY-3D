using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public GameObject Image_Menu, Image_Help, Image_SelectLevel;
    public Button Bt_Play;
    public Button Bt_Continue;
    public Button Bt_Help;
    public Button Bt_Back;
    public Button Bt_Home;
    void Start()
    {
        Bt_Play.onClick.AddListener(Play);
        Bt_Continue.onClick.AddListener(Continue);
        Bt_Help.onClick.AddListener(Help);
        Bt_Back.onClick.AddListener(Back);
        Bt_Home.onClick.AddListener(Home);
    }

    void ShowImage(GameObject Tager)
    {
        Image_Menu.SetActive(false);
        Image_Help.SetActive(false);
        Image_SelectLevel.SetActive(false);
        Tager.SetActive(true);
    }    
    
    private void Play() => ShowImage(Image_SelectLevel);
    private void Continue() => ShowImage(Image_SelectLevel);
    private void Help() => ShowImage(Image_Help);
    private void Back() => ShowImage(Image_Menu);
    private void Home() => ShowImage(Image_Menu);

        
}
