using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

public class RecordedKeysScript : MonoBehaviour
{
    public KMAudio Audio;
    public KMBombInfo bomb;
    public KMColorblindMode ColorblindMode;

    public List<KMSelectable> keys;
    public Renderer[] meter;
    public Renderer[] keyID;
    public Material[] keyColours;

    private readonly int[][][] table = new int[2][][] {
        new int[21][] { new int[21] { 4, 0, 5, 1, 3, 2, 0, 5, 4, 2, 1, 3, 5, 4, 0, 3, 2, 1, 0, 4, 5},
                        new int[21] { 2, 4, 1, 0, 5, 3, 1, 4, 2, 3, 5, 0, 2, 1, 4, 0, 5, 3, 4, 2, 1},
                        new int[21] { 5, 3, 2, 4, 1, 0, 5, 2, 3, 0, 4, 1, 3, 5, 2, 1, 0, 4, 2, 5, 3},
                        new int[21] { 0, 1, 3, 5, 2, 4, 3, 0, 1, 5, 2, 4, 0, 3, 1, 2, 4, 5, 3, 1, 0},
                        new int[21] { 1, 2, 0, 3, 4, 5, 2, 1, 0, 4, 3, 5, 1, 0, 2, 5, 3, 4, 2, 0, 1},
                        new int[21] { 3, 5, 4, 2, 0, 1, 4, 3, 5, 1, 0, 2, 4, 3, 5, 2, 1, 0, 5, 3, 4},
                        new int[21] { 5, 0, 1, 4, 3, 2, 0, 5, 1, 2, 4, 3, 5, 1, 0, 3, 4, 2, 1, 0, 5},
                        new int[21] { 4, 3, 2, 1, 5, 0, 2, 4, 3, 0, 5, 1, 2, 4, 3, 1, 0, 5, 4, 2, 3},
                        new int[21] { 2, 4, 5, 0, 1, 3, 5, 2, 4, 3, 1, 0, 4, 5, 2, 0, 3, 1, 2, 5, 4},
                        new int[21] { 1, 2, 3, 5, 0, 4, 3, 1, 2, 4, 0, 5, 3, 2, 1, 4, 5, 0, 3, 1, 2},
                        new int[21] { 3, 1, 0, 2, 4, 5, 1, 3, 0, 5, 2, 4, 1, 3, 0, 5, 4, 2, 0, 3, 1},
                        new int[21] { 0, 5, 4, 3, 2, 1, 4, 0, 5, 1, 3, 2, 0, 4, 5, 2, 1, 3, 5, 4, 0},
                        new int[21] { 2, 4, 3, 5, 1, 0, 2, 4, 3, 0, 5, 1, 2, 3, 4, 1, 0, 5, 4, 2, 3},
                        new int[21] { 4, 0, 2, 1, 5, 3, 0, 2, 4, 3, 1, 5, 4, 0, 2, 3, 5, 1, 2, 0, 4},
                        new int[21] { 5, 3, 1, 0, 4, 2, 5, 3, 1, 2, 4, 0, 3, 5, 1, 0, 2, 4, 1, 3, 5},
                        new int[21] { 3, 1, 5, 2, 0, 4, 3, 1, 5, 4, 0, 2, 1, 3, 5, 2, 4, 0, 5, 1, 3},
                        new int[21] { 1, 3, 4, 5, 2, 0, 1, 4, 3, 0, 2, 5, 4, 1, 3, 5, 0, 2, 3, 4, 1},
                        new int[21] { 0, 2, 3, 4, 1, 5, 2, 3, 0, 1, 5, 4, 3, 0, 2, 4, 1, 5, 0, 2, 3},
                        new int[21] { 2, 5, 0, 1, 4, 3, 0, 5, 2, 3, 4, 1, 5, 2, 0, 1, 3, 4, 2, 0, 5},
                        new int[21] { 5, 4, 1, 0, 3, 2, 4, 1, 5, 2, 3, 0, 1, 5, 4, 0, 2, 3, 1, 5, 4},
                        new int[21] { 4, 0, 2, 3, 5, 1, 2, 0, 4, 5, 1, 3, 0, 4, 2, 3, 5, 1, 4, 2, 0} },

        new int[12][] { new int[20] { 1, 6, 4, 5, 3, 2, 4, 1, 5, 6, 4, 2, 3, 1, 5, 2, 6, 4, 1, 3 },
                        new int[20] { 3, 2, 1, 6, 4, 5, 6, 3, 1, 2, 3, 4, 5, 6, 3, 4, 2, 1, 6, 5 },
                        new int[20] { 5, 3, 6, 2, 1, 4, 5, 2, 6, 3, 2, 5, 1, 4, 2, 5, 3, 6, 4, 1 },
                        new int[20] { 2, 4, 5, 3, 6, 1, 3, 4, 2, 5, 1, 6, 4, 3, 6, 1, 5, 2, 3, 4 },
                        new int[20] { 4, 5, 3, 1, 2, 6, 2, 5, 3, 4, 6, 1, 2, 5, 4, 6, 1, 3, 5, 2 },
                        new int[20] { 6, 1, 2, 4, 5, 3, 1, 6, 4, 1, 5, 3, 6, 2, 1, 3, 4, 5, 2, 6 },
                        new int[20] { 4, 2, 3, 6, 1, 5, 3, 4, 6, 2, 3, 1, 4, 5, 2, 1, 6, 3, 5, 4 },
                        new int[20] { 1, 5, 4, 2, 6, 3, 4, 5, 2, 1, 4, 5, 3, 6, 1, 5, 4, 2, 6, 3 },
                        new int[20] { 5, 3, 2, 1, 4, 6, 5, 2, 1, 3, 2, 6, 5, 4, 6, 3, 2, 1, 4, 5 },
                        new int[20] { 2, 6, 1, 3, 5, 4, 1, 3, 2, 6, 5, 4, 1, 3, 4, 2, 5, 6, 3, 1 },
                        new int[20] { 6, 1, 5, 4, 3, 2, 6, 1, 4, 5, 3, 2, 6, 2, 3, 4, 1, 5, 2, 6 },
                        new int[20] { 3, 4, 6, 5, 2, 1, 2, 6, 3, 4, 6, 3, 2, 1, 5, 6, 3, 4, 1, 2 } } };

   
    private readonly string[] colourList = new string[6] { "Red", "Green", "Blue", "Cyan", "Magenta", "Yellow" };
    private string[][] soundList = new string[2][] { new string[6] { "Glockenspiel1", "Glockenspiel2", "Glockenspiel3", "Glockenspiel4", "Glockenspiel5", "Glockenspiel6" }, new string[6] };
    private int[][] info = new int[6][] { new int[4], new int[4], new int[4], new int[4], new int[4], new int[4] };
    private int initialValue;
    private int stage = 1;
    private int pressCount;
    private int swapCount;
    private int resetCount;
    private IEnumerator[] sequence = new IEnumerator[2];
    private bool pressable;
    private bool inputMode;
    private int choose;
    private bool[] alreadypressed = new bool[6] { true, true, true, true, true, true };
    private string[] answer = new string[6];
    private List<string> labelList = new List<string> { };
    private bool colorblind;

    //Logging
    static int moduleCounter = 1;
    int moduleID;
    private bool moduleSolved;

    private void Awake()
    {
        moduleID = moduleCounter++;
        sequence[0] = Shuff();

        foreach (Renderer m in meter)
        {
            m.material = keyColours[8];
        }
        foreach (KMSelectable key in keys)
        {
            key.OnInteract += delegate () { KeyPress(key); return false; };
        }
    }

    void Start()
    {
        colorblind = ColorblindMode.ColorblindModeActive;
        Reset();
    }

    private void KeyPress(KMSelectable key)
    {
        int k = keys.IndexOf(key);
        if (k == 6 && moduleSolved == false && pressable == true)
        {
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            key.AddInteractionPunch();
            if (inputMode == false)
            {
                inputMode = true;
            }
            else
            {
                string[] input = new string[6];
                List<string> I = new List<string> { };
                for(int i = 0; i < 6; i++)
                {
                    if (alreadypressed[i] == false)
                    { 
                       input[i] = "0";                      
                    }
                    else
                    {
                       input[i] = "1";
                       I.Add((i + 1).ToString());       
                    }
                }
                string ans = String.Join(String.Empty, answer);
                string inp = String.Join(String.Empty, input);
                Debug.Log(inp);
                Debug.Log(String.Join(String.Empty, I.ToArray()));
                         
                if(ans != inp)
                {
                    inputMode = false;
                    resetCount++;
                    GetComponent<KMBombModule>().HandleStrike();
                }                
                else
                {
                    meter[stage - 1].material = keyColours[9];
                    GetComponent<KMAudio>().PlaySoundAtTransform("InputCorrect", transform);
                    if (stage < 2)
                    {
                        stage++;
                    }
                    else
                    {
                        moduleSolved = true;
                        StartCoroutine(sequence[0]);
                    }
                }
            }
            Reset();
        }
        else if (alreadypressed[k] == false && moduleSolved == false && pressable == true)
        {
            if (inputMode == true)
            {
                GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
                key.AddInteractionPunch();
                alreadypressed[k] = true;
                key.transform.localPosition = new Vector3(0, 0, -1f);
            }
            else
            {
                GetComponent<KMAudio>().PlaySoundAtTransform(soundList[1][k], transform);
            }
        }
        else if(moduleSolved == true)
        {
            GetComponent<KMAudio>().PlaySoundAtTransform(soundList[0][k], transform);
        }
    }

    private void setKey(int keyIndex)
    {
        if (inputMode == false)
        {
            keyID[keyIndex].material = keyColours[info[keyIndex][0]];
            switch (info[keyIndex][2])
            {
                case 0:
                    keys[keyIndex].GetComponentInChildren<TextMesh>().color = new Color32(255, 25, 25, 255);
                    break;
                case 1:
                    keys[keyIndex].GetComponentInChildren<TextMesh>().color = new Color32(25, 255, 25, 255);
                    break;
                case 2:
                    keys[keyIndex].GetComponentInChildren<TextMesh>().color = new Color32(25, 25, 255, 255);
                    break;
                case 3:
                    keys[keyIndex].GetComponentInChildren<TextMesh>().color = new Color32(25, 255, 255, 255);
                    break;
                case 4:
                    keys[keyIndex].GetComponentInChildren<TextMesh>().color = new Color32(255, 75, 255, 255);
                    break;
                default:
                    keys[keyIndex].GetComponentInChildren<TextMesh>().color = new Color32(255, 255, 75, 255);
                    break;
            }
            var label = (info[keyIndex][1] + 1).ToString();
            if (colorblind)
                label += "\n" + "RGBCMY"[info[keyIndex][2]] + "\n\n" + "RGBCMY"[info[keyIndex][0]];
            keys[keyIndex].GetComponentInChildren<TextMesh>().text = label;
        }
        else
        {
            keyID[keyIndex].material = keyColours[info[keyIndex][0] + 6];
            switch (info[keyIndex][2])
            {
                case 0:
                    keys[keyIndex].GetComponentInChildren<TextMesh>().color = new Color32(255, 255, 255, 255);
                    break;
                case 1:
                    keys[keyIndex].GetComponentInChildren<TextMesh>().color = new Color32(0, 0, 0, 255);
                    break;
            }
            keys[keyIndex].GetComponentInChildren<TextMesh>().text = info[keyIndex][1].ToString();
        }
    }

    private void Reset()
    {
        labelList.Clear();
        foreach (KMSelectable key in keys)
        {
            if (keys.IndexOf(key) != 6)
            {
                key.transform.localPosition = new Vector3(0, 0, -1f);
                key.GetComponentInChildren<TextMesh>().text = String.Empty;
                keyID[keys.IndexOf(key)].material = keyColours[8];
                alreadypressed[keys.IndexOf(key)] = false;
            }
        }
        if (moduleSolved == false)
        {
            pressable = false;
            int[] valueList = new int[6];
            string[] v = new string[6];
            int finalValue = 0;
            if (inputMode == false)
            {
                List<int> initialList = new List<int> { 0, 1, 2, 3, 4, 5};
                string[] labelList = new string[6];
                string[] s = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    int temp1 = UnityEngine.Random.Range(0, initialList.Count());
                    s[i] = (initialList[temp1] + 1).ToString();
                    soundList[1][i] = soundList[0][initialList[temp1]];
                    info[i][0] = UnityEngine.Random.Range(0, 6);
                    info[i][1] = UnityEngine.Random.Range(0, 6);
                    info[i][2] = UnityEngine.Random.Range(0, 6);
                    labelList[i] = (info[i][1] + 1).ToString();
                    valueList[i] = table[0][(info[i][0] * 3) + info[i][1]][(info[i][2] * 3) + initialList[temp1]];
                    v[i] = valueList[i].ToString();
                    initialList.RemoveAt(temp1);
                }
                initialValue = valueList.Sum();
                string[] a = new string[6];
                string[] b = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    a[i] = colourList[info[i][0]];
                    b[i] = colourList[info[i][2]];
                    if (i == 5)
                    {
                        string A = String.Join(", ", a);
                        string B = String.Join(", ", b);
                        Debug.LogFormat("[Recorded Keys #{0}] After {1} reset(s), the buttons had the colours: {2}", moduleID, resetCount, A);
                        Debug.LogFormat("[Recorded Keys #{0}] After {1} reset(s), the labels had the colours: {2}", moduleID, resetCount, B);
                    }
                }
                string[] label = labelList.ToArray();
                string sound = String.Join(", ", s);
                string l = String.Join(", ", label);
                string val = String.Join(" + ", v);
                Debug.LogFormat("[Recorded Keys #{0}] After {1} reset(s), the buttons were labelled: {2}", moduleID, resetCount, l);
                Debug.LogFormat("[Recorded Keys #{0}] After {1} reset(s), the keys play the sounds: {2}", moduleID, resetCount, sound);
                Debug.LogFormat("[Recorded Keys #{0}] After {1} reset(s), the initial value was: {2} = {3}", moduleID, resetCount, val, initialValue);
            }
            else
            {
                int newValue = 0;
                string[] BList = new string[2] { "White", "Black" };
                string[][] b = new string[2][] { new string[6], new string[6] };
                for (int i = 0; i < 6; i++)
                {
                    info[i][0] = UnityEngine.Random.Range(0, 2);
                    info[i][1] = UnityEngine.Random.Range(0, 10);
                    info[i][2] = UnityEngine.Random.Range(0, 2);
                    labelList.Add(info[i][1].ToString());
                    valueList[i] = table[1][(info[i][2] * 6) + i][(info[i][0] * 10) + info[i][1]];
                    v[i] = valueList[i].ToString();
                    b[0][i] = BList[info[i][0]];
                    b[1][i] = BList[info[i][2]];
                }
                newValue = valueList.Sum();
                finalValue = initialValue + newValue;
                Debug.LogFormat("[Recorded Keys #{0}] Stage {1}: The buttons had the colours: {2}", moduleID, stage, String.Join(", ", b[0]));
                Debug.LogFormat("[Recorded Keys #{0}] Stage {1}: The labels had the colours: {2}", moduleID, stage, String.Join(", ", b[1]));
                string[] label = labelList.ToArray();
                string l = String.Join(", ", label);
                Debug.LogFormat("[Recorded Keys #{0}] Stage {1}: The buttons were labelled: {2}", moduleID, stage, l);
                string val = String.Join(" + ", v);
                Debug.LogFormat("[Recorded Keys #{0}] The value for stage {1} was: {2} + {3} = {4}", moduleID, stage, val, initialValue, finalValue);
                string[] preanswer = new string[6];
                List<string> ans = new List<string> { };
                for (int i = 0; i < 6; i++)
                {
                    if(finalValue < (int)Math.Pow(2, 5 - i))
                    {
                        preanswer[i] = "0";
                        if(info[i][0] == 0)
                        {
                            answer[i] = "0";
                        }
                        else
                        {
                            answer[i] = "1";
                            ans.Add((i + 1).ToString());
                        }
                    }
                    else
                    {
                        preanswer[i] = "1";
                        finalValue -= (int)Math.Pow(2, 5 - i);
                        if (info[i][0] == 1)
                        {
                            answer[i] = "0";
                        }
                        else
                        {
                            answer[i] = "1";
                            ans.Add((i + 1).ToString());
                        }
                    }
                }
                string preans = String.Join(String.Empty, preanswer);
                Debug.LogFormat("[Recorded Keys #{0}] {1} in binary is {2}", moduleID, newValue + initialValue, preans);
                Debug.LogFormat("[Recorded Keys #{0}] Stage {1}: The keys that should be pressed are: {2}", moduleID, stage, String.Join(", ", ans.ToArray()));
            }        
        }
        StartCoroutine(sequence[0]);
    }

    private IEnumerator Shuff()
    {
        for (int i = 0; i < 30; i++)
        {
            if (i % 5 == 4)
            {
                if (moduleSolved == true)
                {
                    alreadypressed[(i - 4) / 5] = false;
                    keyID[(i - 4) / 5].material = keyColours[6];
                    keys[(i - 4) / 5].GetComponentInChildren<TextMesh>().color = new Color32(0, 0, 0, 255);
                    keys[(i - 4) / 5].GetComponentInChildren<TextMesh>().text = "0";
                    if (i == 29)
                    {
                        GetComponent<KMBombModule>().HandlePass();
                        GetComponent<KMAudio>().PlaySoundAtTransform("Recordered", transform);
                    }
                }
                else
                {
                    alreadypressed[(i - 4) / 5] = false;
                    keys[(i - 4) / 5].transform.localPosition = new Vector3(0, 0, 0);
                    GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, transform);
                    setKey((i - 4) / 5);
                }
                if (i == 29)
                {
                    i = -1;
                    pressable = true;
                    StopCoroutine(sequence[0]);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

 

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press 0123456 [position in reading order; 0 is the black button up top] | !{0} cycle [plays keys in reading order] | !{0} k [momentarily darkens white keys] | !{0} colorblind";
#pragma warning restore 414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*colorblind\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            colorblind = true;
            for (int i = 0; i < 6; i++)
                setKey(i);
            yield return null;
            yield break;
        }

        if (Regex.IsMatch(command, @"^\s*k\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            for (int i = 0; i < 6; i++)
            {
                keyID[i].material = keyColours[8];
            }
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < 6; i++)
                setKey(i);
            yield return null;
            yield break;
        }

        var m = Regex.Match(command, @"^\s*(?:press\s*)?([0123456 ,;]+)\s*$");
        if (!m.Success)
            yield break;

        foreach (var keyToPress in m.Groups[1].Value.Where(ch => ch >= '0' && ch <= '6').Select(ch => ch == '0' ? keys[6] : keys[ch - '1']))
        {
            yield return null;
            while (!pressable)
                yield return "trycancel";
            yield return new[] { keyToPress };
        }

        if (Regex.IsMatch(command, @"^\s*cycle\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (inputMode == false && pressable == true)
            {
                for (int i = 0; i < 6; i++)
                {
                    keys[i].OnInteract();
                    yield return new WaitForSeconds(0.2f);
                }
            }
            else
            {
                yield return "trycancel";
            }
        }
    }
}
