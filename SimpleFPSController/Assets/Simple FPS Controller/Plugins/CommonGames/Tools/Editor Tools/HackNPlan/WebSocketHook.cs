/* WebHook 0.7 - https://github.com/willnode/WebViewHook/ - MIT */

namespace CommonGames.Tools
{
    using System;
    using System.Collections.Generic;
    using WebSocketSharp;
    using WebSocketSharp.Server;

    public class WebSocketHook : IDisposable
    {

        public class Item
        {
            public readonly Func<string> Get;
            public readonly Action<string> Set;
            public readonly Action Bind;

            private readonly Action<Item> _update;
        
            public void Update() { _update(obj: this); }

            public Item(Func<string> get, Action<string> set, Action bind, Action<Item> update)
            {
                Get = get;
                Set = set;
                Bind = bind;
                _update = update;
            }
        }

        public Dictionary<string, Item> Sockets = new Dictionary<string, Item>();

        readonly WebSocketServer _server;

        readonly WebViewHook _webView;

        readonly int _port;

        public WebSocketHook(int port, WebViewHook webView)
        {
            // add WebSocketHook to webView too
            this._port = port;
            this._webView = webView;

            webView.locationChanged += UpdateDefinitions;

            UpdateDefinitions(url: "");

            _server = new WebSocketServer(url: "ws://127.0.0.1:" + port + "");
            _server.AddWebSocketService<Wss>(path: "/ws", initializer: (e) => { e.Hook = this; });
            _server.Start();
        }

        public void Dispose()
        {
            _server.Stop();
            const string __TEMPLATE = "window.socket{0}.close(); window.socket{0} = undefined";

            if (_webView)
            {
                _webView.locationChanged -= UpdateDefinitions;
                _webView.ExecuteJavascript(js: string.Format(format: __TEMPLATE, arg0: _port));
            }
        }

        void UpdateDefinitions(string url)
        {

            const string __TEMPLATE = "window.socket{0} = window.socket{0} || new WebSocket('ws://127.0.0.1:{0}/ws');" +
                                    " function hook(){{ window.socket{0}.onopen = function(){{console.log('open');}}; window.socket{0}.onclose = function(e){{console.log('close' + e.code);}};  window.socket{0}.onerror = function(){{console.log('e');window.socket{0} = new WebSocket('ws://127.0.0.1:{0}/ws'); hook();}}; }};hook();";

            _webView.ExecuteJavascript(js: string.Format(format: __TEMPLATE, arg0: _port));

            foreach (KeyValuePair<string, Item> __item in Sockets)
            {
                __item.Value.Bind();
                __item.Value.Update();
            }
        }

        public Item Add(string path, Func<string> get, Action<string> set)
        {

            const string __TEMPLATE = "Object.defineProperty({0}, '{1}', {{ configurable: true, get: function(){{ return {0}.{1}___ }}, " +
                                    "set: function(v) {{ window.socket{2}.send('{0}.{1}=' + v) }} }})";

            const string __TEMPLATE2 = "{0}.{1}___ = '{2}'";

            int __dot = path.LastIndexOf('.');

            string __first = __dot < 0 ? "window" : path.Substring(0, length: __dot);
            string __last = __dot < 0 ? path : path.Substring(startIndex: __dot + 1);
            string __binding = string.Format(format: __TEMPLATE, arg0: __first, arg1: __last, arg2: _port);

            Item __hook = new Item(get: get, set: set, bind: () => _webView.ExecuteJavascript(js: __binding), update: (x) => 
                _webView.ExecuteJavascript(js: string.Format(format: __TEMPLATE2, arg0: __first, arg1: __last, arg2: (x.Get() ?? "").Replace(oldValue: "'", newValue: "\'").Replace(oldValue: "\n", newValue: "\\n"))));

            Sockets[key: __first + "." + __last] = __hook;
            __hook.Update();
            return __hook;
        }
    }

    public class Wss : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            string __n = e.Data;
            int __i = __n.IndexOf('=');
            string __k = __n.Substring(0, length: __i);
            string __v = __n.Substring(startIndex: __i + 1);
            Hook.Sockets[key: __k].Set(obj: __v);
        }

        public WebSocketHook Hook;
    }
}