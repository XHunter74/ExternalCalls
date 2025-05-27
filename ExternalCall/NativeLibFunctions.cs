using System.Runtime.InteropServices;

namespace ExternalCall;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void IntTraceCallbackDelegate(int traceLevel, IntPtr traceMessagePointer);

public delegate void TraceCallback(int traceLevel, string traceMessage);

public static class NativeLibFunctions
{
    private static IntTraceCallbackDelegate _callbackKeeper;

    [DllImport("TestLib", EntryPoint = "test_log_callback",
           CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void NativeTestLogCallback(int traceLevel, IntPtr traceCallback);

    public static void TestLogCallback(int traceLevel, TraceCallback traceCallback)
    {
        _callbackKeeper = (level, ptr) =>
        {
            string? msg = ptr != IntPtr.Zero
                ? Marshal.PtrToStringAnsi(ptr)
                : null;
            traceCallback(level, msg!);
        };

        var traceDelegatePointer = Marshal
            .GetFunctionPointerForDelegate(_callbackKeeper);
        NativeTestLogCallback(traceLevel, traceDelegatePointer);
    }

    [DllImport("TestLib", EntryPoint = "test_sale",
           CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr NativeTestSale(IntPtr saleRequest);

    public static SaleResponse TestSale(SaleRequest saleRequest)
    {
        var saleRequestPointer = Marshal.AllocHGlobal(Marshal.SizeOf(saleRequest));
        Marshal.StructureToPtr(saleRequest, saleRequestPointer, false);
        try
        {
            var saleResponsePointer = NativeTestSale(saleRequestPointer);
            return Marshal.PtrToStructure<SaleResponse>(saleResponsePointer)!;
        }
        finally
        {
            Marshal.FreeHGlobal(saleRequestPointer);
        }
    }

    [DllImport("TestLib", EntryPoint = "modify_input",
           CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr NativeModifyInputAnsi(IntPtr input);

    public static string? ModifyInputAnsi(string input)
    {
        var inputPointer = Marshal.StringToHGlobalAnsi(input);
        try
        {
            var modifiedInputPointer = NativeModifyInputAnsi(inputPointer);
            return modifiedInputPointer != IntPtr.Zero
                ? Marshal.PtrToStringAnsi(modifiedInputPointer)
                : null;
        }
        finally
        {
            Marshal.FreeHGlobal(inputPointer);
        }
    }

    [DllImport("TestLib", EntryPoint = "modify_input_uni",
           CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private static extern IntPtr NativeModifyInputUnicode(IntPtr input);

    public static string? ModifyInputUnicode(string input)
    {
        var inputPointer = Marshal.StringToHGlobalUni(input);
        try
        {
            var modifiedInputPointer = NativeModifyInputUnicode(inputPointer);
            return modifiedInputPointer != IntPtr.Zero
                ? Marshal.PtrToStringUni(modifiedInputPointer)
                : null;
        }
        finally
        {
            Marshal.FreeHGlobal(inputPointer);
        }
    }
}
