using Go_Saku.Net.Models;
using Go_Saku.Net.Usecase;
using Go_Saku.Net.Utils;
using go_saku_cs.Models;
using go_saku_cs.Usecase;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace go_saku_cs.Controllers
{

    [ApiController]
    [Route("api/bank")]
    public class BankController : ControllerBase
    {
        private readonly IBankUsecase _bankUsecase;

        public BankController(IBankUsecase bankUsecase)
        {
        _bankUsecase = bankUsecase;
        }

        [HttpGet("userid/{user_id}")]
        public ActionResult<IEnumerable<Bank>> GetByUserId(Guid user_id)
        {
            var users = _bankUsecase.GetByUserID(user_id);
            ResponseUtils.JSONSuccess(HttpContext, true, (int)HttpStatusCode.OK, users);

            return new EmptyResult();
        }

        [HttpGet("accountid/{user_id}/{account_id}")]
        public ActionResult<Bank> GetByAccountId(uint account_id)
        {
            var users = _bankUsecase.GetByAccountID(account_id);
            ResponseUtils.JSONSuccess(HttpContext, true, (int)HttpStatusCode.OK, users);

            return new EmptyResult();
        }

        [HttpPost("create/{user_id}")]
        public async Task<IActionResult> AddBank(Guid id, Bank bank)
        {
            Bank createdBank =  _bankUsecase.CreateBankAccount(id, bank);
            ResponseUtils.JSONSuccess(HttpContext, true, (int)HttpStatusCode.Created, createdBank);

            return new EmptyResult();
        }

        [HttpDelete("delete/{account_id}")]
        public async Task<IActionResult> DeleteBank(uint accountID)
        {
            await _bankUsecase.DeleteByAccountID(accountID);
            ResponseUtils.JSONSuccess(HttpContext, true, (int)HttpStatusCode.OK, "Bank account deleted successfully");

            return new EmptyResult();
        }




    }
}
