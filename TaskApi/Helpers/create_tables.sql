CREATE EXTENSION IF NOT EXISTS citext;


CREATE TABLE IF NOT EXISTS users (

    user_id SERIAL PRIMARY KEY,
    user_name VARCHAR(40),
    user_surname VARCHAR(40),
    user_email CITEXT UNIQUE,
    user_birthdate DATE,
    user_password VARCHAR(40),
    user_created TIMESTAMP DEFAULT (NOW() AT TIME ZONE 'UTC') 
);

CREATE TABLE lists(

    list_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES users(user_id) ON DELETE CASCADE,
    list_name VARCHAR(40) NOT NULL,
    list_description TEXT,
    list_created TIMESTAMP DEFAULT (NOW() AT TIME ZONE 'UTC'),
    UNIQUE (user_id, list_name)
);

CREATE TABLE tasks(
    task_id SERIAL PRIMARY KEY,
    list_id INT REFERENCES lists(list_id) ON DELETE CASCADE,
    task_name VARCHAR(40) NOT NULL,
    task_description TEXT,
    task_created TIMESTAMP DEFAULT (NOW() AT TIME ZONE 'UTC'),
    task_done BOOLEAN DEFAULT FALSE,
    deadline TIMESTAMP,
    UNIQUE (list_id, task_name)
);

-- Insert initial data into USER table
INSERT INTO users (user_name, user_surname, user_email, user_birthdate, user_password, user_created)
VALUES 
('John', 'Doe', 'john.doe@example.com', '1990-01-01', 'password123', NOW()),
('Jane', 'Smith', 'jane.smith@example.com', '1985-05-15', 'securepass', NOW()),
('Alice', 'Johnson', 'alice.johnson@example.com', '1992-07-20', 'alicepass', NOW());

-- Insert initial data into LIST table
INSERT INTO lists (user_id, list_name, list_description, list_created)
VALUES 
(1, 'Groceries', 'Weekly grocery shopping list', NOW()),
(1, 'Work Tasks', 'Tasks related to work projects', NOW()),
(2, 'Travel Plans', 'Planning for upcoming trips', NOW());

-- Insert initial data into TASK table
INSERT INTO tasks (list_id, task_name, task_description, task_created, task_done, deadline)
VALUES 
(1, 'Buy milk', 'Get 2 liters of milk', NOW(), FALSE, '2023-10-10 10:00:00'),
(1, 'Buy bread', 'Get whole grain bread', NOW(), FALSE, '2023-10-10 10:00:00'),
(2, 'Finish report', 'Complete the quarterly report', NOW(), FALSE, '2023-10-15 17:00:00'),
(2, 'Email client', 'Send project update email', NOW(), TRUE, '2023-10-12 09:00:00'),
(3, 'Book flights', 'Book tickets for vacation', NOW(), FALSE, '2023-11-01 12:00:00'),
(3, 'Reserve hotel', 'Make hotel reservations', NOW(), FALSE, '2023-11-05 15:00:00');

