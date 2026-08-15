# E-Store MVC Template

A full-stack e-commerce application built with ASP.NET Core MVC using Clean Architecture principles.

## Features

* User registration and authentication with ASP.NET Core Identity
* Product and category management
* Product images and featured header images
* Shopping cart and cart item management
* Favorites
* Discount support
* Order creation and order item tracking
* Payment workflow
* Simulated payment process for safe testing without storing real card data
* User order history
* Admin-ready product and order management structure

## Architecture

```text
E-StoreMVCTemplate.Domain
├── Entities
└── Enums

E-StoreMVCTemplate.Application
├── DTOs
└── Interfaces

E-StoreMVCTemplate.Infrastructure
├── Data
├── Services
└── Migrations

E-StoreMVCTemplate.Web
├── Controllers
├── Views
├── ViewModels
└── wwwroot
```

```text
Controller
→ Service Interface
→ Service
→ AppDbContext
→ SQL Server
```

## Technologies

* ASP.NET Core MVC
* C#
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* Razor Views
* HTML, CSS, JavaScript
* xUnit
* Clean Architecture
* Dependency Injection

## Payment Flow

The project includes a payment structure connected to orders.

```text
Cart
→ Order
→ Order Items
→ Payment
→ Payment Status
```

The payment flow is simulated for development purposes. Real card details are not stored in the database.

## Getting Started

```bash
git clone https://github.com/Muhamet-Ali/E-StoreMVCTemplate.git
cd E-StoreMVCTemplate
dotnet restore
dotnet build
dotnet run --project E-StoreMVCTemplate.Web
```

## Author

Muhammet Ali Abdul Jalil

ASP.NET Core Developer
