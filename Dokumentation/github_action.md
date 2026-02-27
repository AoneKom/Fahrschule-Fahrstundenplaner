# Dokumentation: GitHub Actions (CI)

Um die Softwarequalität sicherzustellen, wurde eine GitHub Action für Continuous Integration (CI) implementiert.

### Erforderliche Schritte:
1. **Workflow-Datei erstellen:** Im Verzeichnis `.github/workflows/` wurde eine YAML-Datei angelegt.
2. **Trigger definieren:** Die Action wird bei jedem `push` in die Branches `main` und `working-version` sowie bei Pull Requests ausgelöst.
3. **Build-Umgebung:** Als Runner wird `windows-latest` verwendet, um die WPF-Kompatibilität zu gewährleisten.
4. **Schritte der Action:**
   - Auschecken des Quellcodes via `actions/checkout`.
   - Setup der .NET-Umgebung.
   - Wiederherstellung der NuGet-Pakete (`dotnet restore`).
   - Kompilierung des Projekts (`dotnet build`).
   - Automatische Ausführung der 5 Unit-Tests (`dotnet test`).
