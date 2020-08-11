#pragma warning disable CS0649

using UnityEngine;

[CreateAssetMenu(fileName = "Information", menuName = "Information")]
public class Information : ScriptableObject
{
    [SerializeField]
    private int lineNumber;
    [SerializeField]
    private Color lineColor = Color.white;
    [SerializeField]
    private int answer;
    [SerializeField]
    private string[] stations = new string[6];
    [SerializeField]
    private string[] examples = new string[5];

    public string[] Stations => stations;
    public string[] Examples => examples;
    public string Destination => stations[stations.Length - 1];
    public string Answer => stations[answer];
    public int AnswerIndex => answer;
    public int LineNumber => lineNumber;
    public Color LineColor => lineColor;
}
