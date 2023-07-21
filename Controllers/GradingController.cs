using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Headers;
using IPR_BE.Services;
using IPR_BE.Models;
using System.Text.Json;
using Serilog;

namespace IPR_BE.Controllers;

[ApiController]
[Route("[controller]")]
public class GradingController : ControllerBase {

    private readonly GradingService _gService;
    public GradingController(GradingService gService) {
        _gService = gService;
    }

    [HttpPut]
    public ActionResult<List<GradedQuestion>> UpdateQuestionGrade(List<GradedQuestion> grades) {
        try {
            grades = _gService.ManualGrade(grades);
            return Ok(grades);
        }
        catch(Exception ex) {
            return Problem(ex.ToString(), Request.Headers.Host, 500);
        }
    }
}