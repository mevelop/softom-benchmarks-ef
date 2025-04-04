FROM mcr.microsoft.com/dotnet/sdk:9.0.202-bookworm-slim AS build

WORKDIR /src
COPY ["src/SoftOm.Benchmarks.Ef/SoftOm.Benchmarks.Ef.csproj", "SoftOm.Benchmarks.Ef/"]
RUN dotnet restore "SoftOm.Benchmarks.Ef/SoftOm.Benchmarks.Ef.csproj"
COPY /src .

WORKDIR /src/SoftOm.Benchmarks.Ef
ENTRYPOINT dotnet run -c Release