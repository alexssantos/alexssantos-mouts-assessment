[Back to README](../README.md)

## Architecture and Design Patterns

This document describes the architectural decisions and design patterns implemented in the Ambev Developer Evaluation project.

## Table of Contents

1. [Solution Structure](#solution-structure)
2. [Architecture Overview](#architecture-overview)
3. [Domain-Driven Design (DDD)](#domain-driven-design-ddd)
4. [Clean Architecture](#clean-architecture)
5. [Design Patterns](#design-patterns)
6. [CQRS Pattern](#cqrs-pattern)
7. [Repository Pattern](#repository-pattern)
8. [Mediator Pattern](#mediator-pattern)
9. [Dependency Injection](#dependency-injection)
10. [Data Mapping](#data-mapping)
11. [Validation Strategy](#validation-strategy)
12. [Event-Driven Architecture](#event-driven-architecture)

---

## Solution Structure

The solution is organized following **Hexagonal Architecture (Ports & Adapters)** principles, with clear separation between core business logic and external concerns.

### Visual Studio Solution Organization

```
Ambev.DeveloperEvaluation.sln
│
├── 📁 Core
│   ├── 📁 Domain
│   │   └── Ambev.DeveloperEvaluation.Domain
│   └── 📁 Application
│       └── Ambev.DeveloperEvaluation.Application
│
├── 📁 Adapters
│   ├── 📁 Drivers (Primary/Inbound)
│   │   └── 📁 WebApi
│   │       └── Ambev.DeveloperEvaluation.WebApi
│   └── 📁 Driven (Secondary/Outbound)
│       └── 📁 Infrastructure
│           └── Ambev.DeveloperEvaluation.ORM
│
├── 📁 Crosscutting
│   ├── Ambev.DeveloperEvaluation.Common
│   └── Ambev.DeveloperEvaluation.IoC
│
└── 📁 Tests
    ├── 📁 Unit
    │   └── Ambev.DeveloperEvaluation.Unit
    ├── 📁 Integration
    │   └── Ambev.DeveloperEvaluation.Integration
    └── 📁 Functional
        └── Ambev.DeveloperEvaluation.Functional
```

### Hexagonal Architecture Layers

#### **Core** (Business Logic)

The innermost layer containing the business rules and domain model:

**Domain** (`Ambev.DeveloperEvaluation.Domain`)
- Pure business logic
- No external dependencies (except Common)
- Contains:
  - Entities (`Sale`, `User`, `SaleItem`)
  - Domain Events (`SaleCreatedEvent`, `SaleCancelledEvent`)
  - Repository Interfaces (`ISaleRepository`, `IUserRepository`)
  - Domain Exceptions (`DomainException`)

**Application** (`Ambev.DeveloperEvaluation.Application`)
- Use cases and application logic
- Orchestrates domain entities
- Contains:
  - Commands (write operations)
  - Queries (read operations)
  - Handlers (CQRS implementation)
  - Validators (FluentValidation)
  - Event Handlers

#### **Adapters** (Integration Points)

**Drivers - Primary Adapters** (Inbound/Left Side)
- Receive requests from external sources
- Drive the application use cases

`WebApi` (`Ambev.DeveloperEvaluation.WebApi`)
- REST API endpoints
- Controllers
- Request/Response DTOs
- API-specific validation
- Swagger documentation

**Driven - Secondary Adapters** (Outbound/Right Side)
- Implement technical capabilities
- Driven by application needs

`Infrastructure` (`Ambev.DeveloperEvaluation.ORM`)
- Data persistence (PostgreSQL)
- Repository implementations
- Entity Framework Core configurations
- Database migrations

#### **Crosscutting** (Shared Resources)

`Common` (`Ambev.DeveloperEvaluation.Common`)
- Shared utilities
- Security components
- Validation framework
- Health checks
- Logging extensions

`IoC` (`Ambev.DeveloperEvaluation.IoC`)
- Dependency injection configuration
- Module initializers
- Service registration

#### **Tests** (Quality Assurance)

`Unit` (`Ambev.DeveloperEvaluation.Unit`)
- Unit tests for isolated components
- Domain logic tests
- Handler tests
- Validator tests

`Integration` (`Ambev.DeveloperEvaluation.Integration`)
- Integration tests
- Repository tests
- Database integration tests

`Functional` (`Ambev.DeveloperEvaluation.Functional`)
- End-to-end tests
- API endpoint tests
- Full workflow tests

### Dependency Flow

```
┌─────────────────────────────────────────┐
│           Drivers (WebApi)              │ ◄── External requests
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│        Application (Use Cases)          │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│      Domain (Business Logic) ◄──────────┼── Repository Interfaces
└─────────────────────────────────────────┘
                    ↑
┌─────────────────────────────────────────┐
│   Driven (Infrastructure/ORM)           │ ◄── Database
└─────────────────────────────────────────┘
```

**Key Principles:**
- ✅ Dependencies point inward (toward Domain)
- ✅ Core has no knowledge of infrastructure
- ✅ Infrastructure implements Core interfaces
- ✅ WebApi depends on Application, not Domain directly

### Benefits of This Structure

1. **Testability**
   - Core business logic can be tested without infrastructure
   - Easy to mock dependencies
   - Clear test boundaries

2. **Flexibility**
   - Easy to swap implementations (e.g., change database)
   - Add new adapters without changing core
   - Technology-agnostic core

3. **Maintainability**
   - Clear separation of concerns
   - Easy to locate code by responsibility
   - Reduced coupling between components

4. **Scalability**
   - Independent scaling of adapters
   - Can add new ports without affecting existing ones
   - Clear extension points

---

## Architecture Overview

The project follows a **layered architecture** with clear separation of concerns, organized into the following layers:

```
┌─────────────────────────────────────┐
│         Presentation Layer          │
│     (WebApi - Controllers/DTOs)     │
└─────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────┐
│        Application Layer            │
│  (Use Cases - Commands/Queries)     │
└─────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────┐
│          Domain Layer               │
│   (Entities, Rules, Interfaces)     │
└─────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────┐
│      Infrastructure Layer           │
│  (ORM, Repositories, External)      │
└─────────────────────────────────────┘
```

### Ports & Adapters Visualization

```
        External World
             │
             ↓
    ┌────────────────┐
    │  Primary Port  │ (HTTP/REST)
    │   (WebApi)     │
    └────────────────┘
             │
             ↓
    ┌────────────────┐
    │  Application   │ (Use Cases)
    └────────────────┘
             │
             ↓
    ┌────────────────┐
    │    Domain      │ (Business Rules)
    └────────────────┘
             │
             ↓
    ┌────────────────┐
    │ Secondary Port │ (Repository Interface)
    └────────────────┘
             │
             ↓
    ┌────────────────┐
    │Secondary Adapter│ (Database/ORM)
    └────────────────┘
             │
             ↓
        Database
```

---

## Domain-Driven Design (DDD)

### Bounded Contexts

The application is organized around business domains (e.g., Sales, Users, Products), each with its own bounded context.

### Entities

Domain entities encapsulate business logic and maintain their own invariants:

```csharp
public class Sale : BaseEntity
{
    private readonly List<SaleItem> _items;
    
    public void AddItem(string productId, string productName, int quantity, decimal unitPrice)
    {
        // Business rules enforced here
        if (quantity > 20)
            throw new DomainException("Cannot add more than 20 identical items");
            
        var discount = CalculateDiscount(quantity);
        var item = SaleItem.Create(productId, productName, quantity, unitPrice, discount);
        _items.Add(item);
    }
}
```

### Value Objects

Immutable objects that represent domain concepts without identity:

```csharp
public class SaleItem
{
    public decimal TotalAmount => (UnitPrice * Quantity) - Discount;
    // ... other properties
}
```

### Domain Events

Events that represent something significant that happened in the domain:

- `SaleCreatedEvent`
- `SaleModifiedEvent`
- `SaleCancelledEvent`
- `ItemCancelledEvent`

### External Identities Pattern

For referencing entities from other domains, we use the **External Identities** pattern with denormalization:

```csharp
public class Sale
{
    public string CustomerId { get; private set; }
    public string CustomerName { get; private set; } // Denormalized
    public string BranchId { get; private set; }
    public string BranchName { get; private set; } // Denormalized
}
```

This allows us to maintain consistency within our bounded context while avoiding complex joins across domains.

---

## Clean Architecture

The project follows **Clean Architecture** principles with dependency inversion:

### Layer Dependencies

```
WebApi → Application → Domain ← Infrastructure
                          ↑
                          └── Common
```

### Dependency Rules

1. **Inner layers don't depend on outer layers**
2. **Domain layer has no dependencies** (except Common utilities)
3. **Infrastructure depends on Domain** (implements interfaces)
4. **Application depends on Domain** (orchestrates use cases)
5. **WebApi depends on Application** (presents data)

### Project References

```
┌─────────────────────────────────────┐
│  Ambev.DeveloperEvaluation.WebApi   │
│  (Presentation/REST API)            │
└─────────────────────────────────────┘
          │ references
          ↓
┌─────────────────────────────────────┐
│ Ambev.DeveloperEvaluation.Application│
│  (Use Cases/CQRS)                   │
└─────────────────────────────────────┘
          │ references
          ↓
┌─────────────────────────────────────┐
│  Ambev.DeveloperEvaluation.Domain   │
│  (Business Logic)                   │
└─────────────────────────────────────┘
          ↑ implements
          │
┌─────────────────────────────────────┐
│   Ambev.DeveloperEvaluation.ORM     │
│  (Data Access/EF Core)              │
└─────────────────────────────────────┘
```

---

## Design Patterns

### 1. Repository Pattern

Abstracts data access logic and provides a collection-like interface:

```csharp
public interface ISaleRepository : IRepository<Sale>
{
    Task<Sale?> GetBySaleNumberAsync(int saleNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sale>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
}
```

**Benefits:**
- Decouples domain from data access
- Enables testing with mock repositories
- Centralizes data access logic

### 2. Unit of Work Pattern

Implemented through Entity Framework's `DbContext`:

```csharp
public class DefaultContext : DbContext
{
    public DbSet<Sale> Sales { get; set; }
    public DbSet<User> Users { get; set; }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Transaction management
        return await base.SaveChangesAsync(cancellationToken);
    }
}
```

### 3. Factory Pattern

Used for entity creation with proper validation:

```csharp
public static Sale Create(
    int saleNumber,
    string customerId,
    string customerName,
    string branchId,
    string branchName)
{
    var sale = new Sale
    {
        SaleNumber = saleNumber,
        SaleDate = DateTime.UtcNow,
        CustomerId = customerId,
        CustomerName = customerName,
        BranchId = branchId,
        BranchName = branchName
    };
    
    sale.AddDomainEvent(new SaleCreatedEvent(sale.Id));
    return sale;
}
```

### 4. Strategy Pattern

Used for discount calculation:

```csharp
private decimal CalculateDiscount(int quantity)
{
    if (quantity < 4)
        return 0;
        
    if (quantity >= 10 && quantity <= 20)
        return 0.20m; // 20% discount
        
    if (quantity >= 4)
        return 0.10m; // 10% discount
        
    return 0;
}
```

---

## CQRS Pattern

**Command Query Responsibility Segregation** separates read and write operations:

### Commands (Write Operations)

```csharp
public class CreateSaleCommand : IRequest<SaleResult>
{
    public int SaleNumber { get; set; }
    public string CustomerId { get; set; }
    public List<CreateSaleItemDto> Items { get; set; }
}

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, SaleResult>
{
    public async Task<SaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        // Command handling logic
    }
}
```

### Queries (Read Operations)

```csharp
public class GetSaleByIdQuery : IRequest<SaleResult>
{
    public Guid Id { get; set; }
}

public class GetSaleByIdHandler : IQueryHandler<GetSaleByIdQuery, SaleResult>
{
    public async Task<SaleResult> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        // Query handling logic
    }
}
```

**Benefits:**
- Optimized read and write models
- Scalability (can scale reads and writes independently)
- Clarity (separates intentions)

---

## Mediator Pattern

Uses **MediatR** library to implement the Mediator pattern:

```csharp
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;
    
    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new ApiResponseWithData<CreateSaleResponse> { Data = response });
    }
}
```

**Benefits:**
- Decouples request senders from handlers
- Centralizes cross-cutting concerns (logging, validation)
- Simplifies controller logic

---

## Dependency Injection

The project uses .NET's built-in DI container with modular initialization:

### Module Initializers

```csharp
public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        // ... other registrations
    }
}
```

### Registration in Program.cs

```csharp
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(ApplicationLayer).Assembly,
    typeof(Program).Assembly
));
```

---

## Data Mapping

### AutoMapper Profiles

Organized by use case:

```csharp
public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<Sale, CreateSaleResponse>();
    }
}
```

### Manual Mapping for Complex Scenarios

```csharp
public class SaleProfile : Profile
{
    public SaleProfile()
    {
        CreateMap<Sale, SaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
    }
}
```

---

## Validation Strategy

### FluentValidation

Request validation at API layer:

```csharp
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(x => x.SaleNumber)
            .GreaterThan(0)
            .WithMessage("Sale number must be greater than 0");
            
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");
            
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");
    }
}
```

### Domain Validation

Business rules enforced in entities:

```csharp
public void AddItem(string productId, string productName, int quantity, decimal unitPrice)
{
    if (IsCancelled)
        throw new DomainException("Cannot add items to a cancelled sale");
        
    if (quantity > 20)
        throw new DomainException("Cannot add more than 20 identical items");
}
```

---

## Event-Driven Architecture

### Domain Events

```csharp
public abstract class BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = new();
    
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public void AddDomainEvent(DomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }
}
```

### Event Handlers

```csharp
public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;
    
    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sale created with ID: {SaleId}", notification.SaleId);
        // Here you could publish to a message broker like RabbitMQ, Azure Service Bus, etc.
    }
}
```

### Event Types

- **SaleCreatedEvent**: Triggered when a new sale is created
- **SaleModifiedEvent**: Triggered when a sale is updated
- **SaleCancelledEvent**: Triggered when a sale is cancelled
- **ItemCancelledEvent**: Triggered when an item is cancelled

---

## Testing Strategy

### Unit Tests

Testing domain logic and handlers:

```csharp
public class CreateSaleHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var repository = Substitute.For<ISaleRepository>();
        var mapper = Substitute.For<IMapper>();
        var handler = new CreateSaleHandler(repository, mapper);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
    }
}
```

### Integration Tests

Testing API endpoints and database interactions:

```csharp
public class SalesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task CreateSale_ValidRequest_ReturnsCreated()
    {
        // Arrange & Act & Assert
    }
}
```

---

## Best Practices Implemented

1. **Separation of Concerns**: Each layer has a single responsibility
2. **Dependency Inversion**: High-level modules don't depend on low-level modules
3. **Single Responsibility Principle**: Each class has one reason to change
4. **Open/Closed Principle**: Open for extension, closed for modification
5. **Interface Segregation**: Clients shouldn't depend on interfaces they don't use
6. **DRY (Don't Repeat Yourself)**: Code reuse through base classes and utilities
7. **YAGNI (You Aren't Gonna Need It)**: Only implement what's needed
8. **Explicit is better than implicit**: Clear naming and explicit types
9. **Fail Fast**: Validate early and throw exceptions when invariants are violated
10. **Immutability**: Use private setters and readonly collections

---

## Technology Stack

- **.NET 8.0**: Latest LTS version of .NET
- **Entity Framework Core**: ORM for database access
- **PostgreSQL**: Primary relational database
- **MediatR**: Mediator pattern implementation
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Request validation
- **xUnit**: Unit testing framework
- **NSubstitute**: Mocking framework
- **Bogus**: Test data generation

---

<div style="display: flex; justify-content: space-between;">
  <a href="./overview.md">Previous: Overview</a>
  <a href="./solution-structure.md">Next: Solution Structure</a>
</div>