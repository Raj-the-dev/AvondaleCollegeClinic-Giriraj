using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AvondaleCollegeClinic.Areas.Identity.Data;

public class AvondaleCollegeClinicContext : IdentityDbContext<AvondaleCollegeClinicUser>
{
    public AvondaleCollegeClinicContext(DbContextOptions<AvondaleCollegeClinicContext> options)
        : base(options)
    { }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Labtest> LabTests { get; set; }
    public DbSet<Homeroom> Homerooms { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Caregiver> Caregivers { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- Link Student -> Identity user (1:1) ----
        modelBuilder.Entity<Student>()
            .HasOne(s => s.AvondaleCollegeClinicUserAccount)      // a student has one linked Identity user
            .WithOne(u => u.StudentProfile)                       // that Identity user has one student profile
            .HasForeignKey<Student>(s => s.IdentityUserId)        // use Student.IdentityUserId as the FK column
            .OnDelete(DeleteBehavior.SetNull);                    // if the Identity user is deleted, keep the student and set FK to null

        // Same pattern for Teacher, Doctor, Caregiver
        modelBuilder.Entity<Teacher>()
            .HasOne(t => t.AvondaleCollegeClinicUserAccount)
            .WithOne(u => u.TeacherProfile)
            .HasForeignKey<Teacher>(t => t.IdentityUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.AvondaleCollegeClinicUserAccount)
            .WithOne(u => u.DoctorProfile)
            .HasForeignKey<Doctor>(d => d.IdentityUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Caregiver>()
            .HasOne(c => c.AvondaleCollegeClinicUserAccount)
            .WithOne(u => u.CaregiverProfile)
            .HasForeignKey<Caregiver>(c => c.IdentityUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // ---------------------------
        // Seed Admin role + user
        // ---------------------------

        // Fixed IDs so we can reference them in seed data and future migrations
        const string ADMIN_ROLE_ID = "00000000-0000-0000-0000-000000000010";
        const string ADMIN_USER_ID = "00000000-0000-0000-0000-000000000001";

        // Create the Admin role record
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = ADMIN_ROLE_ID,
            Name = "Admin",
            NormalizedName = "ADMIN"
        });

        // Build a default Admin user object
        var hasher = new PasswordHasher<AvondaleCollegeClinicUser>();
        var admin = new AvondaleCollegeClinicUser
        {
            Id = ADMIN_USER_ID,
            UserName = "admin@avondaleclinic.com",
            NormalizedUserName = "ADMIN@AVONDALECLINIC.COM",
            Email = "admin@avondaleclinic.com",
            NormalizedEmail = "ADMIN@AVONDALECLINIC.COM",
            EmailConfirmed = true,     // skip email confirmation for this seed account
            FirstName = "System",
            LastName = "Admin",
            UserKind = UserKind.Admin, // your custom enum or field that marks this as an Admin
            MustSetPassword = false    // do not force a reset on first login
        };

        // Hash the password before seeding (never store plain text)
        admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

        // Save the user into the Identity users table
        modelBuilder.Entity<AvondaleCollegeClinicUser>().HasData(admin);

        // Give the Admin user the Admin role by inserting into the join table
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = ADMIN_ROLE_ID,
            UserId = ADMIN_USER_ID
        });

        // SEEDED DATE FOR ALL MODELS
        modelBuilder.Entity<Student>().HasData(
            new Student { StudentID = "ac250001", FirstName = "Sam", LastName = "Hill", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2007, 10, 14), Email = "sam.hill@school.com", HomeroomID = "hr250001" },
            new Student { StudentID = "ac250002", FirstName = "Lily", LastName = "Evans", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2008, 12, 26), Email = "lily.evans@school.com", HomeroomID = "hr250003" },
            new Student { StudentID = "ac250003", FirstName = "Jake", LastName = "Smith", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2009, 11, 28), Email = "jake.smith@school.com", HomeroomID = "hr250003" },
            new Student { StudentID = "ac250004", FirstName = "Emma", LastName = "Johnson", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2007, 9, 4), Email = "emma.johnson@school.com", HomeroomID = "hr250001" },
            new Student { StudentID = "ac250005", FirstName = "Mia", LastName = "Brown", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2008, 1, 7), Email = "mia.brown@school.com", HomeroomID = "hr250003" },
            new Student { StudentID = "ac250006", FirstName = "Noah", LastName = "Taylor", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2009, 1, 22), Email = "noah.taylor@school.com", HomeroomID = "hr250002" },
            new Student { StudentID = "ac250007", FirstName = "Olivia", LastName = "Anderson", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2007, 12, 23), Email = "olivia.anderson@school.com", HomeroomID = "hr250001" },
            new Student { StudentID = "ac250008", FirstName = "Liam", LastName = "Thomas", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2008, 12, 7), Email = "liam.thomas@school.com", HomeroomID = "hr250001" },
            new Student { StudentID = "ac250009", FirstName = "Ava", LastName = "Jackson", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2009, 4, 26), Email = "ava.jackson@school.com", HomeroomID = "hr250002" },
            new Student { StudentID = "ac250010", FirstName = "Ethan", LastName = "White", ImagePath = "/images/students/1.jpg", DOB = new DateTime(2007, 7, 4), Email = "ethan.white@school.com", HomeroomID = "hr250002" }
        );


        modelBuilder.Entity<Caregiver>().HasData(
            new Caregiver
            {
                CaregiverID = "acc250001",
                FirstName = "Emma",
                LastName = "Wilson",
                DOB = new DateTime(1980, 6, 12),
                Email = "emma.wilson@email.com",
                Phone = "021-123-4567",
                Relationship = RelationshipType.Parent,
                ImagePath = "/images/caregivers/1.jpeg"
            },
            new Caregiver
            {
                CaregiverID = "acc250002",
                FirstName = "John",
                LastName = "Evans",
                DOB = new DateTime(1975, 3, 8),
                Email = "john.evans@email.com",
                Phone = "022-987-6543",
                Relationship = RelationshipType.Parent,
                ImagePath = "/images/caregivers/2.jpeg"
            },
            new Caregiver
            {
                CaregiverID = "acc250003",
                FirstName = "Maya",
                LastName = "Singh",
                DOB = new DateTime(1985, 9, 20),
                Email = "maya.singh@email.com",
                Phone = "021-555-8899",
                Relationship = RelationshipType.Guardian,
                ImagePath = "/images/caregivers/3.jpeg"
            },
            new Caregiver
            {
                CaregiverID = "acc250004",
                FirstName = "Chris",
                LastName = "Brown",
                DOB = new DateTime(1979, 12, 4),
                Email = "chris.brown@email.com",
                Phone = "021-333-2222",
                Relationship = RelationshipType.Sibling,
                ImagePath = "/images/caregivers/5.jepg.jpg"
            },
            new Caregiver
            {
                CaregiverID = "acc250005",
                FirstName = "Sophie",
                LastName = "Nguyen",
                DOB = new DateTime(1982, 7, 17),
                Email = "sophie.nguyen@email.com",
                Phone = "022-111-5555",
                Relationship = RelationshipType.Parent,
                ImagePath = "/images/caregivers/6.jpg"
            },
            new Caregiver
            {
                CaregiverID = "acc250006",
                FirstName = "Liam",
                LastName = "King",
                DOB = new DateTime(1973, 11, 25),
                Email = "liam.king@email.com",
                Phone = "021-456-7890",
                Relationship = RelationshipType.Other,
                ImagePath = "/images/caregivers/7.jpg"
            },
            new Caregiver
            {
                CaregiverID = "acc250007",
                FirstName = "Olivia",
                LastName = "Rao",
                DOB = new DateTime(1988, 2, 14),
                Email = "olivia.rao@email.com",
                Phone = "021-999-8888",
                Relationship = RelationshipType.Parent,
                ImagePath = "/images/caregivers/8.jpg"
            },
            new Caregiver
            {
                CaregiverID = "acc250008",
                FirstName = "Ethan",
                LastName = "Lee",
                DOB = new DateTime(1981, 5, 5),
                Email = "ethan.lee@email.com",
                Phone = "022-123-9999",
                Relationship = RelationshipType.Guardian,
                ImagePath = "/images/caregivers/9.jpg"
            },
            new Caregiver
            {
                CaregiverID = "acc250009",
                FirstName = "Grace",
                LastName = "Taylor",
                DOB = new DateTime(1986, 10, 30),
                Email = "grace.taylor@email.com",
                Phone = "021-234-5678",
                Relationship = RelationshipType.Parent,
                ImagePath = "/images/caregivers/10.jpg"
            },
            new Caregiver
            {
                CaregiverID = "acc250010",
                FirstName = "Raj",
                LastName = "Patel",
                DOB = new DateTime(1977, 8, 22),
                Email = "raj.patel@email.com",
                Phone = "022-777-3333",
                Relationship = RelationshipType.Other,
                ImagePath = "/images/caregivers/11.jpg"
            }
        );

        modelBuilder.Entity<Doctor>().HasData(
            new Doctor
            {
                DoctorID = "acd250001",
                FirstName = "Anna",
                LastName = "Roberts",
                Specialization = SpecializationType.General,
                Email = "anna.roberts@avondaleclinic.com",
                Phone = "021-111-2345",
                ImagePath = "/images/doctors/1.jpg",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            },
            new Doctor
            {
                DoctorID = "acd250002",
                FirstName = "Ben",
                LastName = "Morris",
                Specialization = SpecializationType.Pediatric,
                Email = "ben.morris@avondaleclinic.com",
                Phone = "021-222-3456",
                ImagePath = "/images/doctors/2.jpg",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            },
            new Doctor
            {
                DoctorID = "acd250003",
                FirstName = "Claire",
                LastName = "Nguyen",
                Specialization = SpecializationType.MentalHealth,
                Email = "claire.nguyen@avondaleclinic.com",
                Phone = "021-333-4567",
                ImagePath = "/images/doctors/3.png",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            },
            new Doctor
            {
                DoctorID = "acd250004",
                FirstName = "David",
                LastName = "Chen",
                Specialization = SpecializationType.SportsMedicine,
                Email = "david.chen@avondaleclinic.com",
                Phone = "021-444-5678",
                ImagePath = "/images/doctors/4.png",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            },
            new Doctor
            {
                DoctorID = "acd250005",
                FirstName = "Ella",
                LastName = "Turner",
                Specialization = SpecializationType.Dermatology,
                Email = "ella.turner@avondaleclinic.com",
                Phone = "021-555-6789",
                ImagePath = "/images/doctors/5.jpg",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            },
            new Doctor
            {
                DoctorID = "acd250006",
                FirstName = "Frank",
                LastName = "White",
                Specialization = SpecializationType.General,
                Email = "frank.white@avondaleclinic.com",
                Phone = "021-666-7890",
                ImagePath = "/images/doctors/6.png",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            },
            new Doctor
            {
                DoctorID = "acd250007",
                FirstName = "Grace",
                LastName = "Lee",
                Specialization = SpecializationType.Pediatric,
                Email = "grace.lee@avondaleclinic.com",
                Phone = "021-777-8901",
                ImagePath = "/images/doctors/7.jpg",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            },
            new Doctor
            {
                DoctorID = "acd250008",
                FirstName = "Harry",
                LastName = "Singh",
                Specialization = SpecializationType.MentalHealth,
                Email = "harry.singh@avondaleclinic.com",
                Phone = "021-888-9012",
                ImagePath = "/images/doctors/8.jpg",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            },
            new Doctor
            {
                DoctorID = "acd250009",
                FirstName = "Isla",
                LastName = "Walker",
                Specialization = SpecializationType.SportsMedicine,
                Email = "isla.walker@avondaleclinic.com",
                Phone = "021-999-0123",
                ImagePath = "/images/doctors/9.jpg",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            },
            new Doctor
            {
                DoctorID = "acd250010",
                FirstName = "Jack",
                LastName = "Patel",
                Specialization = SpecializationType.Dermatology,
                Email = "jack.patel@avondaleclinic.com",
                Phone = "021-000-1234",
                ImagePath = "/images/doctors/10.jpg",
                WorksMon = true,
                WorksTue = true,
                WorksWed = true,
                WorksThu = true,
                WorksFri = true,
                WorksSat = false,
                WorksSun = false,
                DailyStartTime = new TimeSpan(9, 0, 0),
                DailyEndTime = new TimeSpan(17, 0, 0),
                SlotMinutes = 30
            }
        );



        modelBuilder.Entity<Appointment>().HasData(
            new Appointment
            {
                AppointmentID = 1,
                StudentID = "ac250001",
                DoctorID = "acd250001",
                AppointmentDateTime = new DateTime(2025, 4, 15, 10, 30, 0),
                Status = AppointmentStatus.Confirmed,
                Reason = "General check-up"
            },
            new Appointment
            {
                AppointmentID = 2,
                StudentID = "ac250002",
                DoctorID = "acd250002",
                AppointmentDateTime = new DateTime(2025, 4, 16, 11, 0, 0),
                Status = AppointmentStatus.Pending,
                Reason = "Follow-up on injury"
            },
            new Appointment
            {
                AppointmentID = 3,
                StudentID = "ac250003",
                DoctorID = "acd250003",
                AppointmentDateTime = new DateTime(2025, 4, 17, 9, 15, 0),
                Status = AppointmentStatus.Completed,
                Reason = "Routine blood test"
            },
            new Appointment
            {
                AppointmentID = 4,
                StudentID = "ac250004",
                DoctorID = "acd250004",
                AppointmentDateTime = new DateTime(2025, 4, 18, 14, 0, 0),
                Status = AppointmentStatus.Confirmed,
                Reason = "Skin allergy consultation"
            },
            new Appointment
            {
                AppointmentID = 5,
                StudentID = "ac250005",
                DoctorID = "acd250005",
                AppointmentDateTime = new DateTime(2025, 4, 19, 15, 30, 0),
                Status = AppointmentStatus.Cancelled,
                Reason = "Sore throat"
            },
            new Appointment
            {
                AppointmentID = 6,
                StudentID = "ac250006",
                DoctorID = "acd250006",
                AppointmentDateTime = new DateTime(2025, 4, 20, 13, 0, 0),
                Status = AppointmentStatus.Pending,
                Reason = "Mental health support"
            },
            new Appointment
            {
                AppointmentID = 7,
                StudentID = "ac250007",
                DoctorID = "acd250007",
                AppointmentDateTime = new DateTime(2025, 4, 21, 10, 45, 0),
                Status = AppointmentStatus.Confirmed,
                Reason = "Back pain evaluation"
            },
            new Appointment
            {
                AppointmentID = 8,
                StudentID = "ac250008",
                DoctorID = "acd250008",
                AppointmentDateTime = new DateTime(2025, 4, 22, 9, 0, 0),
                Status = AppointmentStatus.Confirmed,
                Reason = "Yearly physical"
            },
            new Appointment
            {
                AppointmentID = 9,
                StudentID = "ac250009",
                DoctorID = "acd250009",
                AppointmentDateTime = new DateTime(2025, 4, 23, 11, 30, 0),
                Status = AppointmentStatus.Completed,
                Reason = "Follow-up on flu"
            },
            new Appointment
            {
                AppointmentID = 10,
                StudentID = "ac250010",
                DoctorID = "acd250010",
                AppointmentDateTime = new DateTime(2025, 4, 24, 12, 15, 0),
                Status = AppointmentStatus.Pending,
                Reason = "Foot injury assessment"
            }
        );
        modelBuilder.Entity<MedicalRecord>().HasData(
            new MedicalRecord
            {
                MedicalRecordID = 1,
                StudentID = "ac250001",
                DoctorID = "acd250001",
                Notes = "Routine annual check-up. No issues found.",
                Date = new DateTime(2025, 3, 1)
            },
            new MedicalRecord
            {
                MedicalRecordID = 2,
                StudentID = "ac250002",
                DoctorID = "acd250002",
                Notes = "Reviewed knee sprain. Advised rest and light exercises.",
                Date = new DateTime(2025, 3, 3)
            },
            new MedicalRecord
            {
                MedicalRecordID = 3,
                StudentID = "ac250003",
                DoctorID = "acd250003",
                Notes = "Blood test completed. Results within normal range.",
                Date = new DateTime(2025, 3, 5)
            },
            new MedicalRecord
            {
                MedicalRecordID = 4,
                StudentID = "ac250004",
                DoctorID = "acd250004",
                Notes = "Observed rash on arms. Prescribed topical cream.",
                Date = new DateTime(2025, 3, 6)
            },
            new MedicalRecord
            {
                MedicalRecordID = 5,
                StudentID = "ac250005",
                DoctorID = "acd250005",
                Notes = "Reported headache and fatigue. Referred for lab tests.",
                Date = new DateTime(2025, 3, 8)
            },
            new MedicalRecord
            {
                MedicalRecordID = 6,
                StudentID = "ac250006",
                DoctorID = "acd250006",
                Notes = "Initial consultation for anxiety symptoms. Scheduled follow-up.",
                Date = new DateTime(2025, 3, 10)
            },
            new MedicalRecord
            {
                MedicalRecordID = 7,
                StudentID = "ac250007",
                DoctorID = "acd250007",
                Notes = "Complaints of lower back pain. Stretching advised.",
                Date = new DateTime(2025, 3, 11)
            },
            new MedicalRecord
            {
                MedicalRecordID = 8,
                StudentID = "ac250008",
                DoctorID = "acd250008",
                Notes = "Regular check-up completed. Advised hydration.",
                Date = new DateTime(2025, 3, 12)
            },
            new MedicalRecord
            {
                MedicalRecordID = 9,
                StudentID = "ac250009",
                DoctorID = "acd250009",
                Notes = "Follow-up after flu recovery. Patient doing well.",
                Date = new DateTime(2025, 3, 13)
            },
            new MedicalRecord
            {
                MedicalRecordID = 10,
                StudentID = "ac250010",
                DoctorID = "acd250010",
                Notes = "Foot strain reviewed. Rest and ice recommended.",
                Date = new DateTime(2025, 3, 14)
            }
        );

        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { TeacherID = "act250001", FirstName = "Mr. Vijay", LastName = "Prasad", Email = "v.Prasad@avondale.school.nz", TeacherCode = "OPA", ImagePath = "/images/teachers/MrPrasad.jpg" },
            new Teacher { TeacherID = "act250002", FirstName = "James", LastName = "Ngata", Email = "james.ngata@avondale.school.nz", TeacherCode = "JNG", ImagePath = "/images/teachers/Principle.jpeg" },
            new Teacher { TeacherID = "act250003", FirstName = "Sophia", LastName = "Lee", Email = "sophia.lee@avondale.school.nz", TeacherCode = "SLE", ImagePath = "/images/teachers/5.jpg" },
            new Teacher { TeacherID = "act250004", FirstName = "Ethan", LastName = "White", Email = "ethan.white@avondale.school.nz", TeacherCode = "EWH", ImagePath = "/images/teachers/6.jpg" },
            new Teacher { TeacherID = "act250005", FirstName = "Ava", LastName = "Singh", Email = "ava.singh@avondale.school.nz", TeacherCode = "ASI", ImagePath = "/images/teachers/7.jpg" },
            new Teacher { TeacherID = "act250006", FirstName = "William", LastName = "Morris", Email = "william.morris@avondale.school.nz", TeacherCode = "WMO", ImagePath = "/images/teachers/8.jpg" },
            new Teacher { TeacherID = "act250007", FirstName = "Isabella", LastName = "Tao", Email = "isabella.tao@avondale.school.nz", TeacherCode = "ITA", ImagePath = "/images/teachers/9.jpg" },
            new Teacher { TeacherID = "act250008", FirstName = "Lucas", LastName = "Patel", Email = "lucas.patel@avondale.school.nz", TeacherCode = "LPA", ImagePath = "/images/teachers/10.jpg" },
            new Teacher { TeacherID = "act250009", FirstName = "Emily", LastName = "Brown", Email = "emily.brown@avondale.school.nz", TeacherCode = "EBR", ImagePath = "/images/teachers/2.jpg" },
            new Teacher { TeacherID = "act250010", FirstName = "Daniel", LastName = "King", Email = "daniel.king@avondale.school.nz", TeacherCode = "DKG", ImagePath = "/images/teachers/3.jpg" }
        );

        modelBuilder.Entity<Homeroom>().HasData(
            new Homeroom
            {
                HomeroomID = "hr250001",
                YearLevel = YearLevel._9,
                TeacherID = "act250001",
                Block = Block.A,
                ClassNumber = ClassNumber._1
            },
            new Homeroom
            {
                HomeroomID = "hr250002",
                YearLevel = YearLevel._10,
                TeacherID = "act250002",
                Block = Block.B,
                ClassNumber = ClassNumber._2
            },
            new Homeroom
            {
                HomeroomID = "hr250003",
                YearLevel = YearLevel._11,
                TeacherID = "act250003",
                Block = Block.C,
                ClassNumber = ClassNumber._3
            },
            new Homeroom
            {
                HomeroomID = "hr250004",
                YearLevel = YearLevel._12,
                TeacherID = "act250004",
                Block = Block.D,
                ClassNumber = ClassNumber._4
            },
            new Homeroom
            {
                HomeroomID = "hr250005",
                YearLevel = YearLevel._13,
                TeacherID = "act250005",
                Block = Block.E,
                ClassNumber = ClassNumber._5
            },
            new Homeroom
            {
                HomeroomID = "hr250006",
                YearLevel = YearLevel._9,
                TeacherID = "act250006",
                Block = Block.F,
                ClassNumber = ClassNumber._6
            },
            new Homeroom
            {
                HomeroomID = "hr250007",
                YearLevel = YearLevel._10,
                TeacherID = "act250007",
                Block = Block.A,
                ClassNumber = ClassNumber._7
            },
            new Homeroom
            {
                HomeroomID = "hr250008",
                YearLevel = YearLevel._11,
                TeacherID = "act250008",
                Block = Block.B,
                ClassNumber = ClassNumber._8
            },
            new Homeroom
            {
                HomeroomID = "hr250009",
                YearLevel = YearLevel._12,
                TeacherID = "act250009",
                Block = Block.C,
                ClassNumber = ClassNumber._9
            },
            new Homeroom
            {
                HomeroomID = "hr250010",
                YearLevel = YearLevel._13,
                TeacherID = "act250010",
                Block = Block.D,
                ClassNumber = ClassNumber._10
            }
        );
        modelBuilder.Entity<Diagnosis>().HasData(
            new Diagnosis
            {
                DiagnosisID = 1,
                AppointmentID = 1,
                Description = "Mild allergic reaction to pollen",
                DateDiagnosed = new DateTime(2024, 10, 12)
            },
            new Diagnosis
            {
                DiagnosisID = 2,
                AppointmentID = 2,
                Description = "Sprained ankle during soccer",
                DateDiagnosed = new DateTime(2024, 10, 13)
            },
            new Diagnosis
            {
                DiagnosisID = 3,
                AppointmentID = 3,
                Description = "Seasonal flu symptoms",
                DateDiagnosed = new DateTime(2024, 10, 14)
            },
            new Diagnosis
            {
                DiagnosisID = 4,
                AppointmentID = 4,
                Description = "Mild asthma attack",
                DateDiagnosed = new DateTime(2024, 10, 15)
            },
            new Diagnosis
            {
                DiagnosisID = 5,
                AppointmentID = 5,
                Description = "Concussion evaluation after PE",
                DateDiagnosed = new DateTime(2024, 10, 16)
            },
            new Diagnosis
            {
                DiagnosisID = 6,
                AppointmentID = 6,
                Description = "Stomach virus",
                DateDiagnosed = new DateTime(2024, 10, 17)
            },
            new Diagnosis
            {
                DiagnosisID = 7,
                AppointmentID = 7,
                Description = "Sinus infection",
                DateDiagnosed = new DateTime(2024, 10, 18)
            },
            new Diagnosis
            {
                DiagnosisID = 8,
                AppointmentID = 8,
                Description = "Minor burn on hand",
                DateDiagnosed = new DateTime(2024, 10, 19)
            },
            new Diagnosis
            {
                DiagnosisID = 9,
                AppointmentID = 9,
                Description = "Stress-related headaches",
                DateDiagnosed = new DateTime(2024, 10, 20)
            },
            new Diagnosis
            {
                DiagnosisID = 10,
                AppointmentID = 10,
                Description = "Rash caused by detergent allergy",
                DateDiagnosed = new DateTime(2024, 10, 21)
            }
        ); modelBuilder.Entity<Prescription>().HasData(
            new Prescription
            {
                PrescriptionID = 1,
                DiagnosisID = 1,
                Medication = "Cetirizine",
                Dosage = "10mg once daily",
                StartDate = new DateTime(2024, 10, 12),
                EndDate = new DateTime(2024, 10, 19)
            },
            new Prescription
            {
                PrescriptionID = 2,
                DiagnosisID = 2,
                Medication = "Ibuprofen",
                Dosage = "200mg every 6 hours",
                StartDate = new DateTime(2024, 10, 13),
                EndDate = new DateTime(2024, 10, 18)
            },
            new Prescription
            {
                PrescriptionID = 3,
                DiagnosisID = 3,
                Medication = "Paracetamol",
                Dosage = "500mg every 4-6 hours",
                StartDate = new DateTime(2024, 10, 14),
                EndDate = new DateTime(2024, 10, 20)
            },
            new Prescription
            {
                PrescriptionID = 4,
                DiagnosisID = 4,
                Medication = "Albuterol Inhaler",
                Dosage = "2 puffs as needed",
                StartDate = new DateTime(2024, 10, 15),
                EndDate = new DateTime(2024, 10, 30)
            },
            new Prescription
            {
                PrescriptionID = 5,
                DiagnosisID = 5,
                Medication = "Rest and hydration",
                Dosage = "As needed",
                StartDate = new DateTime(2024, 10, 16),
                EndDate = new DateTime(2024, 10, 23)
            },
            new Prescription
            {
                PrescriptionID = 6,
                DiagnosisID = 6,
                Medication = "Loperamide",
                Dosage = "2mg after each loose stool",
                StartDate = new DateTime(2024, 10, 17),
                EndDate = new DateTime(2024, 10, 21)
            },
            new Prescription
            {
                PrescriptionID = 7,
                DiagnosisID = 7,
                Medication = "Amoxicillin",
                Dosage = "500mg three times daily",
                StartDate = new DateTime(2024, 10, 18),
                EndDate = new DateTime(2024, 10, 25)
            },
            new Prescription
            {
                PrescriptionID = 8,
                DiagnosisID = 8,
                Medication = "Aloe Vera Gel",
                Dosage = "Apply twice daily",
                StartDate = new DateTime(2024, 10, 19),
                EndDate = new DateTime(2024, 10, 22)
            },
            new Prescription
            {
                PrescriptionID = 9,
                DiagnosisID = 9,
                Medication = "Ibuprofen",
                Dosage = "200mg twice daily",
                StartDate = new DateTime(2024, 10, 20),
                EndDate = new DateTime(2024, 10, 27)
            },
            new Prescription
            {
                PrescriptionID = 10,
                DiagnosisID = 10,
                Medication = "Hydrocortisone cream",
                Dosage = "Apply once daily",
                StartDate = new DateTime(2024, 10, 21),
                EndDate = new DateTime(2024, 10, 28)
            }
        );
        modelBuilder.Entity<Labtest>().HasData(
            new Labtest
            {
                LabtestID = 1,
                RecordID = 1,
                TestType = "Blood Test 1",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 11))
            },
            new Labtest
            {
                LabtestID = 2,
                RecordID = 2,
                TestType = "Blood Test 2",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 12))
            },
            new Labtest
            {
                LabtestID = 3,
                RecordID = 3,
                TestType = "Blood Test 3",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 13))
            },
            new Labtest
            {
                LabtestID = 4,
                RecordID = 4,
                TestType = "Blood Test 4",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 14))
            },
            new Labtest
            {
                LabtestID = 5,
                RecordID = 5,
                TestType = "Blood Test 5",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 15))
            },
            new Labtest
            {
                LabtestID = 6,
                RecordID = 6,
                TestType = "Blood Test 6",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 16))
            },
            new Labtest
            {
                LabtestID = 7,
                RecordID = 7,
                TestType = "Blood Test 7",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 17))
            },
            new Labtest
            {
                LabtestID = 8,
                RecordID = 8,
                TestType = "Blood Test 8",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 18))
            },
            new Labtest
            {
                LabtestID = 9,
                RecordID = 9,
                TestType = "Blood Test 9",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 19))
            },
            new Labtest
            {
                LabtestID = 10,
                RecordID = 10,
                TestType = "Blood Test 10",
                File = "/labtests/labtestsample.pdf",
                ResultDate = DateOnly.FromDateTime(new DateTime(2024, 10, 20))
            }
        );

        modelBuilder.Entity<Student>()
            .HasMany(s => s.Caregivers)
            .WithMany(c => c.Students)
            .UsingEntity<Dictionary<string, object>>(
                "StudentCaregivers",
                j => j.HasOne<Caregiver>()
                      .WithMany()
                      .HasForeignKey("CaregiverID")
                      .HasConstraintName("FK_StudentCaregivers_Caregiver")
                      .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Student>()
                      .WithMany()
                      .HasForeignKey("StudentID")
                      .HasConstraintName("FK_StudentCaregivers_Student")
                      .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("StudentCaregivers");
                    j.HasKey("StudentID", "CaregiverID");

                    // seed relationships (adjust/add as you like)
                    j.HasData(
                        new { StudentID = "ac250001", CaregiverID = "acc250001" },
                        new { StudentID = "ac250002", CaregiverID = "acc250001" },
                        new { StudentID = "ac250003", CaregiverID = "acc250001" },
                        new { StudentID = "ac250004", CaregiverID = "acc250001" },
                        new { StudentID = "ac250005", CaregiverID = "acc250004" },
                        new { StudentID = "ac250006", CaregiverID = "acc250002" },
                        new { StudentID = "ac250007", CaregiverID = "acc250001" },
                        new { StudentID = "ac250008", CaregiverID = "acc250001" },
                        new { StudentID = "ac250009", CaregiverID = "acc250005" },
                        new { StudentID = "ac250010", CaregiverID = "acc250001" }
                    );
                }
            );




        modelBuilder.Entity<Student>()
            .HasOne(s => s.Homeroom)
            .WithMany(h => h.Students)
            .HasForeignKey(s => s.HomeroomID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Student)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.StudentID);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorID);

        modelBuilder.Entity<Diagnosis>()
            .HasOne(d => d.Appointment)
            .WithOne(a => a.Diagnosis)
            .HasForeignKey<Diagnosis>(d => d.AppointmentID);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Diagnosis)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.DiagnosisID);


        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Student)
            .WithMany(s => s.MedicalRecords)
            .HasForeignKey(m => m.StudentID);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Doctor)
            .WithMany(d => d.MedicalRecords)
            .HasForeignKey(m => m.DoctorID);

        modelBuilder.Entity<Labtest>()
            .HasOne(l => l.MedicalRecord)
            .WithMany(r => r.Labtests)
            .HasForeignKey(l => l.RecordID);

        modelBuilder.Entity<Teacher>()
            .HasOne(t => t.Homeroom)
            .WithOne(h => h.Teacher)
            .HasForeignKey<Homeroom>(h => h.TeacherID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);




        base.OnModelCreating(modelBuilder);



    }
}

