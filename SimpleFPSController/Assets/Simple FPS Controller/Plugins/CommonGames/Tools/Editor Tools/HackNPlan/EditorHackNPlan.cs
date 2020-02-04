
using UnityEditor;
using UnityEngine;

class EditorHackNPlan : EditorWindow
{
    WebViewHook _webView;
    string _url = "https://app.hacknplan.com/p/107236/kanban?categoryId=0&boardId=274409";

    [MenuItem(itemName: "Tools/Common-Games/Hack-N-Plan %#w")]
    static void Load()
    {
        EditorHackNPlan __window = GetWindow<EditorHackNPlan>();
        __window.Show();
    }

    void OnEnable()
    {
        if (!_webView)
        {
            // create webView
            _webView = CreateInstance<WebViewHook>();
        }
    }

    public void OnBecameInvisible()
    {
        if (_webView)
        {
            // signal the browser to unhook
            _webView.Detach();
        }
    }

    private void OnDestroy()
    {
        //Destroy web view
        DestroyImmediate(obj: _webView);
    }

    private void OnGUI()
    {
        // hook to this window
        if (_webView.Hook(host: this))
            // do the first thing to do
            _webView.LoadUrl(url: _url);

        // Navigation
        if (GUI.Button(position: new Rect(0, 0, 25, 20), text: "←"))
            _webView.Back();
        if (GUI.Button(position: new Rect(25, 0, 25, 20), text: "→"))
            _webView.Forward();

        // URL text field
        GUI.SetNextControlName(name: "urlfield");
        _url = GUI.TextField(position: new Rect(50, 0, width: position.width - 50, 20), text: _url);
        Event __ev = Event.current;

        // Focus on web view if return is pressed in URL field    
        if (__ev.isKey && GUI.GetNameOfFocusedControl().Equals(value: "urlfield"))
            if (__ev.keyCode == KeyCode.Return)
            {
                _webView.LoadUrl(url: _url);
                GUIUtility.keyboardControl = 0;
                _webView.SetApplicationFocus(true);
                __ev.Use();
            }
        //  else if (ev.keyCode == KeyCode.A && (ev.control | ev.command))


        if (__ev.type == EventType.Repaint)
        {
            // keep the browser aware with resize
            _webView.OnGUI(r: new Rect(0, 20, width: position.width, height: position.height - 20));
        }
    }
}
