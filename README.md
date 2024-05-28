# Магазин антиквариата

## Описание предметной области

База данных представляет систему для антикварного магазина. В ней отслеживаются пользователи, их роли и их заказы. Клиенты могут просматривать товары, отсортированные по категориям, цветам и размерам. Каждый товар имеет описание, цену, путь изображения и информацию о наличии на складе. Магазин управляет заказами, отслеживая дату заказа, общую сумму и текущий статус. Система также отслеживает товары в корзине, что позволяет клиентам добавлять товары в свои корзины перед покупкой.

## Структура базы данных

### CREATE-выражения таблиц
```sql
CREATE TABLE Categories (
    category_id INT IDENTITY(1,1) PRIMARY KEY,
    category_name VARCHAR(255) NOT NULL
);

CREATE TABLE Colors (
    color_id INT IDENTITY(1,1) PRIMARY KEY,
    color_name VARCHAR(255) NOT NULL
);

CREATE TABLE OrderStatuses (
    status_id INT IDENTITY(1,1) PRIMARY KEY,
    status_name VARCHAR(255) NOT NULL
);

CREATE TABLE Products (
    product_id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10, 2) NOT NULL,
    category_id INT NOT NULL,
    image_url VARCHAR(255),
    stock INT NOT NULL,
    is_featured BIT NOT NULL,
    size_id INT NOT NULL,
    color_id INT NOT NULL,
    FOREIGN KEY (category_id) REFERENCES Categories(category_id),
    FOREIGN KEY (size_id) REFERENCES Sizes(size_id),
    FOREIGN KEY (color_id) REFERENCES Colors(color_id)
);

CREATE TABLE Roles (
    role_id INT IDENTITY(1,1) PRIMARY KEY,
    role_name VARCHAR(255) NOT NULL,
    description TEXT
);

CREATE TABLE Sizes (
    size_id INT IDENTITY(1,1) PRIMARY KEY,
    size_name VARCHAR(255) NOT NULL
);

CREATE TABLE Users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    email VARCHAR(255) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    first_name VARCHAR(255) NOT NULL,
    last_name VARCHAR(255) NOT NULL,
    role_id INT NOT NULL,
    phone VARCHAR(20),
    FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);

CREATE TABLE CartItems (
    cart_item_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

CREATE TABLE Orders (
    order_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    order_date DATE NOT NULL,
    total_amount DECIMAL(10, 2) NOT NULL,
    status_id INT NOT NULL,
    shipping_address VARCHAR(255) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (status_id) REFERENCES OrderStatuses(status_id)
);
```

### Тестовые данные
```sql
INSERT INTO Categories (category_name) VALUES
('Antiques'),
('Furniture'),
('Decorative Arts');

INSERT INTO Colors (color_name) VALUES
('Brown'),
('White'),
('Black');

INSERT INTO Sizes (size_name) VALUES
('Small'),
('Medium'),
('Large');

INSERT INTO Products (name, description, price, category_id, image_url, stock, is_featured, size_id, color_id) VALUES
('Antique Victorian Clock', 'A beautiful antique Victorian clock with intricate details and a delicate chime.', 1500.00, 1, 'clock.jpg', 1, 1, 2, 1),
('Vintage French Armchair', 'A comfortable and stylish vintage French armchair with elegant upholstery.', 800.00, 2, 'armchair.jpg', 2, 1, 1, 2),
('Hand-painted Porcelain Vase', 'A stunning hand-painted porcelain vase with a unique floral design.', 350.00, 3, 'vase.jpg', 5, 0, 3, 3),
('Antique Silver Tea Set', 'A classic antique silver tea set with a timeless design.', 1200.00, 1, 'teaset.jpg', 1, 1, 2, 1),
('Mid-century Modern Coffee Table', 'A stylish mid-century modern coffee table with a sleek design.', 700.00, 2, 'coffeetable.jpg', 3, 0, 2, 2),
('Vintage Glass Chandelier', 'A beautiful vintage glass chandelier with sparkling crystals.', 900.00, 3, 'chandelier.jpg', 2, 1, 1, 3),
('Antique Oak Chest', 'A sturdy antique oak chest with ornate carvings.', 1000.00, 1, 'chest.jpg', 1, 0, 3, 1),
('Art Deco Mirror', 'A stylish Art Deco mirror with a geometric design.', 450.00, 3, 'mirror.jpg', 4, 1, 2, 2),
('Vintage Leather Sofa', 'A comfortable and luxurious vintage leather sofa.', 1800.00, 2, 'sofa.jpg', 1, 0, 1, 1),
('Antique Chinese Porcelain Figurine', 'A delicate antique Chinese porcelain figurine with intricate details.', 250.00, 3, 'figurine.jpg', 3, 1, 3, 3),
('Victorian Side Table', 'A classic Victorian side table with elegant turned legs.', 600.00, 2, 'sidetable.jpg', 2, 0, 2, 1),
('Vintage Enamelware Teapot', 'A charming vintage enamelware teapot with a vibrant color.', 150.00, 1, 'teapot.jpg', 5, 0, 1, 2);
```

### ER-диаграмма

![ER](attachments/Ssms_AsksyjszP7.png "MSSMS ER Diagram")
