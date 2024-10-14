# Zoo Simulator

![Zoo Simulator Logo](Images/zoo_simulator_logo.png)

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Architecture](#architecture)
- [Design Patterns](#design-patterns)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Application](#running-the-application)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgments](#acknowledgments)

## Introduction

Welcome to **Zoo Simulator**, a WPF (Windows Presentation Foundation) application designed to simulate a dynamic and interactive zoo environment. Manage a variety of animals, monitor their health, and ensure the well-being of your virtual zoo inhabitants through engaging user interactions and real-time updates.

## Features

- **Dynamic Animal Management**
  - Feed and monitor various animals such as elephants, giraffes, and monkeys.
  - Each animal has unique behaviors and life states (Alive, Dying, Dead).

- **Real-Time Health Monitoring**
  - Track the health of each animal with real-time updates.
  - Automated health decay managed by the `DeathManager`.

- **Interactive UI Components**
  - Custom controls like `AnimalControl` and `AnimalCage` for intuitive and visually appealing interactions.
  - Smooth animations for animal movements using the `MovingService`.

- **User Commands**
  - **Feed**: Nourish all animals to improve their health.
  - **Jump**: Simulate a time jump affecting animals' life expectancy.
  - **Open Guide**: Access a guide window for instructions and information.

- **Asynchronous Operations**
  - Maintain a responsive UI with asynchronous tasks handling background operations.

- **Resource Management**
  - Proper disposal of resources to ensure application stability and prevent memory leaks.

## Architecture

The **Zoo Simulator** application is built following the **Model-View-ViewModel (MVVM)** pattern, ensuring a clear separation of concerns and enhancing maintainability. The architecture is modular, allowing for scalability and easy integration of new features.

### Components

- **Model**
  - Represents the data and business logic of the application.
  - Interfaces like `IAnimal` define the properties and behaviors of animals.
  
- **View**
  - Consists of WPF controls such as `AnimalControl` and `AnimalCage`.
  - Defines the visual representation and user interactions.
  
- **ViewModel**
  - `MainWindowController` acts as the ViewModel, managing commands, data bindings, and interactions between the View and the Model.

## Design Patterns

The application leverages several design patterns to promote a robust, maintainable, and scalable codebase:

### 1. Model-View-ViewModel (MVVM)
### 2. Command Pattern
### 3. Observer Pattern
### 4. Strategy Pattern
### 5. Factory Pattern
### 6. Singleton Pattern
### 7. IDisposable Pattern
### 8. State Pattern

## Getting Started

### Prerequisites

- **Operating System**: Windows 10 or later.
- **.NET Framework**: .NET 8.0 or later.
- **IDE**: Visual Studio 2022 or later with WPF development tools installed.

### Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/SalvatoreAmaddio/ZooSimulator.git
