using System;
using JSIL.Meta;
using JSIL.Proxy;

namespace HelloWorld.JSILProxies {
    [JSProxy(
        "MonoGame.Utilities.AssemblyHelper",
        JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class AssemblyHelperProxy {
        [JSReplacement("document.title")]
        public static string GetDefaultWindowTitle() {
            throw new NotImplementedException();
        }
    }

    [JSProxy(
        "Microsoft.Xna.Framework.SDL2_GameWindow",
        JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class SDL2_GameWindowProxy {
        private void INTERNAL_SetIcon(string title) {
            // Setting favicons is the html's business
        }
    }

    [JSProxy(
    "System.IO.Path",
    JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class PathProxy {
        public static Char[] GetInvalidFilenameChars() { // XXX maybe implement this in JSIL core
            return new Char[] { }; 
        }
    }

    // not sure it is possible to overcome the GetDelegateForFunctionPointer issue
    // using proxies alone. possibly some fancy reflection trick I don't know yet
    /*[JSProxy(
    "System.Runtime.InteropServices.Marshal",
    JSProxyMemberPolicy.ReplaceDeclared
    )]      
    public abstract class MarshalProxy {
        private delegate void GenericDelegate();
        private static void NotImplementedDelegate() {
            throw new NotImplementedException();
        }

        public static Delegate GetDelegateForFunctionPointer(IntPtr ptr, Type t) {
            return new GenericDelegate(NotImplementedDelegate);
        }
    }*/
}
