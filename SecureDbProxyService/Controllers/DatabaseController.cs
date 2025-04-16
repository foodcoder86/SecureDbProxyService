using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize]  // Requires authentication
[ApiController]
[Route("api/database")]
public class DatabaseController : ControllerBase
{
    private readonly AppDbContext _context;

    public DatabaseController(AppDbContext context)
    {
        _context = context;
    }

    // Securely fetch customer data
    [HttpGet("customers")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return await _context.Customers.ToListAsync();
    }
}
