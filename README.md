# DigitroShop 🛒
**An e-commerce application built with .NET 8, Clean Architecture, and CQRS-inspired patterns.**

DigitroShop is an online store and administration system created to practice common e-commerce workflows and backend architecture concepts, including layered design, role-based access control, and validation.

---

## 🚀 Key Architectural Highlights
This project applies several software design patterns and architectural concepts:

*   **Clean Architecture:** The solution is divided into five layers: Domain, Application, Infrastructure, Persistence, and Presentation. This separation helps organize business logic, data access, and presentation concerns.
*   **CQRS-Inspired Separation:** Read and write responsibilities are separated in the service layer where applicable, helping keep application workflows clearer and easier to maintain.
*   **Facade Pattern:** Used in selected application workflows to provide a simpler interface between controllers and underlying services.
*   **Custom Authentication & Authorization:** The project uses a custom claims-based cookie authentication system with User, Role, and UserInRole management instead of ASP.NET Core Identity.
*   **Layered Validation:** Input validation is handled with FluentValidation on the server side and JavaScript/jQuery on the client side.

---

## 🛠 Tech Stack
*   **Framework:** .NET 8 (ASP.NET Core MVC & Razor Pages)
*   **Database:** Microsoft SQL Server
*   **ORM:** Entity Framework Core (Code First)
*   **Validation:** FluentValidation
*   **UI/Frontend:** Bootstrap, jQuery, JavaScript
*   **Tools:** LazZiya TagHelpers for pagination

---

## 📂 Project Structure
The project is organized into the following layers:
- **Domain:** Core business entities and enums.
- **Application:** Service interfaces and implementations, selected Facade-based workflows, and CQRS-inspired read/write workflows. Data access is performed through a DbContext abstraction.
- **Infrastructure:** Intended for external integrations and cross-cutting concerns such as caching, logging, messaging, file storage, and third-party services. This layer is currently minimal in the project.
- **Persistence:** EF Core DbContext implementation, migrations, and database configuration.
- **Presentation:** MVC controllers, Razor Pages, ViewModels, and client-side assets responsible for user interaction.
---

## 🔑 Role-Based Access Control (RBAC)
The application uses three roles with different permissions:

1.  **Admin:** Full authority over the system. Can manage Users, Categories, Products, Sliders, and Site Settings.
2.  **Operator:** Restricted access. Can manage Products and Orders, but is **blocked** from Users and Categories.
3.  **Customer:** Access to the storefront, cart, and personal order history.

---

## 🌟 Features

### 🛒 Storefront (Client)
- **User Journey:** Custom Registration and Login system.
- **Product Discovery:** Product searching, filtering, and category-based browsing.
- **Shopping Cart:** Add, remove, and update cart items.
- **Order Management:** View order history, payment status, and tracking details.
- **Pagination:** Product-list pagination using LazZiya TagHelpers.

### ⚙️ Admin Dashboard
- **User Management:** Create (with specific roles), Edit, Delete, or Deactivate users.
- **Category Management:** Full CRUD operations for product hierarchy.
- **Product Management:** Create, edit, delete, and manage product details and inventory.
- **Order Tracking:** Monitor all orders and update statuses (*Processing, Delivered, Canceled*).
- **Payment Logs:** View payment transaction records.
- **Content Management:** Manage homepage sliders and site banners.

---
## 📸 Screenshots

To give you a visual tour of **DigitroShop**, here are the previews of the User Interface and Admin Dashboard:

### 🛒 Storefront (Customer Perspective)

| Home Page 1 | Home Page 2 |
|:---:|:---:|
| ![Home Page](Screenshots/home-page1.png) | ![Home Page 2](Screenshots/home-page2.png) |

| Product Details | Shopping Cart |
|:---:|:---:|
| ![Product Details](Screenshots/product-detail-page.png) | ![Cart](Screenshots/cart-page.png) |

| Sign In | Sign Up |
|:---:|:---:|
| ![Sign In](Screenshots/login-page.png) | ![Sign Up](Screenshots/signup-page.png) |

| Product List |
|:---:|
| ![Product Management](Screenshots/products-page.png) |

### ⚙️ Admin Dashboard (Management)

| Admin Overview | Order List |
|:---:|:---:|
| ![Admin Panel](Screenshots/admin-page.png) | ![Order List](Screenshots/order-page.png) |

| Add Product | Edit Product |
|:---:|:---:|
| ![Add Product](Screenshots/add-product-page.png) | ![Edit Product](Screenshots/edit-product-page.png) |

---
## 📋 Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB or Developer Edition)
- Visual Studio 2022 / JetBrains Rider

---

## 🔧 How to Run
1.  Clone the repository: `git clone https://github.com/severanceline/DigitroShop.git`
2.  Navigate to the `Persistence` layer and update the ConnectionString in `appsettings.json`.
3.  Run `Update-Database` in Package Manager Console.
4.  Build and Run the project.

---

## 🔐 Development Test Accounts
After applying migrations, you can use the following credentials to test the roles:
- **Admin:** `digitroadmin@gmail.com` | Password: `LoLo1234` 
- **Operator:** `digitrooperator@gmail.com` | Password: `Moop00W3`

---
