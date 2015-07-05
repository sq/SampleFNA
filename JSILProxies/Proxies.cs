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
        "JSIL.FNAHelpers",
        JSProxyMemberPolicy.ReplaceDeclared
    )]
    public abstract class FNAHelpersProxy {
        internal static void BufferSubData (string p, int elementSizeInBytes, int offsetInBytes, Array data, int startIndex, int elementCount) {
            dynamic gl = Verbatim.Expression(
                "this.cachedGlContext " +
                "  ? this.cachedGlContext " +
                "  : (this.cachedGlContext = document.getElementById('canvas').getContext('webgl'))"
            );
            object view = Verbatim.Expression("new Uint8Array($0.buffer, $1, $2)", data, offsetInBytes, elementSizeInBytes * elementCount);
            gl.bufferSubData(gl[p], startIndex, view);
        }

        public static object GetALContext() {
            return Verbatim.Expression("JSIL.PInvoke.GetModule('soft_oal.dll', true).OpenAL.currentContext.ctx");
        }

        internal static void BeginDecodeSong (string filename, JSIL.SongDecodeCompleteHandler onDecodeComplete) {
            Console.WriteLine("Starting decode for {0}", filename);

            dynamic stream = System.IO.File.OpenRead(filename);
            // HACK: JSIL stores the underlying byte array for the file as a property on the stream
            dynamic fileByteArray = stream._buffer;
            object fileArrayBuffer = fileByteArray.buffer;

            dynamic al = JSIL.FNAHelpers.GetALContext();

            al.decodeAudioData(fileArrayBuffer, Verbatim.Expression(
                "function (buffer) { console.log('decode complete'); $0(buffer); }",
                onDecodeComplete
            ));
            Console.WriteLine("Decode started");
        }

        internal static object PlaySong (object audioBuffer, float volume) {
            if (audioBuffer == null)
                return null;

            dynamic al = JSIL.FNAHelpers.GetALContext();

            var gain = al.createGain();
            gain.gain.value = volume;
            gain.connect(al.destination);

            var source = al.createBufferSource();
            source.buffer = audioBuffer;
            // FIXME
            source.loop = true;
            source.connect(gain);
            source.start();

            return JSIL.Verbatim.Expression(
                "{ source: $0, gain: $1 }",
                source,
                gain
            );
        }

        internal static void PauseSong (object audioBuffer, object playingSong) {
            if ((audioBuffer == null) || (playingSong == null))
                return;

            throw new NotImplementedException("Web Audio API cannot pause/resume");
        }

        internal static void ResumeSong (object audioBuffer, object playingSong) {
            if ((audioBuffer == null) || (playingSong == null))
                return;

            throw new NotImplementedException("Web Audio API cannot pause/resume");
        }

        internal static void StopSong (object audioBuffer, object playingSong) {
            if ((audioBuffer == null) || (playingSong == null))
                return;

            dynamic ps = playingSong;
            ps.source.stop();
        }

        internal static float GetSongLength (object audioBuffer) {
            if (audioBuffer == null)
                return 0.0f;

            dynamic ab = audioBuffer;
            return (float)(ab.length);
        }

        internal static void SetSongVolume (object audioBuffer, object playingSong, float volume) {
            if ((audioBuffer == null) || (playingSong == null))
                return;

            dynamic ps = playingSong;
            ps.gain.gain.value = volume;
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
