using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class Menus : MonoBehaviour {

    [SerializeField]
    private GameObject mainMenuUI;
    [SerializeField]
    private GameObject gameplayUI;
    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private GameObject gameOverMenuUI;
    [SerializeField]
    private GameObject mainButtons;
    [SerializeField]
    private GameObject levelSelectButtons;
    [SerializeField]
    private GameObject tutorialMenu;
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject tileType;
    [SerializeField]
    private GameObject ballType;
    [SerializeField]
    private Slider audioSlider;
    [SerializeField]
    private MenuTransitionAnimation menuTrasitionAnimation;
    [SerializeField]
    private BallsPathfinder pathFinder;
    private AudioSource buttonSound;

    [SerializeField]
    private Sprite[] balls;
    [SerializeField]
    private Image[] nextWaveBalls;
    [SerializeField]
    private Text score;
    [SerializeField]
    private Text bestScore;

    private BannerView bannerView;
    private InterstitialAd interstitial;

    void Start() {
        buttonSound = GameObject.Find("ButtonSound").GetComponent<AudioSource> ();
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
        RequestInterstitial();
    }

    private void RequestBanner() {

        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request);
        this.bannerView.Hide();
    }

    private void RequestInterstitial() {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
   }

    public void ShowLevelSelectMenu() {
        mainButtons.GetComponent<ZoomOutAnimation> ().enabled = true;
        levelSelectButtons.GetComponent<ZoomInAnimation> ().enabled = true;
        buttonSound.Play();
    }
    
    public void StartTheGame() {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        pathFinder.enabled = true;
        score.text = "SCORE: " + Vars.score;
        if(Vars.currentMode == 0) {
            bestScore.text = "BEST: " + PlayerPrefs.GetInt("BestScore7x7");
        }else if(Vars.currentMode == 1) {
            bestScore.text = "BEST: " + PlayerPrefs.GetInt("BestScore9x9");
        }else if(Vars.currentMode == 2) {
            bestScore.text = "BEST: " + PlayerPrefs.GetInt("BestScore11x11");
        }
        bannerView.Show();
    }

    private void GameStartTransitionAnimation() {
        menuTrasitionAnimation.menu = 1;
        menuTrasitionAnimation.enabled = true; 
        buttonSound.Play();
    }

    public void StartTheGameSmall() {//7x7            // we can change the gride size by changeing only thre number  this for small gride size 
        pathFinder.RowsNumber = 7;
        pathFinder.ColumnsNumber = 7;
        GameStartTransitionAnimation();
        Vars.currentMode = 0;
    }

    public void StartTheGameStandard() {//9x9       // we can change the gride size by changeing only thre number  this for normal gride size 
        pathFinder.RowsNumber = 9;
        pathFinder.ColumnsNumber = 9;
        GameStartTransitionAnimation();
        Vars.currentMode = 1;
    }

    public void StartTheGameLarge() {//11x11
        pathFinder.RowsNumber = 11;
        pathFinder.ColumnsNumber = 11;
        GameStartTransitionAnimation();
        Vars.currentMode = 2;
    }

    public void PauseMenu() {
        buttonSound.Play();
        pauseMenuUI.SetActive(true);

    }

    public void ClosePauseMenu() {
        buttonSound.Play();
        pauseMenuUI.SetActive(false);
    }

    public void GameOverMenu() {
        gameOverMenuUI.SetActive(true);
         if (this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        }
        RequestInterstitial();
    }

    public void RestartTheGame() { 
        pathFinder.enabled = false;
        DestroyAllTiles();
        Vars.ResetAllVariables();
        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
        Invoke("Reset", 0);
    }

    private void Reset() {
        StartTheGame();
    }


    public void GameRestartTransitionAnmation() {
        menuTrasitionAnimation.menu = 2;
        menuTrasitionAnimation.enabled = true;
        buttonSound.Play();
    }

    private void DestroyAllTiles() {
        Transform tilesGameObject = GameObject.Find("Tiles").transform;
        foreach (Transform child in tilesGameObject) {
             GameObject.Destroy(child.gameObject);
        }
    }

    public void BackToTheMainMenu() {
        DestroyAllTiles();
        mainMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        pathFinder.enabled = false;
        Vars.ResetAllVariables();
        mainButtons.transform.localScale = new Vector2(1, 1);
        levelSelectButtons.transform.localScale = new Vector2(0, 0);
        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
         if (this.interstitial.IsLoaded()) {
            this.interstitial.Show();
        }
        RequestInterstitial();
    }

    public void BackToTheMainMenuTrasitionAnimation() {
        menuTrasitionAnimation.menu = 0;
        menuTrasitionAnimation.enabled = true;
        buttonSound.Play();
        bannerView.Hide();
    }

    public void UpdateNextWaveBallsColor(int ball, int color) {
        nextWaveBalls[ball].sprite = balls[color];
    }

    /*public void UpdateScore() {
        score.text = "SCORE: " + Vars.score;
        if(Vars.currentMode == 0) {
            if(Vars.score > PlayerPrefs.GetInt("BestScore7x7")) {
                PlayerPrefs.SetInt("BestScore7x7", Vars.score);
                bestScore.text = "BEST: " + PlayerPrefs.GetInt("BestScore7x7");
            }   
        }else if(Vars.currentMode == 1) {
            if(Vars.score > PlayerPrefs.GetInt("BestScore9x9")) {
                PlayerPrefs.SetInt("BestScore9x9", Vars.score);
                bestScore.text = "BEST: " + PlayerPrefs.GetInt("BestScore9x9");
            } 
        }else if(Vars.currentMode == 2) {
            if(Vars.score > PlayerPrefs.GetInt("BestScore11x11")) {
                PlayerPrefs.SetInt("BestScore11x11", Vars.score);
                bestScore.text = "BEST: " + PlayerPrefs.GetInt("BestScore11x11");
            } 
        }
    }*/

    public void ShowTutorialMenu() {
        tutorialMenu.GetComponent<Image> ().enabled = true;
        tutorialMenu.transform.Find("Tutorial").GetComponent<ZoomInAnimation> ().enabled = true;
        buttonSound.Play();
    }

    public void HideTutorialMenu() {
        tutorialMenu.GetComponent<Image> ().enabled = false;
        tutorialMenu.transform.Find("Tutorial").GetComponent<ZoomOutAnimation> ().enabled = true;
        buttonSound.Play();
    }

    public void ShowSettingsMenu() {
        settingsMenu.GetComponent<Image> ().enabled = true;
        settingsMenu.transform.Find("Settings").GetComponent<ZoomInAnimation> ().enabled = true;

        if(PlayerPrefs.GetInt("TileType") == 0) {
            TileType1();
        }else if(PlayerPrefs.GetInt("TileType") == 1) {
            TileType2();
        }else if(PlayerPrefs.GetInt("TileType") == 2) {
            TileType3();
        }

        if(PlayerPrefs.GetInt("BallType") == 0) {
            BallType1();
        }else if(PlayerPrefs.GetInt("BallType") == 1) {
            BallType2();
        }else if(PlayerPrefs.GetInt("BallType") == 2) {
            BallType3();
        }

        buttonSound.Play();
    }

    /*public void HideSettingsMenu() {
        settingsMenu.GetComponent<Image> ().enabled = false;
        settingsMenu.transform.Find("Settings").GetComponent<ZoomOutAnimation> ().enabled = true;
        buttonSound.Play();
    }*/

    public void Volume() {
        AudioListener.volume = audioSlider.value;
    }

    public void TileType1() {
        PlayerPrefs.SetInt("TileType", 0);
        tileType.transform.Find("Type1Arrow").GetComponent<Image> ().enabled = true;
        tileType.transform.Find("Type2Arrow").GetComponent<Image> ().enabled = false;
        tileType.transform.Find("Type3Arrow").GetComponent<Image> ().enabled = false;
        if(!buttonSound.isPlaying)
            buttonSound.Play();
    }

    public void TileType2() {
        PlayerPrefs.SetInt("TileType", 1);
        tileType.transform.Find("Type1Arrow").GetComponent<Image> ().enabled = false;
        tileType.transform.Find("Type2Arrow").GetComponent<Image> ().enabled = true;
        tileType.transform.Find("Type3Arrow").GetComponent<Image> ().enabled = false;
        if(!buttonSound.isPlaying)
            buttonSound.Play();
    }

    public void TileType3() {
        PlayerPrefs.SetInt("TileType", 2);
        tileType.transform.Find("Type1Arrow").GetComponent<Image> ().enabled = false;
        tileType.transform.Find("Type2Arrow").GetComponent<Image> ().enabled = false;
        tileType.transform.Find("Type3Arrow").GetComponent<Image> ().enabled = true;
        if(!buttonSound.isPlaying)
            buttonSound.Play();
    }

    public void BallType1() {
        PlayerPrefs.SetInt("BallType", 0);
        ballType.transform.Find("Type1Arrow").GetComponent<Image> ().enabled = true;
        ballType.transform.Find("Type2Arrow").GetComponent<Image> ().enabled = false;
        ballType.transform.Find("Type3Arrow").GetComponent<Image> ().enabled = false;
        if(!buttonSound.isPlaying)
            buttonSound.Play();
    }

    public void BallType2() {
        PlayerPrefs.SetInt("BallType", 1);
        ballType.transform.Find("Type1Arrow").GetComponent<Image> ().enabled = false;
        ballType.transform.Find("Type2Arrow").GetComponent<Image> ().enabled = true;
        ballType.transform.Find("Type3Arrow").GetComponent<Image> ().enabled = false;
        if(!buttonSound.isPlaying)
            buttonSound.Play();
    }

    public void BallType3() {
        PlayerPrefs.SetInt("BallType", 2);
        ballType.transform.Find("Type1Arrow").GetComponent<Image> ().enabled = false;
        ballType.transform.Find("Type2Arrow").GetComponent<Image> ().enabled = false;
        ballType.transform.Find("Type3Arrow").GetComponent<Image> ().enabled = true;
        if(!buttonSound.isPlaying)
            buttonSound.Play();
    }
}
