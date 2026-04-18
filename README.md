# 🛒 IntelliShop - AI-Powered Ecommerce Platform

![.NET](https://img.shields.io/badge/.NET-8.0/10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![PayPal](https://img.shields.io/badge/PayPal-00457C?style=for-the-badge&logo=paypal&logoColor=white)
![AI](https://img.shields.io/badge/AI-Cerebras-FF9900?style=for-the-badge&logo=openai&logoColor=white)

**IntelliShop** is a full-stack, aesthetically pleasing, and feature-rich Ecommerce web application built using **ASP.NET Core MVC**. It focuses on modern design principles, an end-to-end purchasing workflow, seamless secure checkouts, and boasts an integrated **AI Shopping Assistant** that contextually helps users find and recommend products based on the platform's active catalog.

*(**Note:** Replace this text with a screenshot/GIF of your home page and checkout flow! Recruiters love visual proof of what you've built straight at the top of the README!)*

---

## ✨ Features

- **🔐 Secure Authentication & Authorization**
  - Robust user registration and login workflows using ASP.NET Core Identity.
  - Granular access controls (e.g., Admin vs. Standard User routing).
  
- **🛍️ Complete Shopping Cart Workflow**
  - Users can browse products, add products to their cart, increment variations, and track active statuses dynamically.
  - A comprehensive breakdown of order subtotals and dynamic tax calculations are clearly visualized.

- **💳 Real-Time Payment Processing via PayPal SDK**
  - Integrated with the **PayPal Checkout JS SDK** for secure, SSL-encrypted 3rd-party transactions.
  - Graceful backend verification ensuring transaction captures line up safely within the Database before formally creating an analytical record.

- **🤖 AI-Powered Shopping Assistant**
  - Embedded AI chat widget querying the **Cerebras Inference API**.
  - Intelligently accesses current store databases allowing the AI to recommend actual items, answer product inquiries, and guide buyers efficiently.

- **⚙️ Admin Dashboard & Inventory Management**
  - Exclusive access for platform owners and administrators.
  - Tools to actively add new Products and categorize them effectively seamlessly mapping them across the database.
  - Auto-checks dynamically for avoiding Category label duplications across schemas.

- **👤 Dedicated User Profiles**
  - Personalized profile overview page letting users track chronological history of placed orders and their direct granular payment status.

---

## 🛠️ Technology Stack

* **Backend Environment:** ASP.NET Core MVC (C#)
* **Database & ORM:** PostgreSQL alongside Microsoft Entity Framework Core (EF Core) 
* **Authentication:** ASP.NET Core Identity
* **Payments:** PayPal Standard Integration JS SDK
* **Frontend Design:** Vanilla HTML/CSS, Bootstrap 5, Modern Glassmorphism layout techniques
* **AI Engine:** Cerebras AI API

---

## 🚀 Getting Started

### Prerequisites
1. **.NET SDK** (Version 8.0 or 10.0+ recommended)
2. **PostgreSQL** installed locally (Ensure credentials match the `appsettings.json` output)
3. A **PayPal Developer Sandbox** account (to generate a `ClientId`)
4. **Cerebras API Key** (for accessing the AI recommendations logic)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/YourUsername/IntelliShop-DotNet.git
   cd IntelliShop-DotNet
   ```

2. **Configure AppSettings:**
   Open `Ecommerce/appsettings.json` and adjust the connection strings strictly for your operating environment:
   ```json
   "ConnectionStrings": {
     "ConnectionString": "Host=localhost;Port=5432;Database=ecommerce;Username=postgres;Password=YOUR_DB_PASSWORD"
   },
   "Cerebras": {
     "ApiKey": "YOUR_API_KEY_HERE"
   },
   "PayPal": {
     "ClientId": "YOUR_PAYPAL_SANDBOX_CLIENT_ID"
   }
   ```

3. **Database Migrations:**
   Run Entity Framework tools to formally migrate and build the SQL tables (Orders, Categories, Users, etc.).
   ```bash
   dotnet ef database update
   ```

4. **Run the Project!**
   Execute directly on your localhost to see changes live.
   ```bash
   dotnet run
   ```

---

## 💡 What Makes This Project Unique (For Recruiters 👀)

Building an e-commerce platform is often used to demonstrate full-stack comprehension; however, this architecture goes **beyond standard CRUD logic** by incorporating live ecosystem mechanics:
- Handling third-party SDK dependencies & APIs (PayPal & Cerebras).
- Architecting strict database relationships bridging logical data workflows (`Product` -> `Cart` -> `Checkout` -> `Order` + `Payment`).
- Structuring aesthetic, modern responsive layout paradigms devoid of generic templating utilizing customized UI CSS hooks.

---
*Created by [Your Name Here]*
