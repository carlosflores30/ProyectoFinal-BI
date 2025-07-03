namespace SmartStockAI.Application.Interfaces.Authentication;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hashed);
}