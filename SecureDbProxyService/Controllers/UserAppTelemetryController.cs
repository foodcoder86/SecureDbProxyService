using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize]  // Enforce JWT authentication
[ApiController]
[Route("api/userapptelemetry")]
public class UserAppTelemetryController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserAppTelemetryController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ Get all telemetry records (READ ALL)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserAppTelemetry>>> GetTelemetryRecords()
    {
        return await _context.UserAppTelemetry.ToListAsync();
    }

    // ✅ Get a single telemetry record by ID (READ ONE)
    [HttpGet("{id}")]
    public async Task<ActionResult<UserAppTelemetry>> GetTelemetryRecord(int id)
    {
        var record = await _context.UserAppTelemetry.FindAsync(id);
        if (record == null) return NotFound();
        return record;
    }

    // ✅ Add a new telemetry record (INSERT)
    [HttpPost]
    public async Task<ActionResult<UserAppTelemetry>> CreateTelemetryRecord(UserAppTelemetry record)
    {
        _context.UserAppTelemetry.Add(record);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTelemetryRecord), new { id = record.Id }, record);
    }
}
