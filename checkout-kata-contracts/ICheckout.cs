namespace checkout_kata_contracts
{
    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }
}