// Взято с https://forum.unity.com/threads/webgl-transparent-background.284699/#post-1880667
// Не забудьте установить на основной камера Clear Flags на Solid Color и указать прозрачность на ноль в Background

var LibraryGLClear = {
    glClear: function(mask)
    {
        if (mask == 0x00004000)
        {
            var v = GLctx.getParameter(GLctx.COLOR_WRITEMASK);
            if (!v[0] && !v[1] && !v[2] && v[3])
                // We are trying to clear alpha only -- skip.
                return;
        }
        GLctx.clear(mask);
    }
};
 
mergeInto(LibraryManager.library, LibraryGLClear);