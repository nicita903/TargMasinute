# AutoVault – Gestiune Tranzacții Auto

**Aplicație WPF în C#** pentru gestionarea tranzacțiilor realizate într‑un târg de mașini.  
Permite înregistrarea, vizualizarea, căutarea, modificarea și ștergerea tranzacțiilor, cu stocare în fișier text.

---

## 📋 Funcționalități principale

- **Lista tranzacțiilor** – afișare sub formă de carduri cu toate detaliile (ID, mașină, vânzător, cumpărător, an, culoare, combustibil, preț, dată) + buton de ștergere.
- **Adăugare tranzacție** – formular validat cu secțiuni:
  - Persoane (vânzător, cumpărător)
  - Date mașină (marcă, model, an fabricație, culoare – dropdown cu 20 de culori, tip combustibil, dotări opționale)
  - Detalii tranzacție (dată, preț)
- **Căutare tranzacție** – după numele clientului (vânzător/cumpărător) și/sau după marca mașinii; rezultate afișate în aceleași carduri.
- **Modificare tranzacție** – căutare prealabilă după **model mașină** sau **nume cumpărător**, selectare din listă, editare câmpuri și salvare.
- **Stocare persistentă** – datele sunt salvate într‑un fișier text (`Tranzactii.txt`) pe Desktop.

---

## 🛠️ Tehnologii utilizate

- **.NET** (versiunea compatibilă cu WPF – .NET Core 3.1 sau .NET 5/6/8)
- **WPF (Windows Presentation Foundation)** – interfață grafică
- **C#** – logică de business, validări, evenimente
- **INotifyPropertyChanged** – binding bidirecțional
- **Fișiere text** – stocare date (strat de persistență)

---

## 🚀 Cum rulezi aplicația

### Cerințe
- **Windows** (WPF nu rulează pe Linux/macOS fără compatibilitate specială)
- **.NET Desktop Runtime** (versiunea cu care a fost compilat proiectul)

### Pași
1. Clonează sau copiază sursele într‑un folder.
2. Deschide soluția (`TargMasiniWPF.sln`) în **Visual Studio** (2022 sau mai nou).
3. Asigură‑te că toate referințele (la `LibrarieModele` și `NivelStocareDate`) sunt rezolvate.
4. Compilează soluția (`Build` → `Build Solution` sau `Ctrl+Shift+B`).
5. Rulează aplicația (`F5` sau `Debug` → `Start Debugging`).

> **Observație:** Fișierul de date `Tranzactii.txt` va fi creat automat pe Desktop. Dacă dorești să pornești cu date de test, poți adăuga manual câteva tranzacții prin interfață.

---

## 🧱 Structura proiectului
TargMasiniWPF/
├── MainWindow.xaml # Interfața principală (WPF)
├── MainWindow.xaml.cs # Codul din spate (evenimente, validări)
├── LibrarieModele/ # (referință) Clasele de model
│ ├── Persoana.cs
│ ├── Masina.cs
│ ├── Tranzactie.cs
│ ├── CuloareMasina.cs (enum)
│ └── OptiuniMasina.cs (enum)
├── NivelStocareDate/ # (referință) Stratul de persistență
│ ├── IStocareData.cs
│ └── AdministrareTranzactiiFisierText.cs
└── README.md


---

## 📖 Scurtă descriere a claselor principale

- **`Persoana`** – reține datele vânzătorului/cumpărătorului.
- **`Masina`** – marcă, model, an, culoare, combustibil, dotări (enum cu flag).
- **`Tranzactie`** – agregă o tranzacție: ID, vânzător, cumpărător, mașină, preț, dată.
- **`IStocareData`** – interfață pentru operații CRUD (`Add`, `Get`, `Update`, `Delete`).
- **`AdministrareTranzactiiFisierText`** – implementare concretă ce lucrează cu fișierul text.
- **`MainWindow`** – fereastra principală, gestionează evenimentele UI, validarea și legătura cu stratul de stocare.

---

## ✨ Posibile îmbunătățiri (de viitor)

- Autentificare utilizator (admin / utilizator)
- Rapoarte statistice și grafice (vânzări pe lună, top mărci)
- Export în Excel / PDF
- Migrare la bază de date (SQLite, SQL Server)
- Teme personalizabile (dark/light)

---

## 👥 Autori

- **Numele tău** – student, grupa ____

---

## 📄 Licență

Acest proiect este realizat în scop educațional, în cadrul disciplinei **PIU**.