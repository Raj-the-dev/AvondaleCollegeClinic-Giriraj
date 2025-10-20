SELECT s.StudentID,
       s.FirstName,
       s.LastName,
       a.AppointmentDateTime AS LatestVisit
FROM Students s
CROSS APPLY (
    SELECT TOP 1 AppointmentDateTime
    FROM Appointments
    WHERE StudentID = s.StudentID
    ORDER BY AppointmentDateTime DESC
) a
ORDER BY LatestVisit DESC;
