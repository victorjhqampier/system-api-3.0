using Microsoft.AspNetCore.Mvc;

namespace SystemAPI.Controllers.Healthcheck;

[Route("api/[controller]")]
[ApiController]
public class HealthcheckController : ControllerBase
{
    // GET: api/<HealthcheckController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<HealthcheckController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<HealthcheckController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<HealthcheckController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<HealthcheckController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
