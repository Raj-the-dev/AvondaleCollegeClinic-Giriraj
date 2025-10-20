
SELECT p.DoctorID,
       d.FirstName,
       d.LastName,
       AVG(DATEDIFF(day, p.StartDate, p.EndDate)) AS AvgPrescriptionDays
FROM Prescriptions p
JOIN Doctors d ON p.DoctorID = d.DoctorID
WHERE p.EndDate IS NOT NULL
GROUP BY p.DoctorID, d.FirstName, d.LastName
ORDER BY AvgPrescriptionDays DESC;
