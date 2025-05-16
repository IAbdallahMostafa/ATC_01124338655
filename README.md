# 🎟️ Booking System 

A full-featured system for managing bookings, categories, and events, with integrated JWT authentication, role-based authorization, and Entity Framework Core.

## 🔧 Technologies Used

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- Identity (User & Role management)
- JWT Authentication & Refresh Tokens
- Swagger for API documentation

---

## 📁 Project Structure

- Booking.Infrastructure
    - /Data - Database context
    - /Service - Implementation of all repositries
    - /Helpers - JWT Settings, and static class for roles
    - /Migrations - Database Migrations
    - /DatabaseIntializer - Interface and class to intiailze database (admin account and main roles)
- Booking.Core
    - /Entities - BaseEnitity, Category, Event, Book, and Authentication Entities
    - /DTOs - DTOs for all entites
    - Interface - Generic inteface and all repositries interfaces
- Booking.API
  - /Controllers — API endpoints for Account, Booking, Category, and Event
  - /Helpers - MappingProfile to manage automapper, and Utiltilies to manage images constraints
    
---

## 📌 Features

- ✅ User registration and login
- ✅ JWT authentication with refresh token support
- ✅ Role-based authorization (`Admin`, `Editor`, `User`)
- ✅ CRUD operations:
  - Events
  - Categories
  - Bookings
- ✅ Auto database migration on first run
- ✅ Swagger UI for testing

---

## 🚀 Setup Instructions

### 1. Clone the repository

```bash
git clone https://github.com/IAbdallahMostafa/ATC_01124338655.git
cd booking-system-api

### 2. Navigate to the API project
cd Booking.API

### 3.Restore dependencies
dotnet restore

### 4.Run the API
dotnet run

### 5.Test via Swagger UI
https://localhost:<port>/swagger

### 6.Seeded Admin Credentials
Username: Admin
Password: P@$$w0rd
