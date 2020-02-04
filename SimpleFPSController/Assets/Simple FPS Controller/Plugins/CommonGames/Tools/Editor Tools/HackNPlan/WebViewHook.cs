/* WebHook 0.7 - https://github.com/willnode/WebViewHook/ - MIT */

using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class WebViewHook : ScriptableObject
{
    Object _webView;
    EditorWindow _host;
    object _hostCache;

    static Type _t;
    static FieldInfo _parent;
    static MethodInfo _show, _hide, _back, _reload, _forward;
    static MethodInfo _setSizeAndPosition;
    static MethodInfo _initWebView;
    static MethodInfo _setDelegateObject;
    static MethodInfo _allowRightClickMenu;
    static MethodInfo _showDevTools;
    static MethodInfo _defineScriptObject;
    static MethodInfo _setHostView;
    static MethodInfo _executeJavascript;
    static MethodInfo _loadUrl;
    static MethodInfo _hasApplicationFocus;
    static MethodInfo _setApplicationFocus;
    static Func<Rect, Rect> _unclip;

    static WebViewHook()
    {
        _t = typeof(Editor).Assembly.GetTypes().First(predicate: x => x.Name == "WebView");
        _parent = typeof(EditorWindow).GetField(name: "m_Parent", bindingAttr: _INSTANCE);
        _show = (_t.GetMethod(name: "Show", bindingAttr: _INSTANCE));
        _hide = (_t.GetMethod(name: "Hide", bindingAttr: _INSTANCE));
        _back = (_t.GetMethod(name: "Back", bindingAttr: _INSTANCE));
        _reload = (_t.GetMethod(name: "Reload", bindingAttr: _INSTANCE));
        _forward = (_t.GetMethod(name: "Forward", bindingAttr: _INSTANCE));
        _initWebView = (_t.GetMethod(name: "InitWebView", bindingAttr: _INSTANCE));
        _setSizeAndPosition = (_t.GetMethod(name: "SetSizeAndPosition", bindingAttr: _INSTANCE));
        _setHostView = (_t.GetMethod(name: "SetHostView", bindingAttr: _INSTANCE));
        _allowRightClickMenu = (_t.GetMethod(name: "AllowRightClickMenu", bindingAttr: _INSTANCE));
        _setDelegateObject = (_t.GetMethod(name: "SetDelegateObject", bindingAttr: _INSTANCE));
        _showDevTools = (_t.GetMethod(name: "ShowDevTools", bindingAttr: _INSTANCE));
        _defineScriptObject = (_t.GetMethod(name: "DefineScriptObject", bindingAttr: _INSTANCE));
        _executeJavascript = (_t.GetMethod(name: "ExecuteJavascript", bindingAttr: _INSTANCE));
        _loadUrl = (_t.GetMethod(name: "LoadURL", bindingAttr: _INSTANCE));
        _hasApplicationFocus = (_t.GetMethod(name: "HasApplicationFocus", bindingAttr: _INSTANCE));
        _setApplicationFocus = (_t.GetMethod(name: "SetApplicationFocus", bindingAttr: _INSTANCE));
        _unclip = (Func<Rect, Rect>)Delegate.CreateDelegate(type: typeof(Func<Rect, Rect>), method: typeof(GUI).Assembly.GetTypes()
            .First(predicate: x => x.Name == "GUIClip").GetMethod(name: "Unclip", bindingAttr: BindingFlags.Static | BindingFlags.Public, null, types: new Type[] { typeof(Rect) }, null));

    }

    ~WebViewHook()
    {
        OnDisable();
    }

    void OnEnable()
    {
        if (!_webView)
        {
            _webView = CreateInstance(type: _t);
            _webView.hideFlags = HideFlags.DontSave;
            this.hideFlags = HideFlags.DontSave;
        }
    }

    void OnDisable()
    {
        if (_webView)
        {
            Detach();
        }
    }

    void OnDestroy()
    {
        DestroyImmediate(obj: _webView);
        _webView = null;
    }

    public bool Hook(EditorWindow host)
    {
        if (host == this._host) return false;

        if (!_webView)
            OnEnable();

        // initialization go here

        Invoke(m: _initWebView, _parent.GetValue(obj: _hostCache = (this._host = host)), 0, 0, 1, 1, false);
        Invoke(m: _setDelegateObject, this);
        Invoke(m: _allowRightClickMenu, true);

        return true;
    }

    public void Detach()
    {
        Invoke(m: _setHostView, this._hostCache = null);
    }

    void SetHostView(object host)
    {
        Invoke(m: _setHostView, this._hostCache = host);
        Hide();
        Show();
    }

    void SetSizeAndPosition(Rect position)
    {
        Invoke(m: _setSizeAndPosition, (int)position.x, (int)position.y, (int)position.width, (int)position.height);
    }

    void OnGUI() { }

    public void OnGUI(Rect r)
    {
        if (_host)
        {
            object __h = _parent.GetValue(obj: _host);
            if (_hostCache != __h)
                SetHostView(host: __h);
            else
                Invoke(m: _setHostView, __h);
        }

        SetSizeAndPosition(position: _unclip(arg: r));
    }

    public void AllowRightClickMenu(bool yes)
    {
        Invoke(m: _allowRightClickMenu, yes);
    }

    public void Forward()
    {
        Invoke(m: _forward);
    }

    public void Back()
    {
        Invoke(m: _back);
    }

    public void Show()
    {
        Invoke(m: _show);
    }

    public void Hide()
    {
        Invoke(m: _hide);
    }

    public void Reload()
    {
        Invoke(m: _reload);
    }

    public bool HasApplicationFocus()
    {
        return (bool)_hasApplicationFocus.Invoke(obj: _webView, null);
    }

    public void SetApplicationFocus(bool focus)
    {
        Invoke(m: _setApplicationFocus, focus);
    }

    protected void ShowDevTools()
    {
        // This method may not work
        Invoke(m: _showDevTools);
    }

    public void LoadUrl(string url)
    {
        Invoke(m: _loadUrl, url);
    }

    public void LoadHtml(string html)
    {
        Invoke(m: _loadUrl, "data:text/html;charset=utf-8," + html);
    }

    public void LoadFile(string path)
    {
        Invoke(m: _loadUrl, "file:///" + path);
    }

    protected void DefineScriptObject(string path, ScriptableObject obj)
    {
        // This method has unknown behavior
        Invoke(m: _defineScriptObject, path, obj);
    }

    protected void SetDelegateObject(ScriptableObject obj)
    {
        // Only set into this object
        Invoke(m: _setDelegateObject, obj);
    }

    public void ExecuteJavascript(string js)
    {
        Invoke(m: _executeJavascript, js);
    }

    void Invoke(MethodInfo m, params object[] args)
    {
        try
        {
            m.Invoke(obj: _webView, parameters: args);
        }
        catch (Exception) { }
    }

    const BindingFlags _INSTANCE = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

    /* Default bindings for SetDelegateObject */

    public Action<string> loadError;

    public Action initScripting;

    public Action<string> locationChanged;

    protected virtual void OnLocationChanged(string url)
    {
        if (locationChanged != null)
            locationChanged(obj: url);
    }

    protected virtual void OnLoadError(string url)
    {
        if (loadError != null)
            loadError(obj: url);
        else
            Debug.LogError(message: "WebView has failed to load " + url);
    }

    protected virtual void OnInitScripting()
    {
        if (initScripting != null)
            initScripting();
    }

    protected virtual void OnOpenExternalLink(string url)
    {
        // This binding may not work
    }

    protected virtual void OnWebViewDirty()
    {
        // This binding may not work
    }

    protected virtual void OnDownloadProgress(string id, string message, ulong bytes, ulong total)
    {
        // This binding may not work
    }

    protected virtual void OnBatchMode()
    {
        // This binding may not work
    }

    protected virtual void OnReceiveTitle(string title)
    {
        // This binding may not work
    }

    protected virtual void OnDomainReload()
    {
        // This binding may not work
    }

}
