## Opis

AutoRent to demo aplikacja wypożyczalni samochodów.  
Backend: ASP.NET Core 8 + EF Core. Frontend: HTML/Bootstrap5 + vanill­a JS.  
Działa w Dockerze i na Render.com (free tier).

---

## 🚀 Live Demo

[https://autorent-net.onrender.com](https://autorentnet.onrender.com)  

---

## ✨ Funkcje

- **Flota** – GET `/api/vehicles` → lista aut  
- **Wyszukiwarka** – filtr po dacie, godzinie, lokalizacji  
- **Rezerwacja** – POST `/api/reservations` (walidacja i kolizje)  
- **Swagger UI** – lokalnie pod `/swagger`  

---

## 🛠 Technologie

| Komponent             | Technologia                       |
|-----------------------|-----------------------------------|
| Backend               | ASP.NET Core 8 Minimal API        |
| ORM                   | EF Core 8 (SQLite / PostgreSQL)   |
| Frontend              | HTML, Bootstrap 5, Vanilla JS     |
| Konteneryzacja        | Docker multi-stage                |
| Hosting               | Render.com (free tier)            |

---

## ⚙️ Instalacja i uruchomienie

### 🌱 Lokalnie

```bash
git clone https://github.com/Patruello/AutoRentNet.git
cd AutoRentNet
dotnet run
# Otwórz https://localhost:5162
```
### 🐳 Docker 
Upewnij się, że masz zainstalowanego Dockera.

```bash
docker-compose up --build
# Otwórz: http://localhost:8080
