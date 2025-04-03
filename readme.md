## Запуск

#### Поднятие БД
Если нет своего PostgreSQL (иначе поменять строку подключения в коде):

`docker-compose -p softom-benchmarks -f docker-compose.yml up -d db`

#### Создание миграции
Если есть желание поиграться или создать самому с нуля:

`cd src/SoftOm.Benchmarks.Ef`

`dotnet ef migrations add InitialCreate`

#### Запуск тестирования
`cd src/SoftOm.Benchmarks.Ef`

`dotnet run -c Release`


## Результаты
```

BenchmarkDotNet v0.14.0, macOS Sequoia 15.3.2 (24D81) [Darwin 24.3.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK 9.0.202
  [Host]   : .NET 9.0.3 (9.0.325.11113), Arm64 RyuJIT AdvSIMD
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), Arm64 RyuJIT AdvSIMD

Job=.NET 9.0  Runtime=.NET 9.0  

```
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


```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.5608/22H2/2022Update)
AMD Ryzen 5 5600G with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.202
  [Host]   : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2
  .NET 9.0 : .NET 9.0.3 (9.0.325.11113), X64 RyuJIT AVX2

Job=.NET 9.0  Runtime=.NET 9.0  

```
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

