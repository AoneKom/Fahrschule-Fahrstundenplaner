# Testfälle für Fahrstundenplaner

## Testfall 1: Berechnung der Reststunden
* **Ziel:** Überprüfung, ob die App die verbleibenden Fahrstunden korrekt berechnet.
* **Eingabe:** Gesamtstunden = 12, Absolvierte Stunden = 5.
* **Erwartetes Ergebnis:** Anzeige "Reststunden: 7".

## Testfall 2: Validierung leerer Eingaben
* **Ziel:** Sicherstellen, dass die App bei leerem Schülernamen eine Warnung ausgibt.
* **Eingabe:** Klick auf "Speichern" ohne Namenseingabe.
* **Erwartetes Ergebnis:** Fehlermeldung oder keine Speicherung.

* # Beschreibung der Testfälle

**ID: T01**
**Beschreibung:** Suche nach einem Schülernamen.
**Vorbedingung:** Das Programm ist gestartet und die Liste enthält Daten.
**Test-Schritte:**
1. Im Feld "Suche" wird der Name eines existierenden Schülers eingegeben.
2. Die Liste wird automatisch oder durch Klick gefiltert.
**Erwartetes Resultat:** In der Tabelle werden nur die Einträge angezeigt, die den eingegebenen Namen enthalten. Dies wird durch Sichtprüfung der Tabellenspalte "Schüler" überprüft.

---

**ID: T02**
**Beschreibung:** Filtern nach unbezahlten Fahrstunden.
**Vorbedingung:** Die Liste enthält sowohl bezahlte als auch unbezahlte Stunden.
**Test-Schritte:**
1. Die Checkbox "Nur Unbezahlt" wird aktiviert.
2. Die Ansicht wird aktualisiert.
**Erwartetes Resultat:** In der Tabelle sind nur Einträge sichtbar, bei denen der Status auf "Unbezahlt" steht. Dies wird in der Spalte "Bezahlt" überprüft.
