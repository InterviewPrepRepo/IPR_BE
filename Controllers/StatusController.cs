using Microsoft.AspNetCore.Mvc;

namespace IPR_BE.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase {

    public StatusController() { }

    [HttpGet]
    public ActionResult checkHealth () {
        return Ok();
    }
    
}