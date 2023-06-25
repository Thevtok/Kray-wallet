using go_saku_cs.Models;
using Go_Saku_CS.Repositories;

namespace go_saku_cs.Usecase
{
    public interface IBankUsecase
    {
        List<Bank> GetByUserID(Guid id);
        Bank GetByAccountID(uint id);
        Bank CreateBankAccount(Guid id, Bank newBankAcc);
        Task DeleteByAccountID(uint accountID);
    }
    public class BankUsecase : IBankUsecase { 
    private readonly IBankRepository _bankRepository;
        public BankUsecase(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public List<Bank> GetByUserID(Guid id)
        {
            return _bankRepository.GetByUserID(id);
        }

        public Bank GetByAccountID(uint id)
        {
            return _bankRepository.GetByAccountID(id);
        }

        public Bank CreateBankAccount(Guid id, Bank newBankAcc)
        {
            return _bankRepository.CreateBankAccount(id, newBankAcc);
        }

        public async Task DeleteByAccountID(uint accountID)
        {
            await _bankRepository.DeleteByAccountID(accountID);
        }


    }
}
