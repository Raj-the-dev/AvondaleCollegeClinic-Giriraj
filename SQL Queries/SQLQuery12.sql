SELECT DoctorID,
       FirstName,
       LastName,
       UpcomingCount,
       DENSE_RANK() OVER (ORDER BY UpcomingCount DESC) AS LoadRank
FROM (
    SELECT d.DoctorID,
           d.FirstName,
           d.LastName,
           COUNT(a.AppointmentID) AS UpcomingCount
    FROM Doctors d
    LEFT JOIN Appointments a
      ON a.DoctorID = d.DoctorID
     AND a.AppointmentDateTime >= GETDATE()
    GROUP BY d.DoctorID, d.FirstName, d.LastName
) x
ORDER BY LoadRank, LastName;
