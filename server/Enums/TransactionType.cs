namespace server.Enums;

/// <summary>
/// Санхүүгийн гүйлгээний төрлийг илэрхийлэх жагсаалт.
/// </summary>
public enum TransactionType
{
    /// <summary> Орлого. </summary>
    Deposit = 0,
    /// <summary> Зарлага. </summary>
    Withdraw = 1,
    /// <summary> Шилжүүлэг. </summary>
    Transfer = 2
}