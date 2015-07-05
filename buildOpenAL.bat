del soft_oal.js
call emcc -s EXPORT_FUNCTION_TABLES=1 -s MODULARIZE=1 -s DEFAULT_LIBRARY_FUNCS_TO_INCLUDE=@alcfuncs2 -s EXPORTED_FUNCTIONS=@alcfuncs empty.c --memory-init-file 0 -O3 -o soft_oal.js