# 🤝 ServiSwap: a service Exchange Platform

ServiSwap is a web platform designed to allow users to exchange **non-commercial services** using a **time-based credit system** instead of money.

The purpose of the platform is to facilitate mutual assistance between users: a user can offer their skills to others, while using the time-credit system to request services from other users.

> **No money is involved in the application's exchange system.**
> Services are exchanged exclusively through time credits, based on the number of hours spent providing a service.

---

## ✨ Features

### 🔎 Browse Available Services

Users can browse the services offered by other users directly from the homepage.

Each service contains information about what is being offered, allowing users to find services that match their needs.

### 🛠️ Manage Services

Authenticated users can:

* Add services they wish to offer
* Manage the services they provide to other users

### 📧 User Communication

Users can communicate directly with each other through their personal email addresses.

When a user is logged in, the service provider's email address is displayed on the service description. This allows the requester and provider to communicate outside the platform and agree on the details of the exchange.

Private user information is **never publicly displayed** to unauthenticated users.

### ⏱️ Time-Based Credit System

The platform uses a credit system based exclusively on time.

For example:

> A user provides 2 hours of gardening assistance → the provider receives **2 hours of credit**, while the requester is charged **2 hours**.

The exchange process works as follows:

1. A user finds a service they are interested in.
2. The requester contacts the service provider by email.
3. Both users discuss and agree on the terms of the service.
4. The service is provided.
5. The service provider reports the number of hours spent providing the service.
6. The requester is asked to confirm the reported number of hours.
7. Once confirmed, the provider receives the corresponding time credits.
8. The requester is debited by the same amount.

This ensures that credits are only transferred after the requester has confirmed the service duration.

---

## 🔐 Privacy

User privacy is an important aspect of the platform.

* Private user information is not publicly exposed.
* Contact information is only made available to authenticated users when necessary for a service exchange.
* Users' personal information is not displayed publicly on the platform.

---

## 🏗️ Architecture & UML Analysis

The application follows the **Model-View-Controller (MVC)** architectural pattern.

A complete UML analysis of the system is included in the [`UML`](./UML) directory.

The UML documentation includes:

* 📌 **Use Case Diagram**
* 📝 **Use Case Scenarios**
* 🧩 **Class Diagram**
* 🔄 **Sequence Diagrams**

These documents describe the functional requirements, system interactions, main entities, and the relationships between the different components of the application.

---

## 🧰 Technologies

| Technology                                                                                                                      | Usage                        |
| ------------------------------------------------------------------------------------------------------------------------------- | ---------------------------- |
| ![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge\&logo=csharp\&logoColor=white)                               | Programming language         |
| ![.NET](https://img.shields.io/badge/.NET%2010-512BD4?style=for-the-badge\&logo=dotnet\&logoColor=white)                        | Application framework        |
| ![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core%20MVC-512BD4?style=for-the-badge\&logo=dotnet\&logoColor=white) | Web application architecture |
| ![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge\&logo=mysql\&logoColor=white)                            | Database                     |
| ![VS Code](https://img.shields.io/badge/VS%20Code-007ACC?style=for-the-badge\&logo=visual-studio-code\&logoColor=white)         | Development environment      |

### Development Stack

* **IDE:** Visual Studio Code
* **Framework:** .NET 10 / ASP.NET Core
* **Language:** C#
* **Architecture:** MVC (Model-View-Controller)
* **Database:** MySQL

---

## 📂 Project Structure

The repository is organized around the MVC architecture, with additional documentation for the system analysis.

```text
.
├── Controllers/       # Application controllers
├── Models/            # Application models
├── Views/             # User interface
├── UML/               # UML analysis and documentation
├── wwwroot/           # Static files
└── ...
```

> The exact project structure may evolve as the application develops.

---

## 🚀 Getting Started

### Prerequisites

Before running the application, make sure the following are installed:

* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* MySQL Server
* A development environment such as Visual Studio Code

### Installation

Clone the repository:

```bash
git clone https://github.com/RaoulTav/ServiSwap.git
cd ServiSwap
```

Restore the .NET dependencies:

```bash
dotnet restore
```

Configure the application's database connection according to your local MySQL environment.

Then run the application:

```bash
dotnet run
```

The application will be available through the local web server address displayed by ASP.NET Core.

---

## 🎯 Project Goals

The project aims to provide a simple platform for **non-commercial mutual assistance**, where users can exchange their time and skills without using traditional currency.

The core principles are:

* 🤝 Mutual assistance
* ⏱️ Time-based exchange
* 💰 No monetary transactions
* 🔐 Respect for user privacy
* 📧 Direct communication between users
* ✅ Confirmation-based credit transfers

---

## 📊 System Overview

The basic interaction between two users can be summarized as:

```text
             ┌──────────────────┐
             │      User A      │
             │    Requester     │
             └────────┬─────────┘
                      │
                 Finds a service
                      │
                      ▼
             ┌──────────────────┐
             │      User B      │
             │     Provider     │
             └────────┬─────────┘
                      │
                 Contact by email
                      │
                      ▼
             ┌──────────────────┐
             │ Service Exchange │
             └────────┬─────────┘
                      │
                Service provided
                      │
                      ▼
             ┌──────────────────┐
             │ Provider reports │
             │      hours       │
             └────────┬─────────┘
                      │
                Requester confirms
                      │
                      ▼
             ┌──────────────────┐
             │  Credit Transfer │
             │                  │
             │ Provider  +X h   │
             │ Requester -X h   │
             └──────────────────┘
```

---

## 📚 Documentation

The UML analysis and system documentation can be found in:

**[`/UML`](./UML)**

It contains the project's:

* Use Case Diagram
* Use Case Scenarios
* Class Diagram
* Sequence Diagrams

---

## 📌 Project Status

🚧 **In development**

The application is currently under development. Features and architecture may evolve as the project progresses.

---

## 📄 License

License information will be added later.
