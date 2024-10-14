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

- **Purpose**: Separates the UI (View) from the business logic and data (Model) using a mediator (ViewModel).
- **Implementation**:
  - **View**: `AnimalControl`, `AnimalCage`
  - **ViewModel**: `MainWindowController`
  - **Model**: `IAnimal`, `DeathManager`

### 2. Command Pattern

- **Purpose**: Encapsulates user actions as objects, allowing for parameterization and queuing of requests.
- **Implementation**:
  - Commands like `FeedCMD`, `JumpCMD`, and `OpenGuideCMD` in `MainWindowController` implement `ICommand` to handle user interactions.

### 3. Observer Pattern

- **Purpose**: Establishes a one-to-many dependency between objects, ensuring that changes in one object notify all dependents.
- **Implementation**:
  - Events such as `DeathManager.GameEnded` and property change notifications in `AnimalControl` implement the Observer pattern.

### 4. Strategy Pattern

- **Purpose**: Defines a family of algorithms, encapsulates each one, and makes them interchangeable.
- **Implementation**:
  - `IMovingService` interface and its implementation `MovingService` allow for different movement behaviors of animals.

### 5. Factory Pattern

- **Purpose**: Provides an interface for creating objects without specifying the exact class of the object to be created.
- **Implementation**:
  - `AnimalCage` uses a factory to create instances of `AnimalControl`.

### 6. Dependency Injection (DI)

- **Purpose**: Allows for the decoupling of object creation from object usage, enhancing flexibility and testability.
- **Implementation**:
  - Currently, dependencies like `DeathManager` and `MovingService` are directly instantiated. Future enhancements recommend integrating a DI container for better management.

### 7. Singleton Pattern

- **Purpose**: Ensures a class has only one instance and provides a global point of access to it.
- **Implementation**:
  - Utilization of a shared `Random` instance via a static class to prevent seed duplication issues in random number generation.

### 8. IDisposable Pattern

- **Purpose**: Provides a standardized way to release unmanaged resources and perform cleanup operations.
- **Implementation**:
  - Classes like `AnimalControl`, `AnimalCage`, and `AbstractNotifier` implement `IDisposable` to manage resource cleanup.

## Getting Started

### Prerequisites

- **Operating System**: Windows 10 or later.
- **.NET Framework**: .NET 8.0 or later.
- **IDE**: Visual Studio 2022 or later with WPF development tools installed.

### Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/SalvatoreAmaddio/ZooSimulator.git
