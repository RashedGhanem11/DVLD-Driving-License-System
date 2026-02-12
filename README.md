# DVLD - Driving & Vehicle License Department

A desktop application for managing driving licenses, tests, and renewals. I built this project to master the fundamentals of 3-Tier Architecture and raw SQL handling before moving to modern frameworks.

## Tech Stack
* **Language:** C# (.NET Framework)
* **UI:** Windows Forms
* **Database:** SQL Server
* **Data Access:** ADO.NET (No ORMs used)

## Project Highlights
* **3-Tier Architecture:** Separated the code into Presentation, Business Logic, and Data Access layers manually.
* **SQL:** Relies heavily on SQL Views and specific queries rather than loading logic into memory.
* **Features:**
    * Issue driving licenses (Local & International).
    * Full history of driving licenses (Local & International).
    * Replacement for lost/damaged licenses.
    * Detain/Release license workflow with fine calculations.
