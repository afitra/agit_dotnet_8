# simple api _(dotnet 8)_

[![N|Solid](https://cldup.com/dTxpPi9lDf.thumb.png)](https://nodesource.com/products/nsolid)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

#### Testing Machine:

-   MacOs 12.6.7
-   Docker version 24.0.2, build cb74dfc
-   Docker Compose version v2.19.1
-   dotnet sdk 8.0
-   sql server  mcr.microsoft.com/mssql/server:2022-latest
-   redis 7.2.4
-   makefile


 
## Documentation

## Installation

#### Instant install project laravel:

1. clone repository https://github.com/afitra/peasy_test.git
2. cd peasy_test
3. setup appsettings.json, set db uri on DB_URI
4. run make dev
5. jawaban task 1 ada di file `jawaban_task1.txt`
6. jawaban task 2 di http://localhost:5288/api/production/task2/view
7. jawaban task 3 ada di file `jawaban_task3.sql`

api untuk task 1

curl --location 'localhost:5288/api/production/task1' \
--header 'Content-Type: application/json' \
--data '{
"senin": 4,
"selasa": 5,
"rabo": 1,
"kamis": 4,
"jumat": 6
}'



api untuk add data task 2
curl --location 'localhost:5288/api/production/task2' \
--header 'Content-Type: application/json' \
--data '{
"senin": 4,
"selasa": 5,
"rabo": 1,
"kamis": 7,
"jumat": 6,
"sabtu":4,
"minggu":0
}'


cek view data

http://localhost:5288/api/production/task2/view

tidak bisa export db , terjajdi saslah setup



#### Note

-   if there are problems in the invite project github / postman please contact afitra - +6285230010042

### http://apitoong.com
