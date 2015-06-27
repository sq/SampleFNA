//
// Derived from Emscripten's OpenAL implementation (MIT-licensed)
// see https://github.com/kripken/emscripten/blob/master/src/library_openal.js
//
// Emscripten license follows:
/*
  Copyright (c) 2010-2011 Emscripten authors, see AUTHORS file.

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
  THE SOFTWARE.
*/
//

"use strict";

if (typeof (JSIL) === "undefined") 
  throw new Error("JSIL.Core required");

JSIL.DeclareNamespace("JSIL");
JSIL.DeclareNamespace("JSIL.AL");


JSIL.AL.$deviceToken = null;
JSIL.AL.getDeviceToken = function () {
  if (!JSIL.AL.$deviceToken)
    JSIL.AL.$deviceToken = new System.IntPtr(1);

  return JSIL.AL.$deviceToken;
};

JSIL.AL.contexts = [];
JSIL.AL.currentContext = null;

JSIL.AL.QUEUE_INTERVAL = 25;
JSIL.AL.QUEUE_LOOKAHEAD = 100;

JSIL.AL.getError = function () {
  if (!JSIL.AL.currentContext) {
    return 0xA004; /* AL_INVALID_OPERATION */
  } else {
    // Reset error on get.
    var err = JSIL.AL.currentContext.err;
    JSIL.AL.currentContext.err = 0; /* AL_NO_ERROR */
    return err;
  }
};

JSIL.AL.updateSource = function (src) {
  if (src.state !== 0x1012 /* AL_PLAYING */) {
    return;
  }

  var currentTime = JSIL.AL.currentContext.ctx.currentTime;
  var startTime = src.bufferPosition;

  // Apply any changes to the AL_PITCH parameter;
  for (var i = 0; i < src.queue.length; i++) {
    var entry = src.queue[i];

    if (entry && entry.src)
      entry.src.playbackRate.value = src.pitch;
  }

  for (var i = src.buffersPlayed; i < src.queue.length; i++) {
    var entry = src.queue[i];

    var startOffset = startTime - currentTime;
    var endTime = startTime + entry.buffer.duration;

    // Clean up old buffers.
    if (currentTime >= endTime) {
      // Update our location in the queue.
      src.bufferPosition = endTime;
      src.buffersPlayed = i + 1;

      // Stop / restart the source when we hit the end.
      if (src.buffersPlayed >= src.queue.length) {
        if (src.loop) {
          JSIL.AL.setSourceState(src, 0x1012 /* AL_PLAYING */);
        } else {
          JSIL.AL.setSourceState(src, 0x1014 /* AL_STOPPED */);
        }
      }
    }
    // Process all buffers that'll be played before the next tick.
    else if (startOffset < (JSIL.AL.QUEUE_LOOKAHEAD / 1000) && !entry.src) {
      // If the start offset is negative, we need to offset the actual buffer.
      var offset = Math.abs(Math.min(startOffset, 0));

      entry.src = JSIL.AL.currentContext.ctx.createBufferSource();
      entry.src.buffer = entry.buffer;
      entry.src.playbackRate.value = src.pitch;
      entry.src.connect(src.gain);
      entry.src.start(startTime, offset);
    }

    startTime = endTime;
  }
};

JSIL.AL.updateSources = function (context) {
  for (var i = 0; i < context.src.length; i++) {
    JSIL.AL.updateSource(context.src[i]);
  }
};

JSIL.AL.setSourceState = function(src, state) {
  if (state === 0x1012 /* AL_PLAYING */) {
    if (src.state !== 0x1013 /* AL_PAUSED */) {
      src.state = 0x1012 /* AL_PLAYING */;
      // Reset our position.
      src.bufferPosition = JSIL.AL.currentContext.ctx.currentTime;
      src.buffersPlayed = 0;
    } else {
      src.state = 0x1012 /* AL_PLAYING */;
      // Use the current offset from src.bufferPosition to resume at the correct point.
      src.bufferPosition = JSIL.AL.currentContext.ctx.currentTime - src.bufferPosition;
    }
    JSIL.AL.stopSourceQueue(src);
    JSIL.AL.updateSource(src);
  } else if (state === 0x1013 /* AL_PAUSED */) {
    if (src.state === 0x1012 /* AL_PLAYING */) {
      src.state = 0x1013 /* AL_PAUSED */;
      // Store off the current offset to restore with on resume.
      src.bufferPosition = JSIL.AL.currentContext.ctx.currentTime - src.bufferPosition;
      JSIL.AL.stopSourceQueue(src);
    }
  } else if (state === 0x1014 /* AL_STOPPED */) {
    if (src.state !== 0x1011 /* AL_INITIAL */) {
      src.state = 0x1014 /* AL_STOPPED */;
      src.buffersPlayed = src.queue.length;
      JSIL.AL.stopSourceQueue(src);
    }
  } else if (state == 0x1011 /* AL_INITIAL */) {
    if (src.state !== 0x1011 /* AL_INITIAL */) {
      src.state = 0x1011 /* AL_INITIAL */;
      src.bufferPosition = 0;
      src.buffersPlayed = 0;
    }
  }
};

JSIL.AL.stopSourceQueue = function(src) {
  for (var i = 0; i < src.queue.length; i++) {
    var entry = src.queue[i];
    if (entry.src) {
      entry.src.stop(0);
      entry.src = null;
    }
  }
};

JSIL.ImplementExternals("OpenAL.ALC10", function ($) {
  $.Method({Static:true , Public:true }, "alcOpenDevice", 
    new JSIL.MethodSignature($jsilcore.TypeRef("System.IntPtr"), [$.String], []), 
    function alcOpenDevice (deviceName) {
      return JSIL.AL.getDeviceToken();
    }
  );

  $.Method({Static:true , Public:true }, "alcGetError", 
    new JSIL.MethodSignature($.Int32, [$jsilcore.TypeRef("System.IntPtr")], []), 
    function alcGetError (device) {
      var err = JSIL.AL.alcErr;
      JSIL.AL.alcErr = 0;
      return err;
    }
  );

  var createContext = function (device) {
    if (device.value !== JSIL.AL.getDeviceToken().value)
      throw new Error("Invalid device");

    var ctx;
    try {
      ctx = new AudioContext();
    } catch (e) {
      try {
        ctx = new webkitAudioContext();
      } catch (e) {}
    }

    if (ctx) {
      var context = {
        ctx: ctx,
        err: 0,
        src: [],
        buf: [],
        originalBitCounts: [],
        interval: setInterval(
          function() { JSIL.AL.updateSources(context); }, JSIL.AL.QUEUE_INTERVAL
        )
      };

      JSIL.AL.contexts.push(context);

      return new System.IntPtr(JSIL.AL.contexts.length);
    } else {
      return new System.IntPtr(0);
    }
  };

  $.Method({Static: true, Public: true}, "alcCreateContext",
    new JSIL.MethodSignature($jsilcore.TypeRef("System.IntPtr"), [$jsilcore.TypeRef("System.IntPtr"), $jsilcore.TypeRef("System.Array", [$.Int32])]),
    function alcCreateContext(device, attrList) {
      return createContext(device);
    }
  );

});


