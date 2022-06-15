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
        bestScore.text = "BEST: " + PlayerPrefs.GetInt($"BestScore{Vars.currentMode}x{Vars.currentMode}" );

        bannerView.Show();
    }

    private void GameStartTransitionAnimation() {
        menuTrasitionAnimation.menu = 1;
        menuTrasitionAnimation.enabled = true; 
        buttonSound.Play();
    }

    public void StartTheGameSmall()
    {
        StartTheGame(7);
    }
    public void StartTheGameStandard()
    {
        StartTheGame(9);
    }

    public void StartTheGameLarge()
    {
        StartTheGame(11);
    }

    public void StartTheGame(int size)
    {
        pathFinder.RowsNumber = size;
        pathFinder.ColumnsNumber = size;
        GameStartTransitionAnimation();
        Vars.currentMode = size;
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

    public void UpdateScore() {
        var scoreKey = $"BestScore{Vars.currentMode}x{Vars.currentMode}";
        score.text = "SCORE: " + Vars.score;
        if (Vars.score > PlayerPrefs.GetInt(scoreKey))
        {
            PlayerPrefs.SetInt(scoreKey, Vars.score);
            bestScore.text = "BEST: " + PlayerPrefs.GetInt(scoreKey);
        }
    }

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
