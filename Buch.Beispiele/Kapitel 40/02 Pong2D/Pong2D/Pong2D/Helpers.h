#pragma once

#include <Windows.h>
#include <stdio.h>
#include <stdlib.h>

#pragma warning(disable: 4996)

static wchar_t* StringFormat(const char* format, ...)
{
    va_list ap;
    va_start(ap,format);

    char str[256];
    vsprintf_s(str,format,ap);

	size_t size = strlen(str) + 1;
    wchar_t* wa = new wchar_t[size];
	size_t* ret;
    mbstowcs(wa,str,size);
    return wa;
};
