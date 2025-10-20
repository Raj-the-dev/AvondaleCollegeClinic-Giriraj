SELECT c.CaregiverID,
       c.FirstName,
       c.LastName,
       COUNT(sc.StudentID) AS LinkedStudents
FROM Caregivers c
LEFT JOIN StudentCaregivers sc ON c.CaregiverID = sc.CaregiverID
GROUP BY c.CaregiverID, c.FirstName, c.LastName
ORDER BY LinkedStudents DESC;
