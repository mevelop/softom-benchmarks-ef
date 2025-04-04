## Запуск

#### Поднятие БД
> Если нет своего PostgreSQL (иначе поменять строку подключения в коде):
> 
> `docker-compose -p softom-benchmarks -f docker-compose.yml up -d db`

#### Создание миграции
> Если есть желание поиграться или создать самому с нуля:
> 
> `cd src/SoftOm.Benchmarks.Ef`
> 
> `dotnet ef migrations add InitialCreate`

#### Запуск тестирования
> `cd src/SoftOm.Benchmarks.Ef`
> 
> `dotnet run -c Release`
> 
> Или через docker:
> 
> запуск `docker-compose -p softom-benchmarks -f docker-compose.yml up -d --build --force-recreate app`
> 
> удаление `docker-compose -p softom-benchmarks -f docker-compose.yml down app`


## Результаты
#### Асинхронные
```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.3.2 (24D81) [Darwin 24.3.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK 9.0.202
  [Host]   : .NET 9.0.3 (9.0.325.11113), Arm64 RyuJIT AdvSIMD
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), Arm64 RyuJIT AdvSIMD

Job=.NET 9.0  Runtime=.NET 9.0  

```
<details><summary>Подробности</summary>

| Method                        | N   | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated   | Alloc Ratio |
|------------------------------ |---- |----------:|----------:|----------:|----------:|------:|--------:|----------:|----------:|---------:|------------:|------------:|
| Ef_Load_Tracking_ToList       | 5   |  1.333 ms | 0.0207 ms | 0.0184 ms |  1.330 ms |  1.20 |    0.02 |   66.4063 |   23.4375 |        - |   422.05 KB |        1.62 |
| Ef_FromSql_Tracking_ToList    | 5   |  1.314 ms | 0.0112 ms | 0.0105 ms |  1.314 ms |  1.18 |    0.02 |   66.4063 |   27.3438 |        - |   420.69 KB |        1.62 |
| Ef_Include_Tracking_ToList    | 5   |  1.294 ms | 0.0249 ms | 0.0267 ms |  1.283 ms |  1.16 |    0.03 |   70.3125 |   31.2500 |        - |   437.02 KB |        1.68 |
| Ef_FromSql_NoTracking_ToList  | 5   |  1.289 ms | 0.0168 ms | 0.0149 ms |  1.285 ms |  1.16 |    0.02 |   54.6875 |   17.5781 |        - |   338.15 KB |        1.30 |
| Ef_Include_NoTracking_ToArray | 5   |  1.249 ms | 0.0211 ms | 0.0571 ms |  1.229 ms |  1.12 |    0.05 |   60.5469 |   19.5313 |        - |   370.42 KB |        1.42 |
| Ef_LoadAsList_Tracking_ToList | 5   |  1.249 ms | 0.0107 ms | 0.0101 ms |  1.248 ms |  1.12 |    0.02 |   66.4063 |   23.4375 |        - |   418.42 KB |        1.61 |
| Ef_Load_NoTracking_ToDict     | 5   |  1.234 ms | 0.0160 ms | 0.0142 ms |  1.230 ms |  1.11 |    0.02 |   54.6875 |   17.5781 |        - |    334.2 KB |        1.28 |
| Dapper_TwoQuery               | 5   |  1.112 ms | 0.0186 ms | 0.0165 ms |  1.109 ms |  1.00 |    0.02 |   41.0156 |   11.7188 |        - |   260.46 KB |        1.00 |
|                               |     |           |           |           |           |       |         |           |           |          |             |             |
| Ef_Include_Tracking_ToList    | 50  |  8.342 ms | 0.1409 ms | 0.1249 ms |  8.322 ms |  1.35 |    0.02 |  625.0000 |  312.5000 |        - |  3822.35 KB |        1.50 |
| Ef_Include_NoTracking_ToArray | 50  |  8.331 ms | 0.1624 ms | 0.1439 ms |  8.374 ms |  1.35 |    0.02 |  515.6250 |  250.0000 |        - |  3151.83 KB |        1.23 |
| Ef_FromSql_Tracking_ToList    | 50  |  6.584 ms | 0.1284 ms | 0.1374 ms |  6.541 ms |  1.06 |    0.02 |  578.1250 |  281.2500 |        - |  3561.18 KB |        1.39 |
| Ef_FromSql_NoTracking_ToList  | 50  |  6.347 ms | 0.0512 ms | 0.0428 ms |  6.340 ms |  1.03 |    0.01 |  445.3125 |  218.7500 |        - |  2723.84 KB |        1.07 |
| Dapper_TwoQuery               | 50  |  6.191 ms | 0.0551 ms | 0.0515 ms |  6.177 ms |  1.00 |    0.01 |  414.0625 |  203.1250 |        - |  2556.12 KB |        1.00 |
| Ef_LoadAsList_Tracking_ToList | 50  |  5.911 ms | 0.1172 ms | 0.1564 ms |  5.896 ms |  0.95 |    0.03 |  578.1250 |  281.2500 |        - |  3573.82 KB |        1.40 |
| Ef_Load_Tracking_ToList       | 50  |  5.872 ms | 0.0725 ms | 0.0605 ms |  5.872 ms |  0.95 |    0.01 |  578.1250 |  281.2500 |        - |  3572.87 KB |        1.40 |
| Ef_Load_NoTracking_ToDict     | 50  |  5.840 ms | 0.1140 ms | 0.1067 ms |  5.877 ms |  0.94 |    0.02 |  445.3125 |  218.7500 |        - |  2720.59 KB |        1.06 |
|                               |     |           |           |           |           |       |         |           |           |          |             |             |
| Ef_Include_Tracking_ToList    | 100 | 16.051 ms | 0.1864 ms | 0.1743 ms | 16.050 ms |  1.37 |    0.02 | 1375.0000 |  718.7500 | 218.7500 |  7606.82 KB |        1.49 |
| Ef_Include_NoTracking_ToArray | 100 | 14.796 ms | 0.1792 ms | 0.1677 ms | 14.785 ms |  1.26 |    0.02 | 1000.0000 |  500.0000 |        - |  6240.56 KB |        1.22 |
| Ef_FromSql_Tracking_ToList    | 100 | 13.361 ms | 0.1340 ms | 0.1188 ms | 13.359 ms |  1.14 |    0.01 | 1250.0000 |  625.0000 | 250.0000 |  7074.53 KB |        1.39 |
| Ef_Load_Tracking_ToList       | 100 | 12.375 ms | 0.1121 ms | 0.1048 ms | 12.387 ms |  1.06 |    0.01 | 1281.2500 |  656.2500 | 250.0000 |  7096.46 KB |        1.39 |
| Ef_FromSql_NoTracking_ToList  | 100 | 12.186 ms | 0.2410 ms | 0.3606 ms | 12.029 ms |  1.04 |    0.03 |  875.0000 |  437.5000 |        - |  5375.78 KB |        1.05 |
| Ef_LoadAsList_Tracking_ToList | 100 | 12.034 ms | 0.1555 ms | 0.1378 ms | 12.026 ms |  1.03 |    0.01 | 1281.2500 |  656.2500 | 250.0000 |  7102.33 KB |        1.39 |
| Dapper_TwoQuery               | 100 | 11.706 ms | 0.1119 ms | 0.1046 ms | 11.685 ms |  1.00 |    0.01 |  828.1250 |  406.2500 |        - |  5105.91 KB |        1.00 |
| Ef_Load_NoTracking_ToDict     | 100 | 10.856 ms | 0.1514 ms | 0.1416 ms | 10.829 ms |  0.93 |    0.01 |  875.0000 |  437.5000 |        - |  5372.53 KB |        1.05 |
|                               |     |           |           |           |           |       |         |           |           |          |             |             |
| Ef_Include_Tracking_ToList    | 300 | 79.758 ms | 1.0585 ms | 0.9383 ms | 79.597 ms |  2.74 |    0.03 | 3857.1429 | 1571.4286 | 571.4286 | 22227.02 KB |        1.45 |
| Ef_Include_NoTracking_ToArray | 300 | 77.837 ms | 1.1145 ms | 0.9307 ms | 77.891 ms |  2.67 |    0.03 | 3428.5714 | 1428.5714 | 428.5714 | 18581.94 KB |        1.21 |
| Ef_LoadAsList_Tracking_ToList | 300 | 30.457 ms | 0.2823 ms | 0.2641 ms | 30.428 ms |  1.05 |    0.01 | 3687.5000 | 1625.0000 | 562.5000 | 20736.58 KB |        1.35 |
| Ef_FromSql_Tracking_ToList    | 300 | 30.241 ms | 0.4804 ms | 0.4494 ms | 30.268 ms |  1.04 |    0.02 | 3750.0000 | 1625.0000 | 593.7500 | 20605.83 KB |        1.34 |
| Ef_Load_Tracking_ToList       | 300 | 29.412 ms | 0.4657 ms | 0.4356 ms | 29.315 ms |  1.01 |    0.02 | 3718.7500 | 1656.2500 | 593.7500 | 20670.82 KB |        1.35 |
| Dapper_TwoQuery               | 300 | 29.131 ms | 0.1600 ms | 0.1497 ms | 29.170 ms |  1.00 |    0.01 | 2750.0000 | 1250.0000 | 281.2500 | 15340.08 KB |        1.00 |
| Ef_Load_NoTracking_ToDict     | 300 | 28.798 ms | 0.2942 ms | 0.2608 ms | 28.690 ms |  0.99 |    0.01 | 2906.2500 | 1281.2500 | 375.0000 | 15949.15 KB |        1.04 |
| Ef_FromSql_NoTracking_ToList  | 300 | 28.585 ms | 0.2276 ms | 0.2018 ms | 28.543 ms |  0.98 |    0.01 | 2906.2500 | 1281.2500 | 375.0000 | 15948.52 KB |        1.04 |

</details><br/>

```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.5608/22H2/2022Update)
AMD Ryzen 5 5600G with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.202
  [Host]   : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2

Job=.NET 9.0  Runtime=.NET 9.0  

```
<details><summary>Подробности</summary>

| Method                        | N   | Mean       | Error     | StdDev    | Median     | Ratio | RatioSD | Gen0      | Gen1      | Gen2      | Allocated   | Alloc Ratio |
|------------------------------ |---- |-----------:|----------:|----------:|-----------:|------:|--------:|----------:|----------:|----------:|------------:|------------:|
| Ef_Include_NoTracking_ToArray | 5   |   2.172 ms | 0.2827 ms | 0.8201 ms |   1.535 ms |  1.57 |    0.59 |   42.9688 |   11.7188 |         - |   373.22 KB |        1.44 |
| Ef_FromSql_NoTracking_ToList  | 5   |   1.961 ms | 0.0392 ms | 0.0707 ms |   1.950 ms |  1.41 |    0.05 |         - |         - |         - |   340.29 KB |        1.31 |
| Ef_FromSql_Tracking_ToList    | 5   |   1.730 ms | 0.0319 ms | 0.0267 ms |   1.733 ms |  1.25 |    0.02 |   50.7813 |   19.5313 |         - |   420.29 KB |        1.62 |
| Ef_LoadAsList_Tracking_ToList | 5   |   1.641 ms | 0.0322 ms | 0.0643 ms |   1.614 ms |  1.18 |    0.05 |   46.8750 |   15.6250 |         - |   417.99 KB |        1.61 |
| Ef_Load_Tracking_ToList       | 5   |   1.621 ms | 0.0280 ms | 0.0234 ms |   1.615 ms |  1.17 |    0.02 |   46.8750 |   15.6250 |         - |   421.69 KB |        1.62 |
| Ef_Include_Tracking_ToList    | 5   |   1.621 ms | 0.0260 ms | 0.0217 ms |   1.614 ms |  1.17 |    0.02 |   46.8750 |   15.6250 |         - |   436.56 KB |        1.68 |
| Ef_Load_NoTracking_ToDict     | 5   |   1.491 ms | 0.0202 ms | 0.0189 ms |   1.489 ms |  1.08 |    0.01 |   39.0625 |   11.7188 |         - |   333.62 KB |        1.28 |
| Dapper_TwoQuery               | 5   |   1.387 ms | 0.0106 ms | 0.0094 ms |   1.385 ms |  1.00 |    0.01 |   31.2500 |    9.7656 |         - |   259.88 KB |        1.00 |
|                               |     |            |           |           |            |       |         |           |           |           |             |             |
| Ef_Include_Tracking_ToList    | 50  |   9.882 ms | 0.1907 ms | 0.1958 ms |   9.787 ms |  1.89 |    0.04 |  453.1250 |  390.6250 |         - |  3816.24 KB |        1.50 |
| Ef_Include_NoTracking_ToArray | 50  |   8.211 ms | 0.1573 ms | 0.1544 ms |   8.207 ms |  1.57 |    0.03 |  375.0000 |  296.8750 |         - |  3174.71 KB |        1.25 |
| Ef_FromSql_Tracking_ToList    | 50  |   6.396 ms | 0.1237 ms | 0.1157 ms |   6.343 ms |  1.22 |    0.02 |  406.2500 |  312.5000 |         - |  3555.48 KB |        1.39 |
| Ef_Load_Tracking_ToList       | 50  |   5.755 ms | 0.1129 ms | 0.1159 ms |   5.740 ms |  1.10 |    0.02 |  406.2500 |  281.2500 |         - |  3566.84 KB |        1.40 |
| Ef_LoadAsList_Tracking_ToList | 50  |   5.707 ms | 0.1132 ms | 0.1729 ms |   5.674 ms |  1.09 |    0.03 |  406.2500 |  281.2500 |         - |  3568.04 KB |        1.40 |
| Ef_FromSql_NoTracking_ToList  | 50  |   5.671 ms | 0.0751 ms | 0.0586 ms |   5.656 ms |  1.09 |    0.01 |  328.1250 |   78.1250 |         - |  2718.59 KB |        1.07 |
| Dapper_TwoQuery               | 50  |   5.226 ms | 0.0433 ms | 0.0361 ms |   5.226 ms |  1.00 |    0.01 |  304.6875 |  242.1875 |         - |  2548.93 KB |        1.00 |
| Ef_Load_NoTracking_ToDict     | 50  |   4.988 ms | 0.0677 ms | 0.1034 ms |   4.975 ms |  0.95 |    0.02 |  328.1250 |  125.0000 |         - |  2715.95 KB |        1.07 |
|                               |     |            |           |           |            |       |         |           |           |           |             |             |
| Ef_Include_Tracking_ToList    | 100 |  23.384 ms | 0.4673 ms | 0.4142 ms |  23.289 ms |  2.39 |    0.12 | 1218.7500 | 1187.5000 |  375.0000 |  7600.43 KB |        1.49 |
| Ef_Load_Tracking_ToList       | 100 |  17.178 ms | 0.3426 ms | 0.9831 ms |  17.255 ms |  1.76 |    0.13 | 1142.8571 | 1071.4286 |  357.1429 |  7085.94 KB |        1.39 |
| Ef_FromSql_Tracking_ToList    | 100 |  16.683 ms | 0.3241 ms | 0.3858 ms |  16.684 ms |  1.71 |    0.09 | 1125.0000 | 1093.7500 |  375.0000 |  7065.58 KB |        1.39 |
| Ef_LoadAsList_Tracking_ToList | 100 |  16.269 ms | 0.3199 ms | 0.3422 ms |  16.237 ms |  1.66 |    0.09 | 1156.2500 | 1125.0000 |  343.7500 |  7094.42 KB |        1.39 |
| Ef_Include_NoTracking_ToArray | 100 |  14.285 ms | 0.1072 ms | 0.0951 ms |  14.285 ms |  1.46 |    0.07 |  765.6250 |  671.8750 |         - |  6287.06 KB |        1.23 |
| Dapper_TwoQuery               | 100 |   9.808 ms | 0.1950 ms | 0.4963 ms |   9.601 ms |  1.00 |    0.07 |  609.3750 |  500.0000 |         - |  5092.39 KB |        1.00 |
| Ef_FromSql_NoTracking_ToList  | 100 |   9.633 ms | 0.1705 ms | 0.1423 ms |   9.591 ms |  0.98 |    0.05 |  656.2500 |  562.5000 |         - |  5363.07 KB |        1.05 |
| Ef_Load_NoTracking_ToDict     | 100 |   8.517 ms | 0.1451 ms | 0.1133 ms |   8.448 ms |  0.87 |    0.04 |  656.2500 |  562.5000 |         - |  5362.01 KB |        1.05 |
|                               |     |            |           |           |            |       |         |           |           |           |             |             |
| Ef_Include_Tracking_ToList    | 300 | 193.153 ms | 3.8458 ms | 5.7563 ms | 193.072 ms |  7.55 |    0.27 | 3500.0000 | 3000.0000 | 1000.0000 | 22216.53 KB |        1.45 |
| Ef_Include_NoTracking_ToArray | 300 | 185.108 ms | 3.6546 ms | 5.5809 ms | 186.125 ms |  7.24 |    0.26 | 2666.6667 | 2333.3333 |  666.6667 | 18738.64 KB |        1.22 |
| Ef_Load_Tracking_ToList       | 300 |  46.054 ms | 0.9030 ms | 1.7180 ms |  46.088 ms |  1.80 |    0.08 | 3300.0000 | 3200.0000 | 1000.0000 | 20658.57 KB |        1.35 |
| Ef_LoadAsList_Tracking_ToList | 300 |  46.007 ms | 0.9069 ms | 1.3849 ms |  45.670 ms |  1.80 |    0.06 | 3300.0000 | 3200.0000 | 1000.0000 | 20723.87 KB |        1.35 |
| Ef_FromSql_Tracking_ToList    | 300 |  46.002 ms | 0.9113 ms | 1.9021 ms |  46.164 ms |  1.80 |    0.08 | 3333.3333 | 3250.0000 | 1083.3333 | 20600.93 KB |        1.35 |
| Ef_FromSql_NoTracking_ToList  | 300 |  31.334 ms | 0.6229 ms | 1.2002 ms |  31.024 ms |  1.23 |    0.05 | 2750.0000 | 2625.0000 |  812.5000 | 15908.84 KB |        1.04 |
| Ef_Load_NoTracking_ToDict     | 300 |  30.725 ms | 0.5729 ms | 0.8574 ms |  30.740 ms |  1.20 |    0.04 | 2733.3333 | 2600.0000 |  800.0000 | 15904.84 KB |        1.04 |
| Dapper_TwoQuery               | 300 |  25.584 ms | 0.4980 ms | 0.5329 ms |  25.682 ms |  1.00 |    0.03 | 2218.7500 | 2093.7500 |  750.0000 |  15302.9 KB |        1.00 |

</details><br/>

#### Синхронные
```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.3.2 (24D81) [Darwin 24.3.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK 9.0.202
  [Host]   : .NET 9.0.3 (9.0.325.11113), Arm64 RyuJIT AdvSIMD
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), Arm64 RyuJIT AdvSIMD

Job=.NET 9.0  Runtime=.NET 9.0  

```
<details><summary>Подробности</summary>

| Method                        | N   | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated   | Alloc Ratio |
|------------------------------ |---- |----------:|----------:|----------:|------:|--------:|----------:|----------:|---------:|------------:|------------:|
| Ef_FromSql_Tracking_ToList    | 5   |  1.265 ms | 0.0085 ms | 0.0076 ms |  1.16 |    0.01 |   66.4063 |   23.4375 |        - |   417.32 KB |        1.61 |
| Ef_Include_Tracking_ToList    | 5   |  1.262 ms | 0.0131 ms | 0.0116 ms |  1.16 |    0.01 |   70.3125 |   23.4375 |        - |   434.87 KB |        1.68 |
| Ef_Load_Tracking_ToList       | 5   |  1.242 ms | 0.0086 ms | 0.0067 ms |  1.14 |    0.01 |   66.4063 |   23.4375 |        - |   419.01 KB |        1.62 |
| Ef_FromSql_NoTracking_ToList  | 5   |  1.229 ms | 0.0086 ms | 0.0072 ms |  1.13 |    0.01 |   52.7344 |   15.6250 |        - |   335.03 KB |        1.29 |
| Ef_Include_NoTracking_ToArray | 5   |  1.227 ms | 0.0140 ms | 0.0124 ms |  1.12 |    0.01 |   58.5938 |   15.6250 |        - |   368.15 KB |        1.42 |
| Ef_LoadAsList_Tracking_ToList | 5   |  1.206 ms | 0.0097 ms | 0.0086 ms |  1.11 |    0.01 |   66.4063 |   23.4375 |        - |   414.96 KB |        1.60 |
| Ef_Load_NoTracking_ToDict     | 5   |  1.189 ms | 0.0226 ms | 0.0200 ms |  1.09 |    0.02 |   52.7344 |   15.6250 |        - |   330.64 KB |        1.28 |
| Dapper_TwoQuery               | 5   |  1.091 ms | 0.0087 ms | 0.0081 ms |  1.00 |    0.01 |   41.0156 |   13.6719 |        - |   258.97 KB |        1.00 |
|                               |     |           |           |           |       |         |           |           |          |             |             |
| Ef_Include_Tracking_ToList    | 50  |  8.430 ms | 0.1367 ms | 0.1212 ms |  1.39 |    0.02 |  593.7500 |  281.2500 |        - |  3818.85 KB |        1.49 |
| Ef_Include_NoTracking_ToArray | 50  |  8.247 ms | 0.0811 ms | 0.0719 ms |  1.36 |    0.01 |  500.0000 |  234.3750 |        - |  3146.59 KB |        1.23 |
| Ef_FromSql_Tracking_ToList    | 50  |  6.417 ms | 0.0464 ms | 0.0387 ms |  1.06 |    0.01 |  578.1250 |  281.2500 |        - |  3556.41 KB |        1.39 |
| Ef_FromSql_NoTracking_ToList  | 50  |  6.261 ms | 0.0427 ms | 0.0379 ms |  1.03 |    0.01 |  437.5000 |  218.7500 |        - |  2716.76 KB |        1.06 |
| Dapper_TwoQuery               | 50  |  6.072 ms | 0.0358 ms | 0.0317 ms |  1.00 |    0.01 |  414.0625 |  203.1250 |        - |  2554.96 KB |        1.00 |
| Ef_LoadAsList_Tracking_ToList | 50  |  5.973 ms | 0.0548 ms | 0.0512 ms |  0.98 |    0.01 |  578.1250 |  281.2500 |        - |  3568.67 KB |        1.40 |
| Ef_Load_Tracking_ToList       | 50  |  5.854 ms | 0.0369 ms | 0.0346 ms |  0.96 |    0.01 |  578.1250 |  281.2500 |        - |  3567.73 KB |        1.40 |
| Ef_Load_NoTracking_ToDict     | 50  |  5.675 ms | 0.0294 ms | 0.0245 ms |  0.93 |    0.01 |  437.5000 |  218.7500 |        - |  2712.72 KB |        1.06 |
|                               |     |           |           |           |       |         |           |           |          |             |             |
| Ef_Include_Tracking_ToList    | 100 | 16.313 ms | 0.1381 ms | 0.1153 ms |  1.39 |    0.01 | 1375.0000 |  593.7500 | 218.7500 |  7603.19 KB |        1.49 |
| Ef_Include_NoTracking_ToArray | 100 | 14.627 ms | 0.1139 ms | 0.1010 ms |  1.24 |    0.01 | 1015.6250 |  484.3750 |        - |  6233.39 KB |        1.22 |
| Ef_FromSql_Tracking_ToList    | 100 | 13.398 ms | 0.1340 ms | 0.1253 ms |  1.14 |    0.01 | 1250.0000 |  625.0000 | 250.0000 |  7068.68 KB |        1.38 |
| Ef_Load_Tracking_ToList       | 100 | 12.283 ms | 0.1300 ms | 0.1086 ms |  1.04 |    0.01 | 1250.0000 |  625.0000 | 250.0000 |  7089.18 KB |        1.39 |
| Ef_LoadAsList_Tracking_ToList | 100 | 12.084 ms | 0.1509 ms | 0.1338 ms |  1.03 |    0.01 | 1250.0000 |  625.0000 | 250.0000 |   7095.2 KB |        1.39 |
| Ef_FromSql_NoTracking_ToList  | 100 | 11.852 ms | 0.0919 ms | 0.0859 ms |  1.01 |    0.01 |  875.0000 |  437.5000 |        - |  5364.48 KB |        1.05 |
| Dapper_TwoQuery               | 100 | 11.767 ms | 0.0825 ms | 0.0732 ms |  1.00 |    0.01 |  828.1250 |  406.2500 |        - |  5106.11 KB |        1.00 |
| Ef_Load_NoTracking_ToDict     | 100 | 10.763 ms | 0.2151 ms | 0.2561 ms |  0.91 |    0.02 |  859.3750 |  421.8750 |        - |  5360.32 KB |        1.05 |
|                               |     |           |           |           |       |         |           |           |          |             |             |
| Ef_Include_Tracking_ToList    | 300 | 78.695 ms | 0.6173 ms | 0.5155 ms |  2.70 |    0.03 | 3857.1429 | 1571.4286 | 571.4286 | 22220.41 KB |        1.45 |
| Ef_Include_NoTracking_ToArray | 300 | 77.255 ms | 0.9054 ms | 0.8469 ms |  2.65 |    0.04 | 3428.5714 | 1428.5714 | 428.5714 | 18565.85 KB |        1.21 |
| Ef_FromSql_Tracking_ToList    | 300 | 29.547 ms | 0.2295 ms | 0.2147 ms |  1.01 |    0.01 | 3750.0000 | 1656.2500 | 625.0000 | 20603.29 KB |        1.34 |
| Ef_Load_Tracking_ToList       | 300 | 29.515 ms | 0.2004 ms | 0.1875 ms |  1.01 |    0.01 | 3750.0000 | 1656.2500 | 593.7500 | 20664.22 KB |        1.35 |
| Ef_LoadAsList_Tracking_ToList | 300 | 29.167 ms | 0.2814 ms | 0.2632 ms |  1.00 |    0.01 | 3687.5000 | 1625.0000 | 562.5000 | 20726.78 KB |        1.35 |
| Dapper_TwoQuery               | 300 | 29.158 ms | 0.3155 ms | 0.2951 ms |  1.00 |    0.01 | 2750.0000 | 1250.0000 | 281.2500 | 15341.45 KB |        1.00 |
| Ef_Load_NoTracking_ToDict     | 300 | 29.098 ms | 0.1616 ms | 0.1432 ms |  1.00 |    0.01 | 2906.2500 | 1312.5000 | 406.2500 | 15925.56 KB |        1.04 |
| Ef_FromSql_NoTracking_ToList  | 300 | 29.045 ms | 0.2946 ms | 0.2755 ms |  1.00 |    0.01 | 2875.0000 | 1250.0000 | 375.0000 |  15930.5 KB |        1.04 |

</details><br/>

#### Docker
```

BenchmarkDotNet v0.14.0, Debian GNU/Linux 12 (bookworm) (container)
Unknown processor
.NET SDK 9.0.202
  [Host]   : .NET 9.0.3 (9.0.325.11113), Arm64 RyuJIT AdvSIMD
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), Arm64 RyuJIT AdvSIMD

Job=.NET 9.0  Runtime=.NET 9.0  

```
<details><summary>Подробности</summary>

| Method                        | N   | Mean        | Error       | StdDev      | Median      | Ratio | RatioSD | Gen0      | Gen1      | Gen2     | Allocated   | Alloc Ratio |
|------------------------------ |---- |------------:|------------:|------------:|------------:|------:|--------:|----------:|----------:|---------:|------------:|------------:|
| Ef_Include_Tracking_ToList    | 5   |    830.6 us |    25.05 us |    72.68 us |    809.4 us |  1.85 |    0.16 |   93.7500 |   39.0625 |        - |   434.54 KB |        1.68 |
| Ef_Include_NoTracking_ToArray | 5   |    697.2 us |    13.35 us |    29.03 us |    690.9 us |  1.55 |    0.06 |   85.9375 |   25.3906 |        - |   367.77 KB |        1.42 |
| Ef_FromSql_Tracking_ToList    | 5   |    660.3 us |     5.99 us |     5.60 us |    661.4 us |  1.47 |    0.01 |   91.7969 |   33.2031 |        - |   416.66 KB |        1.61 |
| Ef_LoadAsList_Tracking_ToList | 5   |    629.3 us |    12.18 us |    11.39 us |    630.0 us |  1.40 |    0.03 |   85.9375 |   31.2500 |        - |   414.15 KB |        1.60 |
| Ef_Load_Tracking_ToList       | 5   |    626.8 us |     4.47 us |     3.73 us |    626.5 us |  1.40 |    0.01 |   91.7969 |   33.2031 |        - |   417.91 KB |        1.62 |
| Ef_FromSql_NoTracking_ToList  | 5   |    591.1 us |    11.66 us |    15.56 us |    585.7 us |  1.32 |    0.03 |   72.2656 |   19.5313 |        - |   334.17 KB |        1.29 |
| Ef_Load_NoTracking_ToDict     | 5   |    547.7 us |     7.09 us |     6.29 us |    547.2 us |  1.22 |    0.01 |   74.2188 |   17.5781 |        - |   329.64 KB |        1.28 |
| Dapper_TwoQuery               | 5   |    448.8 us |     2.24 us |     1.99 us |    448.7 us |  1.00 |    0.01 |   61.5234 |   12.6953 |        - |   258.21 KB |        1.00 |
|                               |     |             |             |             |             |       |         |           |           |          |             |             |
| Ef_Include_Tracking_ToList    | 50  |  5,557.7 us |    74.19 us |    69.39 us |  5,561.8 us |  2.60 |    0.04 |  656.2500 |  531.2500 |        - |  3814.83 KB |        1.50 |
| Ef_Include_NoTracking_ToArray | 50  |  4,476.1 us |    42.22 us |    39.50 us |  4,465.1 us |  2.10 |    0.03 |  523.4375 |  500.0000 |        - |  3140.29 KB |        1.23 |
| Ef_FromSql_Tracking_ToList    | 50  |  3,291.2 us |    33.55 us |    31.38 us |  3,291.6 us |  1.54 |    0.02 |  601.5625 |  523.4375 |        - |  3551.74 KB |        1.39 |
| Ef_Load_Tracking_ToList       | 50  |  2,808.0 us |    25.66 us |    24.00 us |  2,815.3 us |  1.31 |    0.02 |  601.5625 |  578.1250 |        - |  3562.05 KB |        1.40 |
| Ef_LoadAsList_Tracking_ToList | 50  |  2,806.5 us |    41.85 us |    39.15 us |  2,800.8 us |  1.31 |    0.02 |  609.3750 |  546.8750 |        - |  3563.34 KB |        1.40 |
| Ef_FromSql_NoTracking_ToList  | 50  |  2,380.0 us |    35.06 us |    32.80 us |  2,371.9 us |  1.11 |    0.02 |  476.5625 |  359.3750 |        - |  2709.54 KB |        1.06 |
| Dapper_TwoQuery               | 50  |  2,135.8 us |    22.25 us |    20.81 us |  2,134.2 us |  1.00 |    0.01 |  472.6563 |  316.4063 |        - |  2547.38 KB |        1.00 |
| Ef_Load_NoTracking_ToDict     | 50  |  1,952.9 us |    13.99 us |    13.08 us |  1,954.7 us |  0.91 |    0.01 |  476.5625 |  375.0000 |        - |  2705.42 KB |        1.06 |
|                               |     |             |             |             |             |       |         |           |           |          |             |             |
| Ef_Include_Tracking_ToList    | 100 | 12,514.1 us |   105.45 us |    98.64 us | 12,526.4 us |  2.96 |    0.04 | 1468.7500 |  937.5000 | 328.1250 |  7598.16 KB |        1.49 |
| Ef_FromSql_Tracking_ToList    | 100 |  8,864.3 us |   164.66 us |   154.02 us |  8,839.7 us |  2.09 |    0.04 | 1343.7500 |  875.0000 | 312.5000 |  7061.22 KB |        1.39 |
| Ef_Include_NoTracking_ToArray | 100 |  7,981.6 us |    87.89 us |    82.21 us |  7,992.1 us |  1.88 |    0.03 | 1000.0000 |  968.7500 |        - |  6221.05 KB |        1.22 |
| Ef_Load_Tracking_ToList       | 100 |  7,769.4 us |   150.62 us |   195.85 us |  7,708.3 us |  1.83 |    0.05 | 1312.5000 |  843.7500 | 281.2500 |   7081.2 KB |        1.39 |
| Ef_LoadAsList_Tracking_ToList | 100 |  7,679.7 us |   119.49 us |   151.11 us |  7,686.1 us |  1.81 |    0.04 | 1343.7500 |  906.2500 | 312.5000 |  7088.61 KB |        1.39 |
| Ef_FromSql_NoTracking_ToList  | 100 |  4,487.1 us |    78.24 us |    73.19 us |  4,460.7 us |  1.06 |    0.02 |  906.2500 |  812.5000 |   7.8125 |  5349.54 KB |        1.05 |
| Dapper_TwoQuery               | 100 |  4,235.1 us |    59.87 us |    56.00 us |  4,218.2 us |  1.00 |    0.02 |  890.6250 |  734.3750 |  15.6250 |  5090.84 KB |        1.00 |
| Ef_Load_NoTracking_ToDict     | 100 |  3,658.7 us |    35.84 us |    33.52 us |  3,663.9 us |  0.86 |    0.01 |  914.0625 |  812.5000 |  15.6250 |   5345.8 KB |        1.05 |
|                               |     |             |             |             |             |       |         |           |           |          |             |             |
| Ef_Include_Tracking_ToList    | 300 | 74,980.0 us | 1,425.59 us | 1,400.12 us | 74,837.1 us |  6.50 |    0.12 | 4000.0000 | 2428.5714 | 857.1429 | 22213.19 KB |        1.45 |
| Ef_Include_NoTracking_ToArray | 300 | 66,724.2 us |   844.10 us |   748.27 us | 66,679.0 us |  5.78 |    0.07 | 3875.0000 | 2750.0000 | 875.0000 | 18541.75 KB |        1.21 |
| Ef_LoadAsList_Tracking_ToList | 300 | 23,453.2 us |   465.64 us |   751.92 us | 23,168.1 us |  2.03 |    0.07 | 4062.5000 | 2531.2500 | 843.7500 |  20717.9 KB |        1.35 |
| Ef_FromSql_Tracking_ToList    | 300 | 22,259.1 us |   359.34 us |   318.54 us | 22,217.8 us |  1.93 |    0.03 | 4000.0000 | 2406.2500 | 812.5000 | 20594.06 KB |        1.35 |
| Ef_Load_Tracking_ToList       | 300 | 22,164.2 us |   265.53 us |   248.38 us | 22,119.4 us |  1.92 |    0.02 | 4000.0000 | 2437.5000 | 812.5000 | 20653.46 KB |        1.35 |
| Ef_Load_NoTracking_ToDict     | 300 | 12,470.5 us |   246.87 us |   346.07 us | 12,429.2 us |  1.08 |    0.03 | 3093.7500 | 1937.5000 | 625.0000 | 15889.36 KB |        1.04 |
| Ef_FromSql_NoTracking_ToList  | 300 | 12,179.4 us |   207.32 us |   183.79 us | 12,170.1 us |  1.06 |    0.02 | 3187.5000 | 1906.2500 | 625.0000 | 15894.15 KB |        1.04 |
| Dapper_TwoQuery               | 300 | 11,541.0 us |    62.04 us |    58.04 us | 11,569.6 us |  1.00 |    0.01 | 3000.0000 | 1984.3750 | 734.3750 | 15302.64 KB |        1.00 |

</details><br/>
