using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace RapidPayPayment.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CreditCardController : ControllerBase
    {
        private readonly ILogger<CreditCardController> _logger;
        private IUserService _userService;
        private ICreditCardRepo _creditCardRepo;

        public CreditCardController(
            ILogger<CreditCardController> logger, 
            IUserService userService, 
            ICreditCardRepo creditCardRepo)
        {
            _creditCardRepo = creditCardRepo;
            _userService = userService;
            _logger = logger;
        }
        
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        // GET api/CreditCard/ {15digits}
        [HttpGet("{cardNumber:regex([[0-9]]{{15}})}")]
        public IActionResult GetBalance(string cardNumber)
        {
            var card = _creditCardRepo.GetCreditCard(cardNumber);
            if (card == null)
                return NotFound();
            return Ok(card.Balance);
        }

        // POST api/CreditCard
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] CreditCard card)
        {
            if ((card?.CardNumber?.Length ?? 0) == 15)
            {
                _creditCardRepo.AddCreditCard(card);
                return StatusCode(201, "Credit Card created succesfully");
            }
            return BadRequest("Card number already exists");
        }

        // PUT api/CreditCard/ {15 digits}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{cardNumber:regex([[0-9]]{{15}})}")]
        public IActionResult Pay([FromRoute] string cardNumber, [FromBody]double amount)
        {
            var card = _creditCardRepo.GetCreditCard(cardNumber);
            if (card == null)
                return BadRequest();

            if (amount <= 0)
                return StatusCode(500, "Amount canot be less than 0");

            var ok = _creditCardRepo.Pay(cardNumber, amount);

            if (ok)
                return Ok();

            return StatusCode(500, "There was an error trying to pay");
        }
    }
}
