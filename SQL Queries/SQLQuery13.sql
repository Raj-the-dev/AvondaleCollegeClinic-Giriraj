SELECT a1.AppointmentID AS FirstBooking,
       a2.AppointmentID AS SecondBooking,
       a1.DoctorID,
       a1.AppointmentDateTime
FROM Appointments a1
JOIN Appointments a2
  ON a1.DoctorID = a2.DoctorID
 AND a1.AppointmentID <> a2.AppointmentID
 AND a1.AppointmentDateTime = a2.AppointmentDateTime;
