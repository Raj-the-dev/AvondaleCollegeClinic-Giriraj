SELECT d.DoctorID,
       d.FirstName,
       d.LastName,
       COUNT(a.AppointmentID) AS UpcomingCount
FROM Doctors d
LEFT JOIN Appointments a
  ON a.DoctorID = d.DoctorID
 AND a.AppointmentDateTime >= GETDATE()
GROUP BY d.DoctorID, d.FirstName, d.LastName
ORDER BY UpcomingCount DESC;
