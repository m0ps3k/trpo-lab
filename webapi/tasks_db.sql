CREATE DATABASE tasks_db;

CREATE TABLE "user" (
    id serial NOT NULL PRIMARY KEY,
    email varchar(50) NOT NULL,
    password varchar(255) NOT NULL,
    password_salt varchar(255) NOT NULL,
    first_name varchar(255) NOT NULL,
    last_name varchar(255) NOT NULL,
    ts timestamp NOT NULL,
    is_active boolean NOT NULL
);

CREATE TABLE refresh_token (
    id serial NOT NULL PRIMARY KEY,
    user_id int NOT NULL REFERENCES "user"(id),
    token_hash varchar(1000) NOT NULL,
    token_salt varchar(50) NOT NULL,
    ts timestamp NOT NULL,
    expiry_date timestamp NOT NULL
);

CREATE TABLE task (
    id serial NOT NULL PRIMARY KEY,
    user_id int NOT NULL REFERENCES "user"(id),
    name varchar(100) NOT NULL,
    is_completed boolean NOT NULL,
    ts timestamp NOT NULL
);

-- Test logins (email field): cody / Test12345! and sam / Test12345!
-- Hashing: PBKDF2-HMAC-SHA256, 300000 iterations, 32 bytes, Base64.
INSERT INTO "user" (email, password, password_salt, first_name, last_name, ts, is_active) VALUES
('cody', 'UaQrj3CnC5MvM7s0gtyLycqsq2SmubAAPoREEwMDKcM=', 'SWq22z7XcL80nUNAmyoYRCGLivPzbFYUm4A6karNQ0A=', 'cody', 'coding', TIMESTAMP '2022-01-14 00:00:00', true),
('sam', '/k+hkoql/vZeT8Ccw8JjOwIlem0xKG5qF6j3DC2h9zI=', 'tAMWKtv2xBdBlKYRuSIyQqQ5d2uMOhRpM6CGIZ7kBQ4=', 'sam', 'santosh', TIMESTAMP '2022-01-14 00:00:00', true);

INSERT INTO task (user_id, name, is_completed, ts) VALUES
(1, 'Blog about Access Token and Refresh Token Authentication', true, TIMESTAMP '2022-01-14 00:00:00'),
(1, 'Vacuum the house', false, TIMESTAMP '2022-01-14 00:00:00'),
(1, 'Attend Market Shopping', false, TIMESTAMP '2022-01-13 00:00:00'),
(1, 'Practice Juggling', false, TIMESTAMP '2022-01-15 00:00:00');


--   "email": "test@gmail.com",
--   "password": "test123!",
--   "confirmPassword": "test123!",
--   "firstName": "testName",
--   "lastName": "testSurname"
