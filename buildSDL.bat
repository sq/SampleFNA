del SDL2.js
call emcc -s FULL_ES2=1 -s EXPORT_FUNCTION_TABLES=1 -s MODULARIZE=1 -s USE_SDL=2 -s EXPORTED_FUNCTIONS=@sdlfuncs empty.c --memory-init-file 0 -O3 -o SDL2.js