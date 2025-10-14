using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AvondaleCollegeClinic.Validation;
using Microsoft.AspNetCore.Http;
using AvondaleCollegeClinic.Validation;

namespace AvondaleCollegeClinic.Models
{
    public class Labtest
    {
        [Key]
        public int LabtestID { get; set; }

        [Required]
        [Display(Name = "Medical Record")]
        public int RecordID { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Test Type")]
        public string TestType { get; set; }

        // Store the relative URL to the PDF, e.g. "/files/labtests/abcd_report.pdf"
        [Required]
        [Display(Name = "Report (PDF)")]
        public string File { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Result Date")]
        [NotFuture(ErrorMessage = "Result date cannot be in the future.")]
        public DateOnly ResultDate { get; set; }

        [NotMapped]
        public IFormFile PDFFile { get; set; }

        public MedicalRecord MedicalRecord { get; set; }

    }
}
