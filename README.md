# 🐱‍👤 Pokemon API - CRUD em C#

Esta é uma API RESTful desenvolvida em **C# com ASP.NET Core**, que permite realizar operações de CRUD (Create, Read, Update, Delete) em uma base de dados fictícia sobre **Pokémons**, seus **donos**, **avaliações**, **categorias** e **países de origem**.

---

## 🔧 Tecnologias Utilizadas

- ASP.NET Core Web API
- Entity Framework Core
- DataContext (Banco de Dados)
- AutoMapper
- Swagger (para documentação)
- RESTful Architecture

---

## 🧩 Estrutura de Dados

### 🐾 Pokémons (`/api/pokemon`)
- `Id`: int
- `Name`: string
- `CategoryId`: int
- `OwnerId`: int
- `CountryId`: int

### 🌍 Countries (`/api/country`)
- `Id`: int
- `Name`: string

### 🧑 Owners (`/api/owner`)
- `Id`: int
- `FirstName`: string
- `LastName`: string
- `Gym`: string

### ⭐ Reviews (`/api/review`)
- `Id`: int
- `Title`: string
- `Text`: string
- `Rating`: int (1 a 5)
- `PokemonId`: int
- `Reviewer`: string

### 🗂️ Categories (`/api/category`)
- `Id`: int
- `Name`: string (ex: "Fogo", "Água", "Elétrico")

---

## 🔁 Endpoints Principais

Todos os endpoints seguem o padrão REST com métodos:

- `GET`: listar ou buscar por ID
- `POST`: criar novo registro
- `PUT`: atualizar registro existente
- `DELETE`: excluir registro

Exemplo:
```http
GET /api/pokemon
POST /api/owner
PUT /api/review/{id}
DELETE /api/category/{id}
