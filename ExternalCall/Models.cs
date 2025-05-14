namespace ExternalCall;
public struct SaleResponse
{
    public int SaleId { get; set; }
    public double Amount { get; set; }
};

public struct SaleRequest
{
    public int Value { get; set; }
};
