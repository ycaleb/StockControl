namespace StockControl.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string nome);
    }

}
