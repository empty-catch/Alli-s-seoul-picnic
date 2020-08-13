#pragma warning disable CS0649

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PicnicHandler : MonoBehaviour
{
    [SerializeField]
    private Information[] infos;

    [Header("Subtitle")]
    [SerializeField]
    private GameObject subtitle;
    [SerializeField]
    private Text subtitleText;

    [Header("Destination")]
    [SerializeField]
    private GameObject destination;
    [SerializeField]
    private Text destinationText;
    [SerializeField]
    private Text destinationCounterText;

    [Header("Question")]
    [SerializeField]
    private GameObject question;
    [SerializeField]
    private GameObject countdown;
    [SerializeField]
    private Graphic[] questionColorObjects;
    [SerializeField]
    private Text questionLineText;
    [SerializeField]
    private Text[] questionStationTexts;
    [SerializeField]
    private Text questionCountdownText;

    [Header("Answer")]
    [SerializeField]
    private GameObject answer;
    [SerializeField]
    private Text answerText;
    [SerializeField]
    private Text answerCounterText;
    [SerializeField]
    private Text[] answerExampleTexts;
    [SerializeField]
    private Graphic[] answerColorObjects;
    [SerializeField]
    private Text answerLineText;
    [SerializeField]
    private Transform answerQuestion;
    [SerializeField]
    private Text[] answerStationTexts;
    [SerializeField]
    private Text answerQuestionText;

    [Header("Ending")]
    [SerializeField]
    private GameObject ending;
    [SerializeField]
    private Graphic[] endingPlaces;

    private bool isCurrect;
    private Information info;
    private List<Information> successInfos = new List<Information>();

    public void LoadMain()
    {
        PlayerPrefs.SetInt("Retry", 0);
        SceneManager.LoadScene("MainScene");
    }

    public void LoadRetry()
    {
        PlayerPrefs.SetInt("Retry", 1);
        SceneManager.LoadScene("MainScene");
    }

    public void Solve(Text text)
    {
        StartCoroutine(SolveCoroutine(text));
    }

    private IEnumerator SolveCoroutine(Text text)
    {
        var result = text.text == info.Answer;
        var sign = text.transform.GetChild(System.Convert.ToInt32(result));

        if (result)
        {
            sign.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            answerQuestionText.text = info.Answer;
            yield return new WaitForSeconds(1f);
            sign.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            sign.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            sign.gameObject.SetActive(false);
        }

        isCurrect = result;
    }

    private IEnumerator Start()
    {
        int picnicCount = PlayerPrefs.GetInt("Difficulty", 3);
        int lastInfoIndex = -1;
        List<int> infoIndices = null;

        for (int current = 1; current <= picnicCount; current++)
        {
            if (infoIndices == null || infoIndices.Count == 0)
            {
                infoIndices = Enumerable.Range(0, infos.Length)
                             .OrderBy(_ => Random.Range(0, infos.Length))
                             .SkipWhile(key => key == lastInfoIndex).ToList();
            }

            lastInfoIndex = infoIndices[infoIndices.Count - 1];
            info = infos[lastInfoIndex];
            successInfos.Add(info);
            infoIndices.RemoveAt(lastInfoIndex);

            subtitle.SetActive(true);
            {
                subtitleText.text = $"{NumberToHangul(current)}번째 나들이";
                yield return new WaitForSeconds(3f);
            }
            subtitle.SetActive(false);

            destination.SetActive(true);
            {
                destinationText.text = $"알리가 {info.Destination.Replace("\\n", string.Empty)}에 가려고 합니다.\n목적지에 가기 위해 거쳐야하는 역을 기억해주세요.";
                destinationCounterText.text = $"나들이 장소 {current}/{picnicCount}";
                yield return new WaitForSeconds(3f);

                question.SetActive(true);
                {
                    foreach (var colorObject in questionColorObjects)
                    {
                        colorObject.color = info.LineColor;
                    }
                    questionLineText.text = $"{info.LineNumber}호선";
                    for (int i = 0; i < questionStationTexts.Length; i++)
                    {
                        questionStationTexts[i].text = info.Stations[i].Replace("\\n", "\n");
                        if (questionStationTexts[i].text.Contains("\n"))
                        {
                            questionStationTexts[i].fontSize = 38;
                        }
                        else
                        {
                            questionStationTexts[i].fontSize = 46;
                        }
                    }

                    for (int i = 5; i > 0; i--)
                    {
                        questionCountdownText.text = i.ToString();
                        yield return new WaitForSeconds(1f);
                    }
                }
                question.SetActive(false);
            }
            destination.SetActive(false);

            answer.SetActive(true);
            {
                answerText.text = $"알리가 {info.Destination.Replace("\\n", string.Empty)}에 가기 위해 거쳐야하는 역은 어느 역인가요?";
                answerCounterText.text = $"나들이 장소 {current}/{picnicCount}";
                answerQuestionText.text = "?";

                var examples = new List<string>(info.Examples);
                foreach (var exampleText in answerExampleTexts)
                {
                    int index = Random.Range(0, examples.Count);
                    exampleText.text = examples[index];
                    examples.RemoveAt(index);
                }

                foreach (var colorObject in answerColorObjects)
                {
                    colorObject.color = info.LineColor;
                }
                answerLineText.text = $"{info.LineNumber}호선";
                answerQuestion.SetSiblingIndex(info.AnswerIndex);

                int offset = 0;
                for (int i = 0; i < answerStationTexts.Length; i++)
                {
                    if (i == info.AnswerIndex)
                    {
                        offset++;
                    }
                    answerStationTexts[i].text = info.Stations[i + offset].Replace("\\n", "\n");
                    if (answerStationTexts[i].text.Contains("\n"))
                    {
                        answerStationTexts[i].fontSize = 32;
                    }
                    else
                    {
                        answerStationTexts[i].fontSize = 40;
                    }
                }

                isCurrect = false;
                yield return new WaitUntil(() => isCurrect);
            }
            answer.SetActive(false);
        }

        ending.SetActive(true);
        for (int i = 0; i < successInfos.Count; i++)
        {
            var place = endingPlaces[i];
            var info = successInfos[i];

            place.gameObject.SetActive(true);
            place.color = info.LineColor;
            place.transform.GetChild(0).GetComponent<Text>().text = info.Destination.Replace("\\n", string.Empty);
            place.transform.GetChild(1).GetComponent<Text>().text = $"{info.LineNumber}호선";
        }
    }

    private string NumberToHangul(int number)
    {
        switch (number)
        {
            case 1:
                return "첫";
            case 2:
                return "두";
            case 3:
                return "세";
            case 4:
                return "네";
            case 5:
                return "다섯";
            case 6:
                return "여섯";
            case 7:
                return "일곱";
        }
        throw new System.ArgumentOutOfRangeException();
    }
}
