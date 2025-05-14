// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <windows.h>
#include <combaseapi.h> 
#pragma once
#include <string>
#include <algorithm>

typedef void(__cdecl* TraceCallback)(int level, const char* message);

extern "C" __declspec(dllexport)
void __cdecl test_log_callback(int traceLevel, TraceCallback traceCallback)
{
    traceCallback(traceLevel, "Starting test_log_callback");
    traceCallback(traceLevel, "Finishing test_log_callback");
}

struct SaleResponse
{
    int    saleId;
    double amount;
};

struct SaleRequest
{
    int value;
};

extern "C" __declspec(dllexport)
SaleResponse* __cdecl test_sale(SaleRequest* saleRequest)
{
    SaleResponse* resp = (SaleResponse*)CoTaskMemAlloc(sizeof(SaleResponse));
   
    resp->saleId = saleRequest ? saleRequest->value : 0;
    resp->amount = 123.45;
    return resp;
}

extern "C" __declspec(dllexport)
char* __cdecl modify_input(const char* input)
{
    if (!input) return nullptr;
    std::string upper(input);
    std::transform(upper.begin(), upper.end(), upper.begin(), ::toupper);
    std::string result = "MODIFIED: " + upper;
    auto output = (char*)CoTaskMemAlloc(result.size() + 1);
    if (output) {
        memcpy(output, result.c_str(), result.size() + 1);
    }
    return output;
}

extern "C" __declspec(dllexport)
wchar_t* __cdecl modify_input_uni(const wchar_t* input)
{
    if (!input) return nullptr;
    std::wstring upper(input);
    std::transform(upper.begin(), upper.end(), upper.begin(), ::towupper);
    std::wstring result = L"MODIFIED: " + upper;
    size_t size = (result.size() + 1) * sizeof(wchar_t);
    auto output = (wchar_t*)CoTaskMemAlloc(size);
    if (output) {
        memcpy(output, result.c_str(), size);
    }
    return output;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    return TRUE;
}

