# Solvation Swish client

A basic Swish client maintained by Solvation AB.

## Getting started

Install and register using .AddSwishClient service collection extension

### Prerequisites

It's required to have a Swish account and an API key.

## Usage

```csharp
var swishClient = serviceProvider.GetRequiredService<ISwishClient>();
var response = await swishClient.Authenticate();
```

## Additional documentation

- [Swish developer](https://developer.swish.nu/)
- [Swish](https://www.swish.nu/)

## Feedback

https://github.com/solvationab/SwishClient
