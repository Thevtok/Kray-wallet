using Go_Saku.Net.Data;

using go_saku_cs.Models;


namespace Go_Saku_CS.Repositories
{
    public interface IBankRepository
    {
        List<Bank> GetByUserID(Guid id);
        Bank GetByAccountID(uint id);
        Bank CreateBankAccount(Guid id, Bank newBankAcc);
        Task DeleteByAccountID(uint accountID);
    }
    public class BankRepository : IBankRepository
    {
        private readonly UserApiDbContext _dbContext;

        public BankRepository(UserApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public List<Bank> GetByUserID(Guid id)
        {
            return _dbContext.Banks.Where(bank => bank.UserID == id)
                .Select(bank => new Bank
                {
                    UserID = bank.UserID,
                    BankName = bank.BankName,
                    AccountId = bank.AccountId,
                    AccountHolderName = bank.AccountHolderName,
                    AccountNumber = bank.AccountNumber
                })
                .ToList();
        }


        public Bank GetByAccountID(uint id)
        {
            return _dbContext.Banks
                .FirstOrDefault(b => b.AccountId == id);
        }

        public Bank CreateBankAccount(Guid id, Bank newBankAcc)
        {
            var bankAcc = new Bank
            {
                UserID = id,
                BankName = newBankAcc.BankName,
                AccountNumber = newBankAcc.AccountNumber,
                AccountHolderName = newBankAcc.AccountHolderName
            };

            _dbContext.Banks.Add(bankAcc);
            _dbContext.SaveChanges();

            return bankAcc;
        }

        public async Task DeleteByAccountID(uint accountID)
        {
            var bankAcc = await _dbContext.Banks.FindAsync(accountID);
            if (bankAcc != null)
            {
                _dbContext.Banks.Remove(bankAcc);
                await _dbContext.SaveChangesAsync();
            }
        }


    }
}
