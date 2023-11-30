using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZPong
{

    [RequireComponent(typeof(VolumeControl))]
    public class SettingsMenuUI : MonoBehaviour
    {
        private MenuBall menuBall;
        [SerializeField] private Paddle menuPaddleLeft;
        [SerializeField] private AIPlayer leftAI;
        [SerializeField] private Paddle menuPaddleRight;
        
        [SerializeField] private RectTransform mainMenuUIParent;

        [SerializeField] private Button resetToDefaultsButton;
        [SerializeField] private Button backButton;

        [SerializeField] private Slider scoreToWinSlider;
        [SerializeField] private TMP_Text scoreToWinText;

        [SerializeField] private Slider ballSpeedSlider;
        [SerializeField] private TMP_Text ballSpeedText;
        [SerializeField] private Slider ballSizeSlider;
        [SerializeField] private TMP_Text ballSizeText;

        [SerializeField] private Button[] aiLevelButtons;
        [SerializeField] private Button[] pitchDirectionButtons;

        [SerializeField] private Slider paddleSpeedSlider;
        [SerializeField] private TMP_Text paddleSpeedText;
        [SerializeField] private Slider paddleSizeSlider;
        [SerializeField] private TMP_Text paddleSizeText;

        [SerializeField] private Button[] playerUpInputButtons;
        [SerializeField] private Button[] playerDownInputButtons;
        [SerializeField] private TMP_Text[] playerUpInputTexts;
        [SerializeField] private TMP_Text[] playerDownInputTexts;

        private bool configuringInput; // Flag to indicate if configuring input.
        private int playerIndexToConfigure; // Index of the player being configured.

        private KeyCode[] playerUpInputs = new KeyCode[2]; // Stores player up inputs for two players.
        private KeyCode[] playerDownInputs = new KeyCode[2]; // Stores player down inputs for two players.

        private VolumeControl volumeSettingsController;
        
        //Default values
        private const int dPaddleSize = 200;
        private const int dPaddleSpeed = 500;
        private const int dBallSpeed = 500;
        private const int dBallSize = 25;
        private const int dScoreToWin = 10;
        private const int dAILevel = 0;
        private const KeyCode dPlayerOneUp = KeyCode.W;
        private const KeyCode dPlayerOneDown = KeyCode.S;
        private const KeyCode dPlayerTwoUp = KeyCode.UpArrow;
        private const KeyCode dPlayerTwoDown = KeyCode.DownArrow;
        private const string pitchDirection = "Left";

        [SerializeField] private Color nonSelectedNormalColor = Color.gray;
        [SerializeField] private Color nonSelectedHighlightedColor = Color.gray;

        private void Start()
        {
            // Attach onClick listeners for buttons and sliders to update settings and PlayerPrefs.
            resetToDefaultsButton.onClick.AddListener(ResetToDefaultSettings);
            backButton.onClick.AddListener(Back);
            scoreToWinSlider.onValueChanged.AddListener(UpdateScoreToWin);
            ballSpeedSlider.onValueChanged.AddListener(UpdateBallSpeed);
            ballSizeSlider.onValueChanged.AddListener(UpdateBallSize);
            paddleSizeSlider.onValueChanged.AddListener(UpdatePaddleSize);
            paddleSpeedSlider.onValueChanged.AddListener(UpdatePaddleSpeed);

            // Attach onClick listeners for AI Level and Pitch Direction buttons.
            aiLevelButtons[0].onClick.AddListener(() => SetAILevel(0));
            aiLevelButtons[1].onClick.AddListener(() => SetAILevel(1));
            aiLevelButtons[2].onClick.AddListener(() => SetAILevel(2));

            foreach (var pitchButton in pitchDirectionButtons)
            {
                pitchButton
                    .onClick.AddListener(() => SetPitchDirection(pitchButton.name));
            }

            // Attach onClick listeners for Player Input buttons.
            for (int i = 0; i < 2; i++) // Only two players.
            {
                int playerIndex = i;
                playerUpInputButtons[i].onClick.AddListener(() => ConfigurePlayerInput(playerIndex, true));
                playerDownInputButtons[i].onClick.AddListener(() => ConfigurePlayerInput(playerIndex, false));
            }

            volumeSettingsController = this.GetComponent<VolumeControl>();
        }

        private void OnEnable()
        {
            // Initialize settings based on PlayerPrefs (if previously set).
            InitializeSettings();
            menuBall = GameObject.Find("MenuBall(Clone)").GetComponent<MenuBall>();
        }

        // Implement methods to update settings and PlayerPrefs based on user interactions.

        private void Back()
        {
            mainMenuUIParent.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }

        private void UpdateScoreToWin(float value)
        {
            // Convert the float value to an integer (if needed) and store it in PlayerPrefs.
            int scoreToWin = Mathf.RoundToInt(value);
            PlayerPrefs.SetInt("ScoreToWin", scoreToWin);

            // Update the text field to display the current score.
            scoreToWinText.text = "" + scoreToWin;
        }


        private void UpdateBallSpeed(float value)
        {
            // Store the ball speed in PlayerPrefs.
            PlayerPrefs.SetFloat("BallSpeed", value);

            ballSpeedText.text = "" + value;
            // Update any visual representation of the ball speed (if needed).
            menuBall.GetComponent<Ball>().speed = value;

        }

        private void UpdateBallSize(float value)
        {
            // Store the ball size in PlayerPrefs.
            PlayerPrefs.SetFloat("BallSize", value);
            ballSizeText.text = "" + value;
            // Update any visual representation of the ball size (if needed).
            menuBall.GetComponent<RectTransform>().sizeDelta = new Vector2(value, value);
        }

        private void UpdatePaddleSpeed(float value)
        {
            // Store the ball speed in PlayerPrefs.
            PlayerPrefs.SetFloat("PaddleSpeed", value);

            paddleSpeedText.text = "" + value;
            // Update any visual representation of the ball speed (if needed).
            leftAI.speed = value;
        }

        private void UpdatePaddleSize(float value)
        {
            // Store the ball size in PlayerPrefs.
            PlayerPrefs.SetFloat("PaddleSize", value);
            paddleSizeText.text = "" + value;
            
            // Update any visual representation of the ball size (if needed).
            var paddle = menuPaddleLeft.GetComponent<RectTransform>();
            paddle.sizeDelta = new Vector2(paddle.sizeDelta.x, value);
            paddle.GetComponent<BoxCollider2D>().size = paddle.sizeDelta;
            paddle = menuPaddleRight.GetComponent<RectTransform>();
            paddle.sizeDelta = new Vector2(paddle.sizeDelta.x, value);
            paddle.GetComponent<BoxCollider2D>().size = paddle.sizeDelta;

        }


        private void SetAILevel(int level)
        {
            // Store the selected AI level in PlayerPrefs.
            PlayerPrefs.SetInt("AILevel", level);

            // Highlight the selected AI Level button and deselect others.
            UpdateButtonColors(aiLevelButtons, level);
            leftAI.difficulty = (AILevel)level;
        }


        private void SetPitchDirection(string direction)
        {
            // Store the selected pitch direction in PlayerPrefs.
            PlayerPrefs.SetString("PitchDirection", direction);

            // Highlight the selected Pitch Direction button and deselect others.
            UpdateButtonColors(pitchDirectionButtons, Array.FindIndex(pitchDirectionButtons, button => button.name == direction));
        }

        private void ConfigurePlayerInput(int playerIndex, bool isUpInput)
        {
            configuringInput = true;
            playerIndexToConfigure = playerIndex;

            // Update the UI to indicate that input configuration is active.
            if (isUpInput)
                playerUpInputTexts[playerIndexToConfigure].text = "...";
            else
                playerDownInputTexts[playerIndexToConfigure].text = "...";
        }

        private void UpdateButtonColors(Button[] buttonsToColor, int selectedIndex)
        {
            // Iterate through the provided buttons and update their colors based on the selected index.
            for (int index = 0; index < buttonsToColor.Length; index++)
            {
                bool isSelected = (index == selectedIndex);
                ColorBlock colors = buttonsToColor[index].colors;
                colors.normalColor = isSelected ? Color.white : nonSelectedNormalColor;
                colors.highlightedColor = isSelected ? Color.white : nonSelectedHighlightedColor;
                buttonsToColor[index].colors = colors;
            }
        }



        private void InitializeSettings()
        {
            // Initialize settings based on PlayerPrefs (if previously set).

            // Initialize Score to Win setting.
            if (PlayerPrefs.HasKey("ScoreToWin"))
            {
                int scoreToWinValue = PlayerPrefs.GetInt("ScoreToWin");
                scoreToWinSlider.value = scoreToWinValue;
                scoreToWinText.text = scoreToWinValue.ToString();
            }

            // Initialize Ball Speed setting.
            if (PlayerPrefs.HasKey("BallSpeed"))
            {
                float ballSpeedValue = PlayerPrefs.GetFloat("BallSpeed");
                ballSpeedSlider.value = ballSpeedValue;
                ballSpeedText.text = ballSpeedValue.ToString();
            }

            // Initialize Ball Size setting.
            if (PlayerPrefs.HasKey("BallSize"))
            {
                float ballSizeValue = PlayerPrefs.GetFloat("BallSize");
                ballSizeSlider.value = ballSizeValue;
                ballSizeText.text = ballSizeValue.ToString();
            }

            // Initialize Paddle Speed setting.
            if (PlayerPrefs.HasKey("PaddleSpeed"))
            {
                float paddleSpeedValue = PlayerPrefs.GetFloat("PaddleSpeed");
                paddleSpeedSlider.value = paddleSpeedValue;
                paddleSpeedText.text = paddleSpeedValue.ToString();
            }

            // Initialize Paddle Size setting.
            if (PlayerPrefs.HasKey("PaddleSize"))
            {
                float paddleSizeValue = PlayerPrefs.GetFloat("PaddleSize");
                paddleSizeSlider.value = paddleSizeValue;
                paddleSizeText.text = paddleSizeValue.ToString();
            }

            // Initialize AI Level setting.
            if (PlayerPrefs.HasKey("AILevel"))
            { 
                int aiLevelValue = PlayerPrefs.GetInt("AILevel");
                SetAILevel(aiLevelValue); // Call the SetAILevel method to highlight the correct button.
            }

            // Initialize Pitch Direction setting.
            if (PlayerPrefs.HasKey("PitchDirection"))
            {
                string pitchDirectionValue = PlayerPrefs.GetString("PitchDirection");
                SetPitchDirection(pitchDirectionValue); // Call the SetPitchDirection method to highlight the correct button.
            }

            // Initialize Player Input settings.
            for (int i = 0; i < 2; i++)
            {
                string upKey = "Player" + i + "UpInput";
                string downKey = "Player" + i + "DownInput";

                if (PlayerPrefs.HasKey(upKey) && PlayerPrefs.HasKey(downKey))
                {
                    playerUpInputs[i] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(upKey));
                    playerDownInputs[i] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(downKey));

                    // Update the UI to display the assigned keys.
                    playerUpInputTexts[i].text = GetFormattedInputKey(playerUpInputs[i]);
                    playerDownInputTexts[i].text = GetFormattedInputKey(playerDownInputs[i]);
                }
            }
        }


        private void Update()
        {
            if (configuringInput)
            {
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        // Ensure the pressed key is not already bound to another player.
                        bool isKeyAlreadyBound = IsKeyBoundToOtherPlayer(key);
                        if (!isKeyAlreadyBound)
                        {
                            if (playerIndexToConfigure >= 0 && playerIndexToConfigure < 2)
                            {
                                if (IsUpArrowKey(key))
                                {
                                    playerUpInputs[playerIndexToConfigure] = key;
                                    PlayerPrefs.SetString("Player" + playerIndexToConfigure + "UpInput",
                                        key.ToString());

                                    // Update the UI with the selected key (using "↑" character for up).
                                    playerUpInputTexts[playerIndexToConfigure].text = "↑ " + key;
                                }
                                else if (IsDownArrowKey(key))
                                {
                                    playerDownInputs[playerIndexToConfigure] = key;
                                    PlayerPrefs.SetString("Player" + playerIndexToConfigure + "DownInput",
                                        key.ToString());

                                    // Update the UI with the selected key (using "↓" character for down).
                                    playerDownInputTexts[playerIndexToConfigure].text = "↓ " + key;
                                }
                            }

                            configuringInput = false; // Configuration is complete.
                            break;
                        }
                    }
                }
            }
        }

        [ContextMenu("Reset To Default Settings")]
        private void ResetToDefaultSettings()
        {
            // Reset Score to Win to default.
            scoreToWinSlider.value = dScoreToWin;
            UpdateScoreToWin(dScoreToWin);

            // Reset Ball Speed to default.
            ballSpeedSlider.value = dBallSpeed;
            UpdateBallSpeed(dBallSpeed);

            // Reset Ball Size to default.
            ballSizeSlider.value = dBallSize;
            UpdateBallSize(dBallSize);

            // Reset Paddle Speed to default.
            paddleSpeedSlider.value = dPaddleSpeed;
            UpdatePaddleSpeed(dPaddleSpeed);

            // Reset Paddle Size to default.
            paddleSizeSlider.value = dPaddleSize;
            UpdatePaddleSize(dPaddleSize);

            // Reset AI Level to default.
            SetAILevel(dAILevel);

            // Reset Pitch Direction to default.
            SetPitchDirection(pitchDirection);

            // Reset Player Input settings to default.
            for (int i = 0; i < 2; i++)
            {
                PlayerPrefs.SetString("Player" + i + "UpInput", GetDefaultPlayerUpInput(i).ToString());
                PlayerPrefs.SetString("Player" + i + "DownInput", GetDefaultPlayerDownInput(i).ToString());

                // Update the UI to display the default assigned keys.
                playerUpInputTexts[i].text = GetFormattedInputKey(GetDefaultPlayerUpInput(i));
                playerDownInputTexts[i].text = GetFormattedInputKey(GetDefaultPlayerDownInput(i));
            }
            
            //Reset volume controls
            volumeSettingsController.ResetToDefaultSettings();
        }

        // Define default player up input keys.
        private KeyCode GetDefaultPlayerUpInput(int playerIndex)
        {
            return playerIndex == 0 ? dPlayerOneUp : dPlayerTwoUp;
        }

        // Define default player down input keys.
        private KeyCode GetDefaultPlayerDownInput(int playerIndex)
        {
            return playerIndex == 0 ? dPlayerOneDown : dPlayerTwoDown;
        }


        private bool IsKeyBoundToOtherPlayer(KeyCode key)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i != playerIndexToConfigure)
                {
                    if (playerUpInputs[i] == key || playerDownInputs[i] == key)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsUpArrowKey(KeyCode key)
        {
            return key == KeyCode.UpArrow;
        }

        private bool IsDownArrowKey(KeyCode key)
        {
            return key == KeyCode.DownArrow;
        }

        private string GetFormattedInputKey(KeyCode key)
        {
            if (IsUpArrowKey(key))
            {
                return "↑"; // Use the ↑ character for the up arrow.
            }
            else if (IsDownArrowKey(key))
            {
                return "↓"; // Use the ↓ character for the down arrow.
            }
            else
            {
                return key.ToString(); // Use the key name for other keys.
            }
        }
    }
}