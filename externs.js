JSIL.ImplementExternals("OpenAL.AL10", function ($) {
  $.Method({Static:true, Public:true}, "alBufferData",
	  new JSIL.MethodSignature(null, [$.UInt32, $.Int32, $jsilcore.TypeRef("System.Array", [$.Byte]), $.IntPtr, $.IntPtr], []),  
	  function alBufferData(buffer, format, data, size, freq) {
	  	// this currently has poor performance due to copying to the emscripten heap
	  	// ideally it would just set the buffer directly, but that requires access to the AL
	  	// var which is internal to soft_oal.js
	  	// - mispy
	  	size = size.ToInt32();
	  	freq = freq.ToInt32();
 	    var Module = JSIL.PInvoke.GetModule("soft_oal.dll");

        var _alBufferData = Module.cwrap(
        	'alBufferData', null, ['number', 'number', 'number', 'number', 'number']
  		);

	    var nDataBytes = data.length * data.BYTES_PER_ELEMENT;
        var dataPtr = Module._malloc(nDataBytes);	  		
	    var dataHeap = new Uint8Array(Module.HEAPU8.buffer, dataPtr, nDataBytes);
	    dataHeap.set(new Uint8Array(data.buffer));

 	    _alBufferData(buffer, format, dataHeap.byteOffset, size, freq);

	    Module._free(dataHeap.byteOffset);
	  }
  );
});