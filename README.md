# Управление системой производства пищевых продуктов

## Описание предметной области

База данных представляет систему для производства пищевых продуктов. В ней отслеживаются пользователи, их роли и их заказы. Клиенты могут просматривать продукты, отсортированные по цене и. Каждый продукт имеет описание, цену, путь изображения, информацию о наличии на складе, сроке годности и весе. Компания управляет заказами, отслеживая дату заказа, общую сумму и текущий статус. Система также отслеживает товары в корзине, что позволяет клиентам добавлять продукты в свои корзины перед покупкой. Дополнительно, база данных хранит информацию о пищевой ценности продуктов и возможных аллергенах, а также позволяет пользователям добавлять продукты в избранное.

## Структура базы данных

### CREATE-выражения таблиц

```sql
CREATE TABLE Roles (
  role_id INT IDENTITY(1,1) PRIMARY KEY,
  role_name VARCHAR(50) NOT NULL,
  description TEXT NULL
);

CREATE TABLE Categories (
  category_id INT IDENTITY(1,1) PRIMARY KEY,
  category_name VARCHAR(50) NOT NULL
);

CREATE TABLE Users (
  user_id INT IDENTITY(1,1) PRIMARY KEY,
  email VARCHAR(100) UNIQUE NOT NULL,
  password VARCHAR(255) NOT NULL,
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  role_id INT NOT NULL,
  phone VARCHAR(12) NOT NULL,
  FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);

CREATE TABLE OrderStatuses (
  status_id INT IDENTITY(1,1) PRIMARY KEY,
  status_name VARCHAR(50) NOT NULL
);

CREATE TABLE NutritionFacts (
  nutrition_id INT IDENTITY(1,1) PRIMARY KEY,
  calories INT NOT NULL,
  protein DECIMAL(5,2) NOT NULL,
  carbohydrates DECIMAL(5,2) NOT NULL,
  fats DECIMAL(5,2) NOT NULL
);

CREATE TABLE Allergens (
  allergen_id INT IDENTITY(1,1) PRIMARY KEY,
  allergen_name VARCHAR(50) NOT NULL
);

CREATE TABLE PackagingTypes (
  packaging_id INT IDENTITY(1,1) PRIMARY KEY,
  packaging_name VARCHAR(50) NOT NULL,
  description TEXT NULL
);

CREATE TABLE StorageConditions (
  condition_id INT IDENTITY(1,1) PRIMARY KEY,
  condition_name VARCHAR(50) NOT NULL,
  temperature_range VARCHAR(50) NULL,
  humidity_range VARCHAR(50) NULL
);

CREATE TABLE Products (
  product_id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  description TEXT NULL,
  price DECIMAL(10,2) NOT NULL,
  category_id INT NOT NULL,
  image_url VARCHAR(255) NULL,
  stock INT NOT NULL,
  is_featured TINYINT NOT NULL,
  nutrition_id INT NOT NULL,
  packaging_id INT NOT NULL,
  storage_condition_id INT NOT NULL,
  shelf_life_days INT NOT NULL,
  weight_grams INT NOT NULL,
  FOREIGN KEY (category_id) REFERENCES Categories(category_id),
  FOREIGN KEY (nutrition_id) REFERENCES NutritionFacts(nutrition_id),
  FOREIGN KEY (packaging_id) REFERENCES PackagingTypes(packaging_id),
  FOREIGN KEY (storage_condition_id) REFERENCES StorageConditions(condition_id)
);

CREATE TABLE Orders (
  order_id INT IDENTITY(1,1) PRIMARY KEY,
  user_id INT NOT NULL,
  order_date DATETIME NOT NULL,
  total_amount DECIMAL(10,2) NOT NULL,
  status_id INT NOT NULL,
  shipping_address TEXT NULL,
  FOREIGN KEY (status_id) REFERENCES OrderStatuses(status_id),
  FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

CREATE TABLE CartItems (
  cart_item_id INT IDENTITY(1,1) PRIMARY KEY,
  user_id INT NOT NULL,
  product_id INT NOT NULL,
  quantity INT NOT NULL,
  order_id INT NULL,
  FOREIGN KEY (product_id) REFERENCES Products(product_id),
  FOREIGN KEY (user_id) REFERENCES Users(user_id),
  FOREIGN KEY (order_id) REFERENCES Orders(order_id)
);

CREATE TABLE ProductAllergens (
  product_id INT NOT NULL,
  allergen_id INT NOT NULL,
  PRIMARY KEY (product_id, allergen_id),
  FOREIGN KEY (product_id) REFERENCES Products(product_id),
  FOREIGN KEY (allergen_id) REFERENCES Allergens(allergen_id)
);

CREATE TABLE Favourites (
  favourite_id INT IDENTITY(1,1) PRIMARY KEY,
  user_id INT NOT NULL,
  product_id INT NOT NULL,
  added_date DATETIME NOT NULL,
  FOREIGN KEY (user_id) REFERENCES Users(user_id),
  FOREIGN KEY (product_id) REFERENCES Products(product_id)
);
```

### Тестовые данные
```sql
-- Allergens
INSERT INTO Allergens (allergen_name) VALUES
('Peanuts'),
('Milk'),
('Eggs'),
('Wheat'),
('Soy');

-- Packaging Types
INSERT INTO PackagingTypes (packaging_name, description) VALUES
('Plastic Container', 'Durable plastic container with sealed lid'),
('Cardboard Box', 'Eco-friendly cardboard packaging'),
('Glass Jar', 'Reusable glass jar with twist-off lid');

-- Storage Conditions
INSERT INTO StorageConditions (condition_name, temperature_range, humidity_range) VALUES
('Room Temperature', '20-25°C', '30-50%'),
('Refrigerated', '2-8°C', '80-90%'),
('Frozen', '-18°C or below', 'N/A'),
('Cool and Dry', '10-15°C', '50-60%'),
('Pantry', '15-20°C', '50-70%');

-- Categories
INSERT INTO Categories (category_name) VALUES
('Dairy'),
('Bakery'),
('Fruits and Vegetables'),
('Snacks'),
('Beverages');

-- Nutrition Facts
INSERT INTO NutritionFacts (calories, protein, carbohydrates, fats) VALUES
(120, 5.0, 20.0, 3.5),
(200, 8.0, 25.0, 10.0),
(80, 2.0, 15.0, 1.5),
(150, 6.0, 18.0, 7.0),
(90, 3.0, 12.0, 4.5),
(180, 7.0, 22.0, 9.0),
(100, 4.0, 16.0, 2.5),
(220, 10.0, 28.0, 11.0),
(130, 5.5, 19.0, 5.0),
(170, 7.5, 21.0, 8.0),
(110, 4.5, 17.0, 3.0),
(190, 8.5, 24.0, 9.5),
(140, 6.5, 20.0, 6.0),
(160, 7.0, 23.0, 7.5),
(70, 1.5, 14.0, 1.0);

-- Products
INSERT INTO Products (name, description, price, category_id, image_url, stock, is_featured, nutrition_id, packaging_id, storage_condition_id, shelf_life_days, weight_grams) VALUES
('Creamy Yogurt', 'Delicious probiotic yogurt', 2.99, 1, 'yogurt.jpg', 100, 1, 1, 1, 2, 14, 150),
('Whole Grain Bread', 'Nutritious whole grain bread', 3.49, 2, 'bread.jpg', 50, 0, 2, 2, 1, 7, 500),
('Fresh Apple Juice', 'Freshly squeezed apple juice', 4.99, 5, 'apple_juice.jpg', 75, 1, 3, 3, 2, 10, 1000),
('Mixed Nuts', 'Assorted premium nuts', 6.99, 4, 'mixed_nuts.jpg', 60, 0, 4, 1, 4, 90, 250),
('Organic Spinach', 'Fresh organic spinach leaves', 3.99, 3, 'spinach.jpg', 40, 1, 5, 2, 2, 5, 200),
('Chocolate Chip Cookies', 'Crunchy chocolate chip cookies', 3.79, 4, 'cookies.jpg', 80, 1, 6, 2, 1, 30, 300),
('Low-Fat Milk', 'Creamy low-fat milk', 2.49, 1, 'milk.jpg', 90, 0, 7, 1, 2, 7, 1000),
('Blueberry Muffins', 'Freshly baked blueberry muffins', 4.99, 2, 'muffins.jpg', 30, 1, 8, 2, 1, 5, 360),
('Orange Soda', 'Refreshing orange flavored soda', 1.99, 5, 'orange_soda.jpg', 120, 0, 9, 1, 1, 365, 355),
('Trail Mix', 'Energy-packed trail mix', 5.49, 4, 'trail_mix.jpg', 70, 1, 10, 1, 4, 120, 400),
('Greek Yogurt', 'High-protein Greek yogurt', 3.29, 1, 'greek_yogurt.jpg', 85, 1, 11, 1, 2, 14, 200),
('Sourdough Bread', 'Artisanal sourdough bread', 4.99, 2, 'sourdough.jpg', 40, 0, 12, 2, 1, 5, 450),
('Green Smoothie', 'Healthy green vegetable smoothie', 5.99, 5, 'green_smoothie.jpg', 25, 1, 13, 3, 2, 2, 500),
('Potato Chips', 'Crispy salted potato chips', 2.99, 4, 'potato_chips.jpg', 100, 0, 14, 2, 1, 60, 150),
('Organic Carrots', 'Fresh organic carrots', 2.49, 3, 'carrots.jpg', 75, 1, 15, 2, 2, 14, 500);

-- ProductAllergens
INSERT INTO ProductAllergens (product_id, allergen_id) VALUES
(1, 2), -- Creamy Yogurt contains Milk
(2, 4), -- Whole Grain Bread contains Wheat
(4, 1), -- Mixed Nuts contains Peanuts
(6, 2), -- Chocolate Chip Cookies contain Milk
(6, 3), -- Chocolate Chip Cookies contain Eggs
(6, 4), -- Chocolate Chip Cookies contain Wheat
(7, 2), -- Low-Fat Milk contains Milk
(8, 2), -- Blueberry Muffins contain Milk
(8, 3), -- Blueberry Muffins contain Eggs
(8, 4), -- Blueberry Muffins contain Wheat
(10, 1), -- Trail Mix contains Peanuts
(10, 5), -- Trail Mix contains Soy
(11, 2), -- Greek Yogurt contains Milk
(12, 4), -- Sourdough Bread contains Wheat
(14, 5); -- Potato Chips contain Soy (assuming they're fried in soybean oil)
```

### ER-диаграмма

![ER](attachments/Ssms_AsksyjszP7.png "MSSMS ER Diagram")
