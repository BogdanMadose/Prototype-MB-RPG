using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyBindManager : MonoBehaviour
{
    private static KeyBindManager instance;

    public static KeyBindManager MInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeyBindManager>();
            }
            return instance;
        }
    }

    private string bindName;

    public Dictionary<string, KeyCode> Keybinds { get; private set; }
    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Keybinds = new Dictionary<string, KeyCode>();
        ActionBinds = new Dictionary<string, KeyCode>();
        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDict = Keybinds;

        if (key.Contains("ACT"))
        {
            currentDict = ActionBinds;
        }
        if (!currentDict.ContainsKey(key))
        {
            currentDict.Add(key, keyBind);
            UIManager.MInstance.UpdateKeyText(key, keyBind);
        }
        else if (currentDict.ContainsValue(keyBind))
        {
            string newKey = currentDict.FirstOrDefault(x => x.Value == keyBind).Key;
            currentDict[newKey] = KeyCode.None;
            UIManager.MInstance.UpdateKeyText(key, KeyCode.None);
        }
        currentDict[key] = keyBind;
        UIManager.MInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
