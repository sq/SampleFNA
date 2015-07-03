using System;
using JSIL;
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
        "Microsoft.XNA.Framework.Input",
        JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class GamepadProxy {
        internal static void INTERNAL_InitMonoGameJoystick() {
            Console.WriteLine("INTERNAL_InitMonoGameJoystick");
            // Do nothing for now
        }
    }

    [JSProxy(
        "Microsoft.Xna.Framework.SDL2_GameWindow",
        JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class SDL2_GameWindowProxy {
        private void INTERNAL_SetIcon(string title) {
            Console.WriteLine("INTERNAL_SetIcon");
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

    [JSProxy(
        "Microsoft.Xna.Framework.Graphics.JSILHelpers",
        JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class JSILHelpersProxy {
        internal static void BufferSubData (string p, int elementSizeInBytes, int offsetInBytes, Array data, int startIndex, int elementCount) {
            dynamic gl = Builtins.Global["document"].getElementById("canvas").getContext("webgl");
            object view = Verbatim.Expression("new Uint8Array($0.buffer, $1, $2)", data, offsetInBytes, elementSizeInBytes * elementCount);
            gl.bufferSubData(gl[p], startIndex, view);
        }
    }

    [JSProxy(
        "Platformer2D.PlatformerGame",
        JSProxyMemberPolicy.ReplaceNone
    )]
    public class PlatformerGameProxy {
        public void Run () {
        }
    }
    
    [JSProxy(
        "Platformer2D.Program",
        JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class ProgramProxy {
		public static void Main () {
            var game = new PlatformerGameProxy();
            game.Run();
        }
    }
}
