using EduCycle.Application.Interfaces;
using EduCycle.Contracts.Messages;
using EduCycle.Contracts.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduCycle.Api.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;
    private readonly IMessageService _messageService;

    public TransactionsController(ITransactionService service, IMessageService messageService)
    {
        _service = service;
        _messageService = messageService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionRequest request)
    {
        var buyerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _service.CreateAsync(request, buyerId));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMyTransactions()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _service.GetMyTransactionsAsync(userId));
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateTransactionStatusRequest request)
    {
        return Ok(await _service.UpdateStatusAsync(id, request));
    }

    [HttpPost("{id:guid}/otp")]
    public async Task<IActionResult> GenerateOtp(Guid id)
    {
        return Ok(await _service.GenerateOtpAsync(id));
    }

    [HttpPost("{id:guid}/verify-otp")]
    public async Task<IActionResult> VerifyOtp(Guid id, [FromBody] TransactionVerifyOtpRequest request)
    {
        await _service.VerifyOtpAsync(id, request.Otp);
        return Ok(new { message = "OTP verified successfully" });
    }

    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> ConfirmReceipt(Guid id)
    {
        return Ok(await _service.ConfirmReceiptAsync(id));
    }

    // ===== Messages =====

    [HttpGet("{transactionId:guid}/messages")]
    public async Task<IActionResult> GetMessages(Guid transactionId)
    {
        return Ok(await _messageService.GetByTransactionIdAsync(transactionId));
    }

    [HttpPost("{transactionId:guid}/messages")]
    public async Task<IActionResult> SendMessage(Guid transactionId, SendMessageRequest request)
    {
        var senderId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _messageService.SendAsync(transactionId, request, senderId));
    }
}

public class TransactionVerifyOtpRequest
{
    public string Otp { get; set; } = null!;
}
