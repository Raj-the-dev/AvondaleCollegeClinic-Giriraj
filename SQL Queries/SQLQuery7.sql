SELECT s.StudentID,
       s.FirstName,
       s.LastName,
       h.HomeroomID,
       t.FirstName AS TeacherFirst,
       t.LastName  AS TeacherLast
FROM Students s
JOIN Homerooms h ON s.HomeroomID = h.HomeroomID
JOIN Teachers  t ON h.TeacherID  = t.TeacherID
ORDER BY t.LastName, s.LastName;
