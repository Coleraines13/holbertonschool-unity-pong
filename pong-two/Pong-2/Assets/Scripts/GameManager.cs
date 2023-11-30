using System.Collections;
using UnityEngine;

namespace ZPong
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float startDelay = 3f;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private GameObject canvasParent;

        public Ball activeBall;

        public static GameManager Instance { get; private set; }

        private Goal[] goals;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            goals = new Goal[2];
        }

        void SetGame()
        {
            if (activeBall == null)
            {
                activeBall = Instantiate(ballPrefab, Vector3.zero, this.transform.rotation, canvasParent.transform)
                    .GetComponent<Ball>();
                activeBall.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }

            activeBall.SetBallActive(false);
        }

        void StartGame()
        {
            //Debug.Log("Starting game!");
            activeBall.SetBallActive(true);
        }

        public void Reset()
        {
            StartCoroutine(StartTimer());
        }

        private void Start()
        {
            Reset();
        }

        IEnumerator StartTimer()
        {
            SetGame();
            yield return new WaitForSeconds(startDelay);

            SetBounds();

            StartGame();
        }

        void SetBounds()
        {
            activeBall.SetHeightBounds();
            foreach (var g in goals)
            {
                g.SetHeightBounds();
            }
        }

        public void ResetBall()
        {
            StartCoroutine(ResetBallCoroutine());
        }

        private IEnumerator ResetBallCoroutine()
        {
            // Simply reset the ball's position and state instead of destroying it
            activeBall.transform.position = Vector3.zero;
            activeBall.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            activeBall.SetBallActive(false);

            yield return null;

            StartCoroutine(StartTimer());
        }

        public void SetGoalObj(Goal g)
        {
            if (goals[0])
            {
                goals[1] = g;
            }
            else
            {
                goals[0] = g;
            }
        }
    }
}
