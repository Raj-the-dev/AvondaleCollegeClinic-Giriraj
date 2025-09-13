using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AvondaleCollegeClinic.Models;
using Microsoft.AspNetCore.Identity;

namespace AvondaleCollegeClinic.Areas.Identity.Data
{
    // This is the custom user class for people logging in to the clinic system
    // It is built on top of IdentityUser so it has all the login stuff like Email and Password
    public class AvondaleCollegeClinicUser : IdentityUser
    {
        // First name of the user (like Student, Teacher, Doctor, or Caregiver)
        // This is required so we always know their name
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(30, ErrorMessage = "First Name must be at most 30 characters long.")]
        public string FirstName { get; set; }

        // Last name of the user
        // Also required because it is part of their identity
        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(30, ErrorMessage = "Last Name must be at most 30 characters long.")]
        public string LastName { get; set; }


        // A simple security keyword that the user can set
        // For example: "What city were you born in?"
        [MaxLength(40, ErrorMessage = "Security keyword must be at most 40 characters long.")]
        public string? CityOfBirth { get; set; }

        // A link to their profile picture if available (like from Student or Doctor model)
        [MaxLength(256)]
        public string? AvatarPath { get; set; }

        // Flag that tells us if the user must set their password
        // This is true the first time they log in (like for students/teachers)
        public bool MustSetPassword { get; set; } = false;

        // Shows what kind of user this is
        // Could be Admin, Student, Teacher, Doctor, Caregiver
        public UserKind UserKind { get; set; } = UserKind.Admin;

        // Links to the actual model records (only one will be used depending on type)
        public Student? StudentProfile { get; set; }
        public Teacher? TeacherProfile { get; set; }
        public Doctor? DoctorProfile { get; set; }
        public Caregiver? CaregiverProfile { get; set; }
    }

    // Enum to easily check what type of user someone is
    public enum UserKind
    {
        Admin,
        Student,
        Teacher,
        Doctor,
        Caregiver
    }
}