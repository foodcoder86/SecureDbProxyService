using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserAppTelemetry
{
    [Key]  // Assuming there's a primary key (if not, modify this)
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Auto-incremented ID (if needed)

    [Required]
    [MaxLength(255)]  // Adjust max length based on your actual column size
    public string ComputerName { get; set; }

    [Required]
    [MaxLength(255)]
    public string UserID { get; set; }

    [Required]
    [MaxLength(255)]
    public string ProcessName { get; set; }

    public int ProcessID { get; set; }

    [Required]
    public DateTime EntryDateTime { get; set; }
}
//Why This Structure?

//    [Key] – Defines the primary key (needed for EF Core, adjust if your table doesn't have a primary key).
//    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] – Auto - generates ID if the table supports it.
//    [Required] – Ensures fields are mandatory.
//    [MaxLength(255)] – Matches VARCHAR constraints to prevent EF from creating larger fields in migrations.
//    DateTime EntryDateTime – EF automatically maps timestamp fields to DateTime.

//2. Update AppDbContext to Include the New Table