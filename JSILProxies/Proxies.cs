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
        "SDL2.SDL",
        JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class SDLProxy {
        public static string SDL_GetPlatform() {
            return "Windows";
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
        "Microsoft.Xna.Framework.Audio.OpenALDevice",
        JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class OpenALDeviceProxy {
        public static void Initialize() {
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
}
