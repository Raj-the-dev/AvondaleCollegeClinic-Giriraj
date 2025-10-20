SELECT s.StudentID,
       s.FirstName,
       s.LastName
FROM Students s
WHERE NOT EXISTS (
    SELECT 1
    FROM Appointments a
    WHERE a.StudentID = s.StudentID
      AND a.AppointmentDateTime >= DATEADD(day, -60, GETDATE())
);
