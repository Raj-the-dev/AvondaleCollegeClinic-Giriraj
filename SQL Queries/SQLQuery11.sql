SELECT TOP 3 
       a.DoctorID,
       d.FirstName,
       d.LastName,
       COUNT(*) AS TotalAppointments
FROM Appointments a
JOIN Doctors d ON d.DoctorID = a.DoctorID
WHERE a.AppointmentDateTime >= DATEADD(day, -90, GETDATE())
GROUP BY a.DoctorID, d.FirstName, d.LastName
ORDER BY TotalAppointments DESC;
