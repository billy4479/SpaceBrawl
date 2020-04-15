using Lean.Transition;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DialogueSystem : MonoBehaviour
{
    private Dictionary<int, DialogueData> dialogues;
    public static DialogueSystem instance { get; private set; }
    [SerializeField] private GameObject dialoguePrefab;
    private TextMeshProUGUI[] buttons;
    private bool isThereADialogue = false;
    private GameObject currentDialogue;

    private void LoadDialogs()
    {
        dialogues = new Dictionary<int, DialogueData>();
        var rawCSV = Resources.Load("dialogues") as TextAsset;
        string[] lines = rawCSV.text.Split(new char[] { '\n' });
        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i] == "") continue;
            string[] data = lines[i].Split(new char[] { ';' });
            int.TryParse(data[0], out int id);
            string content = data[1];
            int.TryParse(data[2], out int onClick);
            int.TryParse(data[3], out int nextDialogue);
            int.TryParse(data[4], out int buttonNumber);
            string[] buttonTexts = new string[buttonNumber];
            int[] buttonsActions = new int[buttonNumber];

            for (int j = 0; j < buttonNumber * 2; j += 2)
            {
                buttonTexts[j] = data[5 + j];
                int.TryParse(data[6 + j], out buttonsActions[j]);
            }


            DialogueButton[] buttonData = new DialogueButton[buttonNumber];
            for (int j = 0; j < buttonNumber; j++)
                buttonData[j] = new DialogueButton((ButtonActions)buttonsActions[j], buttonTexts[j]);

            dialogues.Add(id, new DialogueData(id, content, (Dialogue_OnClick)onClick, buttonData, nextDialogue));
        }
    }

    private void Awake()
    {
        #region Singleton

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        #endregion Singleton

        LoadDialogs();
    }

    public void ShowDialogue(int id)
    {
        if (isThereADialogue)
        {
            Debug.LogWarning("[Dialogue System] There\'s already a dialogue!");
            return;
        }
        if (!dialogues.ContainsKey(id))
        {
            Debug.LogError($"[Dialogue System] ID not found! The ID was {id}");
            return;
        }
        isThereADialogue = true;
        currentDialogue = Instantiate(dialoguePrefab, GameObject.Find("Canvas").transform);
        var frame = currentDialogue.transform.Find("Frame");
        var textObj = frame.transform.Find("Text Background").Find("Content").GetComponent<TextMeshProUGUI>();
        GameObject[] containers = GameObject.FindGameObjectsWithTag("DialogueButtons");
        foreach (var button in containers)
            button.SetActive(false);

        currentDialogue.GetComponent<Image>().colorTransition(new Color(0, 0, 0, 0.33f), 1f);
        frame.localPositionTransition_Y(-390f, 1f, LeanEase.Bounce); //World Space?
        dialogues.TryGetValue(id, out DialogueData data);
        StartCoroutine(AnimateTextAndButtons(data.content, textObj, data, containers));
        if (data.onClick != Dialogue_OnClick.TheresAButton) StartCoroutine(WaitForClick(data));
    }

    private IEnumerator WaitForClick(DialogueData mode)
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(1))
                break;
            yield return new WaitForEndOfFrame();
        }
        if (mode.onClick == Dialogue_OnClick.Quit) RemoveDialogue();
        if (mode.onClick == Dialogue_OnClick.ChangeDialogue)
        {
            RemoveDialogue();
            ShowDialogue(mode.nextDialogue);
        }
    }

    private IEnumerator AnimateTextAndButtons(string text, TextMeshProUGUI obj, DialogueData mode, GameObject[] containers)
    {
        obj.text = "";
        yield return new WaitForSeconds(1f);
        foreach (var c in text)
        {
            obj.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        if (mode.onClick == Dialogue_OnClick.TheresAButton)
        {
            for (int i = 0; i < containers.Length; i++)
            {
                int.TryParse(containers[i].name, out int index);
                if (mode.dialogueButtons.Length > index)
                {
                    containers[i].SetActive(true);
                    var button = containers[i].transform.Find("Button").GetComponent<Button>();
                    var button_text = button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                    button_text.text = mode.dialogueButtons[index].text;
                    switch (mode.dialogueButtons[index].callback)
                    {
                        case ButtonActions.OPENDOWNLOADPAGE:
                            button.onClick.AddListener(ButtonAction_OpenDownloadPage);
                            break;
                        default:
                            break;
                    }
                    button.onClick.AddListener(RemoveDialogue);
                }
            }
        }
    }

    private void ButtonAction_OpenDownloadPage()
    {
        Application.OpenURL(GameUpdater.instance.GetURL());
    }

    private void RemoveDialogue()
    {
        StartCoroutine(RemoveDialogCoroutine(currentDialogue, currentDialogue.transform.Find("Frame")));
    }
    private IEnumerator RemoveDialogCoroutine(GameObject fade, Transform frame)
    {
        fade.GetComponent<Image>().colorTransition(new Color(0, 0, 0, 0), 0.5f);
        frame.localPositionTransition_Y(-700f, 1f, LeanEase.Accelerate);
        yield return new WaitForSeconds(1f);
        isThereADialogue = false;
        Destroy(fade);
    }

    class DialogueData
    {
        public Dialogue_OnClick onClick { get; private set; }
        public DialogueButton[] dialogueButtons { get; private set; }
        public int nextDialogue { get; private set; }
        public string content { get; private set; }
        public int id { get; private set; }

        public DialogueData(int id, string content, Dialogue_OnClick dialogue_OnClick, DialogueButton[] dialogueButtons = null, int nextDialogue = default)
        {
            onClick = dialogue_OnClick;
            this.id = id;
            if (string.IsNullOrEmpty(content)) throw new ArgumentNullException(nameof(content));
            this.content = content;
            if (onClick == Dialogue_OnClick.TheresAButton)
            {
                this.dialogueButtons = dialogueButtons ?? throw new ArgumentNullException(nameof(dialogueButtons));
                if (dialogueButtons.Length > 3) throw new ArgumentOutOfRangeException(nameof(dialogueButtons), "The max button number is 3");
            }
            if (onClick == Dialogue_OnClick.ChangeDialogue)
            {
                this.nextDialogue = nextDialogue;
            }
        }
    }

    class DialogueButton
    {
        public ButtonActions callback { get; private set; }
        public string text { get; private set; }

        public DialogueButton(ButtonActions callback, string text)
        {
            this.callback = callback;
            this.text = text ?? throw new ArgumentNullException(nameof(text));
        }
    }

    enum Dialogue_OnClick { Quit, ChangeDialogue, TheresAButton }
    enum ButtonActions { OPENDOWNLOADPAGE }//Aggiungere altre azioni
}


