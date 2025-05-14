using System.Runtime.InteropServices;

namespace ExternalCall;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal delegate void IntTraceCallbackDelegate(int traceLevel, IntPtr traceMessagePointer);

public delegate void TraceCallback(int traceLevel, string traceMessage);

public static class ExtLibFunctions
{
    [DllImport("TestLib.dll", EntryPoint = "test_log_callback",
           CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void TestLogCallbacExt(int traceLevel, IntPtr traceCallback);

    public static void TestLogCallback(int traceLevel, TraceCallback traceCallback)
    {
        IntTraceCallbackDelegate traceDelegate = (int level, IntPtr messagePointer) =>
        {
            var message = messagePointer != IntPtr.Zero 
                ? Marshal.PtrToStringAnsi(messagePointer) 
                : null;
            traceCallback(level, message!);
        };

        var traceDelegatePointer = Marshal
            .GetFunctionPointerForDelegate(traceDelegate);
        TestLogCallbacExt(traceLevel, traceDelegatePointer);
    }

    [DllImport("TestLib.dll", EntryPoint = "test_sale",
           CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr TestSaleExt(IntPtr saleRequest);

    public static SaleResponse TestSale(SaleRequest saleRequest)
    {
        var saleRequestPointer = Marshal.AllocHGlobal(Marshal.SizeOf(saleRequest));
        Marshal.StructureToPtr(saleRequest, saleRequestPointer, false);
        try
        {
            var saleResponsePointer = TestSaleExt(saleRequestPointer);
            return Marshal.PtrToStructure<SaleResponse>(saleResponsePointer)!;
        }
        finally
        {
            Marshal.FreeHGlobal(saleRequestPointer);
        }
    }

    [DllImport("TestLib.dll", EntryPoint = "modify_input",
           CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr ModifyInputAnsiExt(IntPtr input);

    public static string? ModifyInputAnsi(string input)
    {
        var inputPointer = Marshal.StringToHGlobalAnsi(input);
        try
        {
            var modifiedInputPointer = ModifyInputAnsiExt(inputPointer);
            return modifiedInputPointer != IntPtr.Zero
                ? Marshal.PtrToStringAnsi(modifiedInputPointer)
                : null;
        }
        finally
        {
            Marshal.FreeHGlobal(inputPointer);
        }
    }

    [DllImport("TestLib.dll", EntryPoint = "modify_input_uni",
           CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private static extern IntPtr ModifyInputUniExt(IntPtr input);

    public static string? ModifyInputUni(string input)
    {
        var inputPointer = Marshal.StringToHGlobalUni(input);
        try
        {
            var modifiedInputPointer = ModifyInputUniExt(inputPointer);
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
