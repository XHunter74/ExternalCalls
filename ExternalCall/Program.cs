using ExternalCall;

//Test the external library functions with callback
NativeLibFunctions.TestLogCallback(1, (int traceLevel, string traceMessage) =>
{
    var traceLevelStr = traceLevel switch
    {
        0 => "Error",
        1 => "Warning",
        2 => "Info",
        _ => "Unknown"
    };
    Console.WriteLine($"[{traceLevelStr.ToUpper()}] {traceMessage}");
});

//Test the external library functions with params as struct
var saleRequest = new SaleRequest
{
    Value = 1000
};
var saleResponse = NativeLibFunctions.TestSale(saleRequest);
Console.WriteLine($"Sale response: Id: {saleResponse.SaleId}, Amount: {saleResponse.Amount}");

//Test the external library functions with params as ANSI string
const string inputStringAnsi = "Ansi string: Hello, World!";
Console.WriteLine($"Input ANSI string: {inputStringAnsi}");
var outputStringAnsi = NativeLibFunctions.ModifyInputAnsi(inputStringAnsi);
Console.WriteLine($"Modified ANSI string: {outputStringAnsi}");

//Test the external library functions with params as Unicode string
const string inputStringUnicode = "Unicode string: Hello, World!";
Console.WriteLine($"Input string: {inputStringUnicode}");
var outputStringUnicode = NativeLibFunctions.ModifyInputUnicode(inputStringUnicode);
Console.WriteLine($"Modified Unicode string: {outputStringUnicode}");

Console.WriteLine("Multiply values 3 and 9");
var result = NativeLibFunctions.Multiply(3, 9);
Console.WriteLine($"Result: {result}");

Console.WriteLine("Press any key to exit...");
Console.ReadKey(true);