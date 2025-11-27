# Datenbank-Design-Dokumentation

## Überblick

Dieses Dokument erläutert die Datenbankstruktur für den Driving School Lesson Scheduler. Es beschreibt die Tabellen, Beziehungen, Datentypen sowie die zugrunde liegenden Designentscheidungen. Außerdem enthält es einen Verweis auf das SQL-Skript, mit dem die Datenbank erzeugt wird.

---

## 1. Zusammenfassung des SQL-Skripts
Die Datenbank wurde für Microsoft SQL Server erstellt und umfasst folgende Schritte:
- Erstellen der Datenbank, falls sie noch nicht existiert.
- Löschen bestehender Tabellen für eine saubere Neuinitialisierung.
- Anlegen von vier Haupttabellen: Student, Instructor, Lesson und Booking.
- Definieren von Primärschlüsseln in jeder Tabelle.
- Festlegen von Fremdschlüsselbeziehungen.
- Einfügen von Beispieldaten in alle Tabellen.
- Bereitstellen einer Beispiel-JOIN-Abfrage zur Demonstration der Nutzung.

Das SQL-Skript implementiert das konzeptionelle ER-Modell basierend auf den funktionalen Anforderungen: Unterrichtsplanung, Instruktorenzuordnung und Schülerbuchungen.

---

## 2. Tabellen und Spaltenauswahl
### 2.1 Student
**Zweck:** Speicherung von persönlichen Daten und Fortschritt der Fahrschüler.

- `StudentID` – Primärschlüssel, Identity-Integer für schnelle Indizierung.
- `FirstName`, `LastName` – Grundlegende Identifikationsfelder.
- `Phone` – Optionale Kontaktinformation.
- `TotalHours` – Integer zur Erfassung der absolvierten Fahrstunden.

Diese Felder bilden die minimal erforderliche Datenbasis für Buchungen und Fortschrittsverfolgung.

## 2.2 Instructor
**Zweck:** Informationen zu Fahrlehrern und deren Verfügbarkeit.
- `InstructorID` – Primärschlüssel.
- `FirstName`, `LastName` – Identifikationsdaten.
- `WorkHours` – Freitextdarstellung der Verfügbarkeit.

Die Speicherung der Verfügbarkeit als einfacher Text bietet für den aktuellen Projektumfang maximale Flexibilität.

## 2.3 Lesson
**Zweck:** Darstellung geplanter Fahreinheiten.

- `LessonID` – Primärschlüssel.
- `InstructorID` – Fremdschlüssel zum Instructor.
- `LessonDate` – Datum und Uhrzeit der Unterrichtseinheit.
- `DurationMinutes` – Dauer des Unterrichts in Minuten.
- `Status` – Status wie geplant, abgeschlossen oder storniert.

Diese Tabelle ermöglicht Planung und Verfolgung aller Fahrstunden.

## 2.4 Booking
**Zweck:** Verknüpft Schüler mit ihren gebuchten Fahrstunden.

- `BookingID` Primärschlüssel.
- `StudentID` – Fremdschlüssel.
- `LessonID` – Fremdschlüssel.

Die separate Buchungstabelle realisiert eine flexible Zuordnung vieler Schüler zu vielen Fahrstunden.

---

## 3. Beziehungen
### **3.1 Instructor → Lesson (1:n)**
Ein Fahrlehrer führt viele Unterrichtseinheiten durch, aber jede Einheit hat genau einen Fahrlehrer. Daher ist eine 1:n-Beziehung passend.

### **3.2 Student → Booking → Lesson (n:m über Zwischentabelle)**
Ein Schüler kann viele Fahrstunden besuchen, und eine Fahrstunde kann theoretisch mehreren Schülern zugewiesen werden.

Die Tabelle `Booking` dient als **Verknüpfungstabelle**, die diese n:m-Beziehung umsetzt.

---


## 4. Datentypen und Begründung
### **INT IDENTITY**
Für Primärschlüssel verwendet, da effizient, schnell und automatisch fortlaufend generiert.

### **NVARCHAR**
Für Namen, Telefonnummern und Statusfelder, um Unicode-Zeichen zu unterstützen (wichtig für internationale Datensätze).

### **DATETIME**
Für das Datum und die Uhrzeit einer Fahrstunde. Dieser Typ ist präzise und vollständig in SQL Server unterstützt.

### **INT (DurationMinutes, TotalHours)**
Ein Integer ist ausreichend für Zeitangaben und Zählwerte.

## 5. Designbegründung
### **Einfachheit und Klarheit**
Das Schema ist minimal gehalten, unterstützt aber vollständig:
- Verwaltung von Fahrschülern,
- Zuweisung von Fahrlehrern,
- Planung von Unterrichtseinheiten,
- Buchungen und Fortschrittsverfolgung.

### **Skalierbarkeit**
Das Modell ist leicht erweiterbar, z. B. um:
- Zahlungssysteme,
- detaillierte Verfügbarkeitspläne der Fahrlehrer,
- Fahrzeugverwaltung oder Prüfungstermine.

### **Normalisierung**
Die Struktur entspricht der 3. Normalform:
- Keine redundanten Daten,
- Alle Attribute hängen funktional vom Primärschlüssel ab,
- n:m-Beziehungen sind über eine Zwischentabelle korrekt modelliert.

---

### **6. Fazit**
Dieses Datenbankdesign ist auf Übersichtlichkeit, Wartbarkeit und Anforderungen des Systems abgestimmt. Es ermöglicht eine strukturierte Verwaltung von Fahrstunden, Schülern, Lehrern und Buchungen.

Erweiterungen können problemlos ergänzt werden, ohne das Grundmodell zu verändern.

---

**Ende des Dokuments**