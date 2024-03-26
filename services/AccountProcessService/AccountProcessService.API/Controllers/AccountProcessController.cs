using AccountProcessService.Application.Contracts.Services;
using AccountProcessService.Application.Models;
using AccountProcessService.Application.Validations.ModelValidators;
using AccountProcessService.Domain.Entities;
using EventBus.Messages.Common;
using EventBus.Messages.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccountProcessService.Api.Controllers
{
    public class AccountProcessController : BaseController
    {
        private readonly ILogger<AccountProcessController> _logger;
        private readonly IAccountProcessService _accountProcessService;
        private readonly IPublisherMq _publishEndpoint;
        //private readonly IValidator<UpdateAccountDto> _updateValidator;
        //private readonly IValidator<CreateAccountsDto> _createValidator;

        public AccountProcessController(
            ILogger<AccountProcessController> logger,
            IAccountProcessService accountProcessService,
            IPublisherMq publishEndpoint
            //,IValidator<UpdateAccountDto> updateValidator,
            //IValidator<CreateAccountsDto> createValidator
            )
        {
            _logger = logger;
            _accountProcessService = accountProcessService;
            _publishEndpoint = publishEndpoint;
            //_updateValidator = updateValidator;
            //_createValidator = createValidator;
        }

        [HttpPost]
        [Route("{agencyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [Produces("application/json")]
        public async Task<IActionResult> Create(Guid agencyId, [FromBody] List<CreateAccountDto> createAccountDto)
        {
            //var validationResult = _createValidator.Validate(createAccountsDto);
            //if (!validationResult.IsValid)
            //{
            //    throw new ValidationException("Validation failed", validationResult.Errors);
            //}

            _accountProcessService.ValidateAccountCreateObject(agencyId, createAccountDto);
            var accounts = _accountProcessService.GetAccountCreateMappingObject(agencyId, createAccountDto);

            //send to no-sql               
            await _accountProcessService.CreateAsync(agencyId, accounts);

            //send to queue
            await _publishEndpoint.PublishAsync(EventBusConstants.AccountCreateQueue,
                    HelperUtility.GetMessageEventObject(EventBusConstants.AccountCreate, accounts));

            return NoContent();
        }

        [HttpPatch]
        [Route("{accountId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [Produces("application/json")]
        public async Task<IActionResult> Update(Guid accountId, [FromBody] UpdateAccountDto accountUpdateDto)
        {
            //var validationResult = _updateValidator.Validate(accountUpdateDto);
            //if (!validationResult.IsValid)
            //{
            //    throw new ValidationException($"Validation failed: {JsonSerializer.Serialize(validationResult.Errors.Select(z => $"{z.PropertyName}: {z.ErrorMessage}"))}");
            //}

            _accountProcessService.ValidateAccountUpdatePayload(accountId, accountUpdateDto);

            var account = await _accountProcessService.LoadAsync<Account>(accountId);
            if (account == null)
            {
                return NotFound();
            }

            _accountProcessService.UpdateAccountDetails(accountUpdateDto, ref account);

            //send to no-sql
            await _accountProcessService.SaveAsync(account);

            //send to queue
            await _publishEndpoint.PublishAsync(EventBusConstants.AccountUpdateQueue,
                    HelperUtility.GetMessageEventObject(EventBusConstants.AccountUpdate, new Account { UId = accountId, Name = accountUpdateDto.Name }));

            return NoContent();
        }

        [HttpDelete]
        [Route("{accountId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(Guid accountId)
        {
            _accountProcessService.ValidateAccountDeletePayload(accountId);

            var account = await _accountProcessService.LoadAsync<Account>(accountId);
            if (account == null)
            {
                return NotFound();
            }

            _accountProcessService.CloseAccount(ref account);

            //send to no-sql
            await _accountProcessService.SaveAsync(account);

            //send to queue
            await _publishEndpoint.PublishAsync(EventBusConstants.AccountDeleteQueue,
                    HelperUtility.GetMessageEventObject(EventBusConstants.AccountDelete, new Account { UId = accountId }));

            return NoContent();
        }
    }
}