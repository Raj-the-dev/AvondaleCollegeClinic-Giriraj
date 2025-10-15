using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AvondaleCollegeClinic.Validation;   // custom NotFuture


namespace AvondaleCollegeClinic.Models
{
    public class Labtest
    {
        [Key]
        public int LabtestID { get; set; } // primary key

        [Required]
        [Display(Name = "Medical Record")]
        public int RecordID { get; set; }  // FK to MedicalRecord 

        [Required, StringLength(100)]
        [Display(Name = "Test Type")]
        public string TestType { get; set; } // e.g., "Blood Test", "X-Ray"

        // We store ONLY the relative URL/path in the DB (file lives on disk/cloud).
        // Example: "/files/labtests/abcd_report.pdf"
        [Required]
        [Display(Name = "Report (PDF)")]
        public string File { get; set; }

        // DateOnly = date without time 
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Result Date")]
        [NotFuture(ErrorMessage = "Result date cannot be in the future.")]
        public DateOnly ResultDate { get; set; }

        // Upload helper: the posted PDF from the form, NOT saved in DB.
        [NotMapped]                 // EF should ignore this property 
        public IFormFile PDFFile { get; set; }

        //lets EF load the related MedicalRecord.
        public MedicalRecord MedicalRecord { get; set; }
    }
}
