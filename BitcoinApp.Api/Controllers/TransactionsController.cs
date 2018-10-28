using System.Threading.Tasks;
using BitcoinApp.Api.Models;
using BitcoinApp.Domain.InTransactionAggregate;
using BitcoinApp.Domain.OutTransactionAggregate;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly SendService _sendService;
        private readonly LastInTransactionsService _lastInTransactionsService;

        public TransactionsController(
            SendService sendService,
            LastInTransactionsService lastInTransactionsService)
        {
            _sendService = sendService;
            _lastInTransactionsService = lastInTransactionsService;
        }

        [HttpPost]
        [Route("SendBtc")]
        public async Task<object> SendBtcAsync([FromBody] SendBtcRequest request)
        {
            var (isSucceed, errorMessage) = await _sendService.SendBtcAsync(request.FromWalletId, request.ToWalletAddress, request.Amount);
            return new {isSucceed, errorMessage};
        }

        [HttpGet]
        [Route("GetLast")]
        public async Task<object> GetLastAsync()
        {
            return await _lastInTransactionsService.GetLastAsync();
        }
    }
}
