using Exchange.Client.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Exchange.Client.WebService.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class QuotesController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;

    public QuotesController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult GetQuotes()
    {
        return Ok(_unitOfWork.TickerRepository.Get());
    }

    [HttpGet]
    public IActionResult Delete(int quoteId)
    {
        _unitOfWork.TickerRepository.Delete(quoteId);
        _unitOfWork.Save();
        return Ok();
    }

    [HttpPost]
    public IActionResult Create([FromBody] Ticker quote)
    {
        _unitOfWork.TickerRepository.Insert(quote);
        _unitOfWork.Save();
        return Ok();
    }

    [HttpPut]
    public IActionResult Update([FromBody] Ticker quote)
    {
        _unitOfWork.TickerRepository.Update(quote);
        _unitOfWork.Save();
        return Ok();
    }
}