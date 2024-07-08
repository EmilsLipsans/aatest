Create a local database(aadb) and execute these queries

  ```sql
  CREATE TABLE CarBrands (
    id INT PRIMARY KEY IDENTITY,
    brand_name VARCHAR(100) NOT NULL
);

INSERT INTO CarBrands (brand_name) VALUES ('Mercedes');
INSERT INTO CarBrands (brand_name) VALUES ('BMW');
INSERT INTO CarBrands (brand_name) VALUES ('Toyota');
INSERT INTO CarBrands (brand_name) VALUES ('VW');
INSERT INTO CarBrands (brand_name) VALUES ('Ford');

  
CREATE TABLE Cars (
    id INT PRIMARY KEY IDENTITY,
    brand_id INT,
    model VARCHAR(255) NOT NULL,
    year INT NOT NULL,
    in_stock bit NOT NULL,
    FOREIGN KEY (brand_id) REFERENCES dbo.CarBrands(id)
);

INSERT INTO Cars (brand_id, model, year, in_stock) VALUES (1, 'C-Class', 2021, 1);
INSERT INTO Cars (brand_id, model, year, in_stock) VALUES (2, '3 Series', 2022, 0);
INSERT INTO Cars (brand_id, model, year, in_stock) VALUES (3, 'Corolla', 2023, 1);
INSERT INTO Cars (brand_id, model, year, in_stock) VALUES (4, 'Golf', 2021, 1);
INSERT INTO Cars (brand_id, model, year, in_stock) VALUES (5, 'Mustang', 2020, 0);
 ```
For api 
1. If db isn't named aadb modify the connectionId value in serviceDependencies.json
2. In appsettings.json modify the ConnectionStrings Data Source value 

For UI change the API_URL value in Variables.js
  

