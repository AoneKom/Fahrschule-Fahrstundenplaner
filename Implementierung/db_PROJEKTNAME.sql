-- Tabelle der Fahrlehrer--------------------------------------------
-- 1. Erstellen der Datenbank, falls nicht vorhanden
--------------------------------------------
IF DB_ID('DrivingSchoolDB') IS NULL
    CREATE DATABASE DrivingSchoolDB;
GO

USE DrivingSchoolDB;
GO

--------------------------------------------
-- 2. Löschen vorhandener Tabellen
--------------------------------------------
IF OBJECT_ID('dbo.Booking', 'U') IS NOT NULL DROP TABLE dbo.Booking;
IF OBJECT_ID('dbo.Lesson', 'U') IS NOT NULL DROP TABLE dbo.Lesson;
IF OBJECT_ID('dbo.Student', 'U') IS NOT NULL DROP TABLE dbo.Student;
IF OBJECT_ID('dbo.Instructor', 'U') IS NOT NULL DROP TABLE dbo.Instructor;
GO

--------------------------------------------
-- 3. Erstellen der Tabellen
--------------------------------------------

-- Tabelle der Schüler
CREATE TABLE Student (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) 
);
GO


CREATE TABLE Instructor (
    InstructorID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    WorkHours NVARCHAR(100) NULL
);
GO

-- Tabelle der Fahrstunden
CREATE TABLE Lesson (
    LessonID INT IDENTITY(1,1) PRIMARY KEY,
    InstructorID INT NOT NULL,
    LessonDate DATETIME NOT NULL,
    DurationMinutes INT NOT NULL,
    Status NVARCHAR(20) NOT NULL,

    CONSTRAINT FK_Lesson_Instructor
        FOREIGN KEY (InstructorID) REFERENCES Instructor(InstructorID)
);
GO

-- Tabelle der Buchungen (Student ↔ Lesson)
CREATE TABLE Booking (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    LessonID INT NOT NULL,

    CONSTRAINT FK_Booking_Student
        FOREIGN KEY (StudentID) REFERENCES Student(StudentID),

    CONSTRAINT FK_Booking_Lesson
        FOREIGN KEY (LessonID) REFERENCES Lesson(LessonID)
);
GO

--------------------------------------------
-- 4. Einfügen von Beispieldaten
--------------------------------------------

-- Schüler
INSERT INTO Student (FirstName, LastName, Phone, TotalHours)
VALUES
('Lukas', 'Müller', '123456', 5),
('Anna', 'Schneider', '555120', 2),
('Jonas', 'Weber', '8800555', 7),
('Sophie', 'Wagner', '101010', 10),
('Daniel', 'Becker', '999999', 1);

-- Fahrlehrer
INSERT INTO Instructor (FirstName, LastName, WorkHours)
VALUES
('Markus', 'Hoffmann', 'Mo–Fr 9-17'),
('Julia', 'Schulz', 'Di–Sa 10-18'),
('Florian', 'Braun', 'Mo–Do 8-16'),
('Clara', 'Fischer', 'Mi–So 12-20'),
('Sebastian', 'Schäfer', 'Mo–Fr 11-19');

-- Fahrstunden
INSERT INTO Lesson (InstructorID, LessonDate, DurationMinutes, Status)
VALUES
(1, '2025-03-10 10:00', 60, 'geplant'),
(1, '2025-03-10 12:00', 90, 'geplant'),
(2, '2025-03-11 14:00', 60, 'abgeschlossen'),
(3, '2025-03-12 09:00', 120, 'storniert'),
(4, '2025-03-12 16:00', 60, 'geplant');

-- Buchungen
INSERT INTO Booking (StudentID, LessonID)
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5);

--------------------------------------------
-- 5. Beispielabfrage mit JOIN
--------------------------------------------
SELECT
    s.FirstName + ' ' + s.LastName AS StudentName,
    i.FirstName + ' ' + i.LastName AS InstructorName,
    l.LessonDate,
    l.Status,
    l.DurationMinutes
FROM Booking b
JOIN Student s ON b.StudentID = s.StudentID
JOIN Lesson l ON b.LessonID = l.LessonID
JOIN Instructor i ON l.InstructorID = i.InstructorID
ORDER BY l.LessonDate;
GO

